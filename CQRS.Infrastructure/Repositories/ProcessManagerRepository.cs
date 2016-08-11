using RaraAvis.nCubed.Core.Exceptions.Core;
using RaraAvis.nCubed.Core.Expressions;
using RaraAvis.nCubed.Core.Infrastructure.StrategyErrors;
using RaraAvis.nCubed.Core.Messaging.Bus;
using RaraAvis.nCubed.Core.Messaging.StrategyErrors;
using RaraAvis.nCubed.Core.Serialization;
using RaraAvis.nCubed.CQRS.Core.Commands;
using RaraAvis.nCubed.CQRS.Core.Entities;
using RaraAvis.nCubed.CQRS.Core.Exceptions;
using RaraAvis.nCubed.CQRS.Core.ProcessManager;
using RaraAvis.nCubed.CQRS.Core.RepositoryContracts;
using System;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace RaraAvis.nCubed.CQRS.Infrastructure.Repositories
{
    /// <summary>
    /// Data context used to persist instances of <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> (also known as Sagas in the CQRS community) using Entity Framework.
    /// </summary>
    /// <typeparam name="TProcessManager">The process manager to persist.</typeparam>
    [Export(typeof(IProcessManagerRepository<>))]
    public class ProcessManagerRepository<TProcessManager> : IProcessManagerRepository<TProcessManager>
        where TProcessManager : class, IProcessManager
    {
        private readonly IBus<CommandDispatcher, ICommandHandler> commandBus;
        private readonly ITextSerializer serializer;
        private readonly MessagingRandomRetry messagingRandomRetry;
        private readonly SqlIncrementalRetry sqlIncrementalRetry;
        private readonly DbContext context;
        /// <summary>
        /// Constructor for process managers.
        /// </summary>
        /// <param name="context">Context wich process manager resides.</param>
        /// <param name="commandBus">A bus that implements <see cref="T:RaraAvis.nCubed.Core.Messaging.Bus.IBus`2"/>.</param>
        /// <param name="serializer">The serializer for commands.</param>
        [ImportingConstructor]
        public ProcessManagerRepository([Import] DbContext context, [Import]IBus<CommandDispatcher, ICommandHandler> commandBus, [Import]ITextSerializer serializer)
        {
            this.commandBus = commandBus;
            this.serializer = serializer;
            this.context = context;

            messagingRandomRetry = new MessagingRandomRetry();
            sqlIncrementalRetry = new SqlIncrementalRetry();
        }
        /// <summary>
        /// Find the specified process manager.
        /// </summary>
        /// <param name="id">The <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> id.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> object.</returns>
        public TProcessManager Find(Guid id)
        {
            return CoreExceptionProcessor.ProcessInfrastructure<TProcessManager>(
                () =>
                Find(pm => pm.Id == id, true)
            );
        }
        /// <summary>
        /// Find the specified <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/>.
        /// </summary>
        /// <param name="predicate">Searchs predicate based.</param>
        /// <param name="includeCompleted">Include or not completed process managers.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> object.</returns>
        public TProcessManager Find(Expression<Func<TProcessManager, bool>> predicate, bool includeCompleted)
        {
            return CoreExceptionProcessor.ProcessInfrastructure<TProcessManager>(
                () =>
                {
                    TProcessManager pm = null;
                    if (!includeCompleted)
                    {
                        // first try to get the non-completed, in case the table is indexed by Completed, or there is more
                        // than one process manager that fulfills the predicate but only 1 is not completed.
                        this.sqlIncrementalRetry.ExecuteAction(() =>
                            {
                                pm = context.Set<TProcessManager>().Where(predicate.And(x => x.Completed == false)).FirstOrDefault();
                            });
                    }

                    if (pm == null)
                    {
                        this.sqlIncrementalRetry.ExecuteAction(() =>
                        {
                            pm = context.Set<TProcessManager>().Where(predicate).FirstOrDefault();
                        });
                    }

                    if (pm != null)
                    {
                        using (var contextUndispatcher = new CQRSContext())
                        {
                            this.DispatchMessages(pm);
                            if (!pm.Completed || includeCompleted)
                            {
                                return pm;
                            }
                        }
                    }
                    return null;
                });
        }
        /// <summary>
        /// Find the specified <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/>.
        /// </summary>
        /// <param name="predicate">Searchs predicate based.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> object.</returns>
        public TProcessManager Find(Expression<Func<TProcessManager, bool>> predicate)
        {
            return Find(predicate, false);
        }
        /// <summary>
        /// Saves a <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> and publishes the commands in a resilient way.
        /// </summary>
        /// <param name="processManager">The instance to save.</param>
        public void Save(TProcessManager processManager)
        {
            CoreExceptionProcessor.ProcessInfrastructure(() =>
            {
                context.Set<TProcessManager>().Add(processManager);
                this.sqlIncrementalRetry.ExecuteAction(() => { context.SaveChanges(); });
                DispatchMessages(processManager);
            });
        }
        /// <summary>
        /// Updates the state of a <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> and publishes the commands in a resilient way.
        /// </summary>
        /// <param name="processManager">The <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/>.</param>
        public void Update(TProcessManager processManager)
        {
            CoreExceptionProcessor.ProcessInfrastructure(() =>
            {
                context.Set<TProcessManager>().Attach(processManager);
                context.SaveChanges();
                this.sqlIncrementalRetry.ExecuteAction(() => { context.SaveChanges(); });
                DispatchMessages(processManager);
            });
        }
        /// <summary>
        /// Dispatch unprocessed messages.
        /// </summary>
        /// <param name="processManager">Dispatches all process manager commands.</param>
        private void DispatchMessages(IProcessManager processManager)
        {
            if (processManager.Commands != null)
            {
                var undispatched = new UndispatchedMessage(processManager.Id);

                context.Set<UndispatchedMessage>().Add(undispatched);// We create a undispatched object previous processing case of system crash.

                try
                {
                    while (processManager.Commands.Count > 0)
                    {// We process each command.
                        var command = processManager.Commands.Dequeue();
                        messagingRandomRetry.ExecuteAction(() => this.commandBus.Publish(command));
                    }
                }
                catch (Exception ex)
                {// Any kind of exception we recreate commands and update in database.
                    Exception outEx1 = null;
                    CQRSExceptionProcessor.HandleException(ex, out outEx1);

                    undispatched.Commands = this.serializer.Serialize(processManager.Commands);
                    sqlIncrementalRetry.ExecuteAction(() =>
                    {
                        context.SaveChanges();
                    });

                    throw outEx1;
                }
            }
        }
        /// <summary>
        /// Close all resources.
        /// </summary>
        #region ·   IDisposable Members ·
        bool disposed = false;
        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        /// <param name="disposing">False if unmanaged resources must be disposed, false otherwise.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                context.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
