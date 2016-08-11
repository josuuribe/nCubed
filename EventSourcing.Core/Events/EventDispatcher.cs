using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Messaging.Messages;
using RaraAvis.nCubed.EventSourcing.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RaraAvis.nCubed.EventSourcing.Core.Events
{
    /// <summary>
    /// Class for dispatch events.
    /// </summary>
    public sealed class EventDispatcher : IDispatcher<IEventHandler>
    {
        private Dictionary<Type, List<Tuple<Type, Action<Envelope>>>> handlersByEventType;
        private Dictionary<Type, Action<IEvent, string, string>> dispatchersByEventType;
        /// <summary>
        /// Creates an EventDispatcher object.
        /// </summary>
        public EventDispatcher()
        {
            this.handlersByEventType = new Dictionary<Type, List<Tuple<Type, Action<Envelope>>>>();
            this.dispatchersByEventType = new Dictionary<Type, Action<IEvent, string, string>>();
        }
        /// <summary>
        /// Registers a handler.
        /// </summary>
        /// <param name="handler">Handles an <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEventHandler"/>.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Error is intentional")]
        public void Register(IEventHandler handler)
        {
            var handlerType = handler.GetType();

            foreach (var invocationTuple in BuildHandlerInvocations(handler))
            {
                List<Tuple<Type, Action<Envelope>>> invocations;
                if (!this.handlersByEventType.TryGetValue(invocationTuple.Item1, out invocations))
                {
                    invocations = new List<Tuple<Type, Action<Envelope>>>();
                    this.handlersByEventType[invocationTuple.Item1] = invocations;
                }
                invocations.Add(new Tuple<Type, Action<Envelope>>(handlerType, invocationTuple.Item2));

                if (!this.dispatchersByEventType.ContainsKey(invocationTuple.Item1))
                {
                    this.dispatchersByEventType[invocationTuple.Item1] = this.BuildDispatchInvocation(invocationTuple.Item1);
                }
            }
        }
        /// <summary>
        /// Dispatchs a message.
        /// </summary>
        /// <typeparam name="T">Message type to be processed.</typeparam>
        /// <param name="message">Message to be processed.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Error is intentional, otherwise constructor is implicit and with no default arguments, always must be a body.")]
        public void DispatchMessage<T>(Envelope<T> message) where T : IMessage
        {
            IEvent @event = message.Body as IEvent;
            Action<IEvent, string, string> dispatch;
            if (this.dispatchersByEventType.TryGetValue(message.Body.GetType(), out dispatch))
            {
                dispatch(@event, message.MessageId, message.CorrelationId);
            }
            // Invoke also the generic handlers that have registered to handle IEvent directly.
            if (this.dispatchersByEventType.TryGetValue(typeof(IEvent), out dispatch))
            {
                dispatch(@event, message.MessageId, message.CorrelationId);
            }
        }
        /// <summary>
        /// Dispatchs a message.
        /// </summary>
        /// <typeparam name="T">Message type to be processed.</typeparam>
        /// <param name="event">Event to be processed.</param>
        /// <param name="messageId">Message to be processed.</param>
        /// <param name="correlationId">Correlation Id.</param>
        private void DoDispatchMessage<T>(T @event, string messageId, string correlationId)
            where T : IEvent
        {
            var envelope = Envelope.Create(@event);
            envelope.MessageId = messageId;
            envelope.CorrelationId = correlationId;

            List<Tuple<Type, Action<Envelope>>> handlers;
            if (this.handlersByEventType.TryGetValue(typeof(T), out handlers))
            {
                foreach (var handler in handlers)
                {
                    handler.Item2(envelope);
                }
            }
        }
        private static IEnumerable<Tuple<Type, Action<Envelope>>> BuildHandlerInvocations(IEventHandler handler)
        {
            var interfaces = handler.GetType().GetInterfaces();

            var eventHandlerInvocations =
                interfaces
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                    .Select(i => new { HandlerInterface = i, EventType = i.GetGenericArguments()[0] })
                    .Select(e => new Tuple<Type, Action<Envelope>>(e.EventType, BuildHandlerInvocation(handler, e.HandlerInterface, e.EventType)));

            var envelopedEventHandlerInvocations =
                interfaces
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnvelopedEventHandler<>))
                    .Select(i => new { HandlerInterface = i, EventType = i.GetGenericArguments()[0] })
                    .Select(e => new Tuple<Type, Action<Envelope>>(e.EventType, BuildEnvelopeHandlerInvocation(handler, e.HandlerInterface, e.EventType)));

            return eventHandlerInvocations.Union(envelopedEventHandlerInvocations);
        }
        private static Action<Envelope> BuildHandlerInvocation(IEventHandler handler, Type handlerType, Type messageType)
        {
            var envelopeType = typeof(Envelope<>).MakeGenericType(messageType);

            var parameter = Expression.Parameter(typeof(Envelope));
            var invocationExpression =
                Expression.Lambda(
                    Expression.Block(
                        Expression.Call(
                            Expression.Convert(Expression.Constant(handler), handlerType),
                            handlerType.GetMethod("Handle"),
                            Expression.Property(Expression.Convert(parameter, envelopeType), "Body"))),
                    parameter);

            return (Action<Envelope>)invocationExpression.Compile();
        }
        private static Action<Envelope> BuildEnvelopeHandlerInvocation(IEventHandler handler, Type handlerType, Type messageType)
        {
            var envelopeType = typeof(Envelope<>).MakeGenericType(messageType);

            var parameter = Expression.Parameter(typeof(Envelope));
            var invocationExpression =
                Expression.Lambda(
                    Expression.Block(
                        Expression.Call(
                            Expression.Convert(Expression.Constant(handler), handlerType),
                            handlerType.GetMethod("Handle"),
                            Expression.Convert(parameter, envelopeType))),
                    parameter);

            return (Action<Envelope>)invocationExpression.Compile();
        }
        private Action<IEvent, string, string> BuildDispatchInvocation(Type eventType)
        {
            var eventParameter = Expression.Parameter(typeof(IEvent));
            var messageIdParameter = Expression.Parameter(typeof(string));
            var correlationIdParameter = Expression.Parameter(typeof(string));

            var dispatchExpression =
                Expression.Lambda(
                    Expression.Block(
                        Expression.Call(
                            Expression.Constant(this),
                            this.GetType().GetMethod("DoDispatchMessage", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(eventType),
                            Expression.Convert(eventParameter, eventType),
                            messageIdParameter,
                            correlationIdParameter)),
                    eventParameter,
                    messageIdParameter,
                    correlationIdParameter);

            return (Action<IEvent, string, string>)dispatchExpression.Compile();
        }
    }
}

