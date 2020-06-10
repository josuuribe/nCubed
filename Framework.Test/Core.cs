using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaraAvis.nCubed.Core.Containers.DI;
using RaraAvis.nCubed.Core.Infrastructure.StrategyErrors;
using RaraAvis.nCubed.Core.Logging;
using RaraAvis.nCubed.Core.Messaging.StrategyErrors;
using RaraAvis.nCubed.Core.StrategyErrors;
using RaraAvis.nCubed.Core.Test.FakeObjects;
using System;
using System.Diagnostics.Tracing;

namespace RaraAvis.nCubed.Core.Test
{
    [TestClass]
    public class Core
    {
        private static IDisposable shimsContext = null;

        [ClassInitialize]
        public static void Initialize(TestContext tc)
        {
            /*
            shimsContext = ShimsContext.Create();

            N3Section n3Section = new N3Section();
            n3Section.System = new SystemSection();
            n3Section.System.RetryConfiguration = new Configurations.Common.Sections.RetrySection();

            n3Section.System.RetryConfiguration.ProcessConfiguration = new Configurations.Common.Sections.Retries.ProcessRetrySection();
            n3Section.System.RetryConfiguration.ProcessConfiguration.RetryName = "Retry";
            n3Section.System.RetryConfiguration.ProcessConfiguration.RetryCount = 3;
            n3Section.System.RetryConfiguration.ProcessConfiguration.RetryInterval = 10;
            n3Section.System.RetryConfiguration.ProcessConfiguration.FirstFastRetry = true;

            n3Section.System.RetryConfiguration.DatabaseConfiguration = new Configurations.Common.Sections.Retries.DatabaseRetrySection();
            n3Section.System.RetryConfiguration.DatabaseConfiguration.RetryName = "Retry";
            n3Section.System.RetryConfiguration.DatabaseConfiguration.RetryCount = 3;
            n3Section.System.RetryConfiguration.DatabaseConfiguration.InitialRetry = 1;
            n3Section.System.RetryConfiguration.DatabaseConfiguration.IncrementalRetry = 1;
            n3Section.System.RetryConfiguration.DatabaseConfiguration.FirstFastRetry = true;

            n3Section.System.RetryConfiguration.MessagingConfiguration.RetryName = "Retry";
            n3Section.System.RetryConfiguration.MessagingConfiguration.RetryCount = 3;
            n3Section.System.RetryConfiguration.MessagingConfiguration.MinBackOff = 1;
            n3Section.System.RetryConfiguration.MessagingConfiguration.MaxBackOff = 3;
            n3Section.System.RetryConfiguration.MessagingConfiguration.FirstFastRetry = true;
            
            ShimConfigurationManager.GetSectionString = (name) =>
                {
                    return n3Section as ConfigurationSection;
                };
                */
        }


        [TestMethod]
        public void RetryApplication()
        {
            ProcessFixedRetry pfr = new ProcessFixedRetry();

            int i = 0;

            pfr.ExecuteAction(() =>
                {
                    if (i < 3)
                    {
                        i++;
                        throw new Exception("Retry " + i);
                    }
                });

            Assert.AreEqual(i, 3);
        }

        [TestMethod]
        public void RetryDatabase()
        {
            SqlIncrementalRetry sql = new SqlIncrementalRetry();

            int i = 0;

            sql.ExecuteAction(() =>
            {
                if (i < 3)
                {
                    i++;
                    throw new TimeoutException("Retry " + i);
                }
            });

            Assert.AreEqual(i, 3);
        }

        [TestMethod]
        public void RetryMessaging()
        {
            MessagingRandomRetry messaging = new MessagingRandomRetry();

            int i = 0;

            messaging.ExecuteAction(() =>
            {
                if (i < 3)
                {
                    i++;
                    throw new Exception("Retry " + i);
                }
            });

            Assert.AreEqual(i, 3);
        }

        [TestMethod]
        public void TestDummyInjection()
        {
            var dummy = SystemFactory<CommonContainer>.Container.Constructor.CreateObject<IDummy>();
            var echo = "Echo";
            dummy.Echo = "Echo";
            Assert.AreEqual(echo, dummy.Echo, "Can not say " + echo + ".");
        }

        [TestMethod]
        public void InsertLog()
        {
            string expected = "Test Message";

            var listener1 = new ObservableEventListener();
            listener1.EnableEvents("RaraAvis-N3-Core-Logging", EventLevel.LogAlways);
            TestObserver to = new TestObserver();
            listener1.Subscribe(to);

            FrameworkLogging.LogMessage("Test Message");

            Assert.AreEqual(to.Entry.FormattedMessage, expected, "The log message is not correct.");
        }

        [TestMethod]
        public void InsertLogWithCorrelationId()
        {
            string expected = "Test Message";

            var listener1 = new ObservableEventListener();
            listener1.EnableEvents("RaraAvis-N3-Core-Logging", EventLevel.LogAlways);
            TestObserver to = new TestObserver();
            listener1.Subscribe(to);

            FrameworkLogging.LogMessage(Guid.NewGuid(), "Test Message");

            Assert.AreEqual(to.Entry.FormattedMessage, expected, "The log message is not correct.");
        }

        [ClassCleanup]
        public static void Destroy()
        {
            shimsContext.Dispose();
        }
    }
}
