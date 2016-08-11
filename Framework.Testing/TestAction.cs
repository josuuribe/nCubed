using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaraAvis.nCubed.Core.Testing.Infrastructure.TestDbSet;
using RaraAvis.nCubed.Core.Testing.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Fakes;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Fakes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Testing
{
    /// <summary>
    /// Classs that implements several actions for testing.
    /// </summary>
    public class TestAction : ITestAction
    {
        private static IDictionary<string, ITestDb> registeredSets = new Dictionary<string, ITestDb>();
        private static IList<ITestDb> preservedSets = new List<ITestDb>();
        private static IDisposable shimsContext = null;
        private int triesBeforeError = 0;
        private TestContext context;
        private EntityState state = EntityState.Unchanged;
        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="context">Testing context.</param>
        public TestAction(TestContext context)
        {
            shimsContext = ShimsContext.Create();
            this.context = context;
        }
        /// <summary>
        /// Replace for <see cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute" />
        /// </summary>
        public void ClassInitialize()
        {
        }
        /// <summary>
        /// Replace for <see cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute" />, it disposes shims context.
        /// </summary>
        public void ClassCleanup()
        {
            shimsContext.Dispose();
        }
        /// <summary>
        /// Replace for <see cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute" />, it fakes <see cref="M:System.Data.Entity.DbContext.SaveChanges"/>.
        /// </summary>
        public void TestInitialize()
        {
            ShimDbContext.AllInstances.SaveChanges = (ctx) =>
            {//It must be set at each test due to WithExceptionSavingDbSet(), it changes the behaviour.
                return 0;
            };
        }
        /// <summary>
        /// Replace for <see cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute" />, it clears sets if are not marked to preserve.
        /// </summary>
        public void TestCleanup()
        {
            foreach (var itest in registeredSets)
            {
                if (!preservedSets.Contains(itest.Value))
                {
                    itest.Value.Clear();
                }
            }
        }
        /// <summary>
        /// Gets or sets a <see cref="T:System.Data.Entity.DbSet`1"/>.
        /// </summary>
        /// <typeparam name="T">Type to create set.</typeparam>
        /// <returns>A <see cref="T:System.Data.Entity.DbSet`1"/>.</returns>
        public IDbSet<T> GetSet<T>() where T : class, new()
        {
            return registeredSets[typeof(T).Name] as InMemoryDbSet<T>;
        }
        /// <summary>
        /// Use the context given, not the fake one.
        /// </summary>
        /// <typeparam name="T">A <see cref="T:System.Data.Entity.DbContext"/> to use.</typeparam>
        /// <returns>The action object.</returns>
        public ITestAction UseRealContext<T>() where T : DbContext, new()
        {
            ShimDbContext.Constructor = (ctx) => { ctx = new T(); };
            return this;
        }
        /// <summary>
        /// Fakes <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1"/>.
        /// </summary>
        /// <typeparam name="T">Type entry to fake.</typeparam>
        /// <returns>The action object.</returns>
        public ITestAction UseEntry<T>() where T : class
        {
            state = EntityState.Unchanged;
            ShimDbContext.AllInstances.EntryOf1M0<T>((ctx, entity) =>
                {
                    ShimDbEntityEntry<T> entry = new ShimDbEntityEntry<T>();
                    entry.EntityGet = () => { return entity; };
                    entry.StateGet = () => { return state; };
                    entry.StateSetEntityState = (stateNew) => { state = stateNew; };

                    ShimDbPropertyValues spv = new ShimDbPropertyValues();

                    spv.SetValuesObject = (obj) =>
                    {
                        foreach (var property in typeof(T).GetProperties())
                        {
                            if (property.CanWrite)
                                property.SetValue(obj, property.GetValue(entity));
                        }
                    };

                    spv.PropertyNamesGet = () =>
                    {
                        List<string> propertyNames = new List<string>();
                        foreach (var property in typeof(T).GetType().GetProperties())
                        {
                            propertyNames.Add(property.Name);
                        }
                        return propertyNames;
                    };

                    spv.SetValuesDbPropertyValues = (values) =>
                        {

                        };

                    entry.CurrentValuesGet = () => { return spv; };
                    System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues.AllInstances.SetValuesObject = (value, obj) => { };

                    return entry;
                });
            return this;
        }
        /// <summary>
        /// Use specified find to search entity.
        /// </summary>
        /// <typeparam name="T">Type entry to fake.</typeparam>
        /// <param name="func">The func object.</param>
        /// <returns>The action object.</returns>
        public ITestAction UseFind<T>(Func<IEnumerable<dynamic>, object[], object> func) where T : class
        {
            registeredSets[typeof(T).Name].RegisterFind(func);
            return this;
        }
        /// <summary>
        /// Registers a set of data.
        /// </summary>
        /// <typeparam name="T">The entity type to register set.</typeparam>
        /// <returns>The action object.</returns>
        public ITestAction UseRegisterTestDbSet<T>() where T : class, new()
        {
            InMemoryDbSet<T> eventSet = new InMemoryDbSet<T>();

            if (!registeredSets.ContainsKey(typeof(T).Name))
            {
                registeredSets.Add(typeof(T).Name, eventSet);

                ShimDbContext.AllInstances.SetOf1<T>((p) =>
                {
                    return registeredSets[typeof(T).Name] as DbSet<T>;
                });
            }
            return this;
        }
        /// <summary>
        /// Mark repository as preserved so these are not cleared.
        /// </summary>
        /// <param name="testSet">Test set to preserve.</param>
        /// <returns>The action object.</returns>
        public ITestAction MarkPreserve(ITestDb testSet)
        {
            preservedSets.Add(testSet);
            return this;
        }
        /// <summary>
        /// Marks a <see cref="M:System.Data.Entity.DbContext.SaveChanges"/> to raise an exception.
        /// </summary>
        /// <typeparam name="T">Exception type to raise.</typeparam>
        /// <param name="tryCount">Number of <see cref="M:System.Data.Entity.DbContext.SaveChanges"/> before raise exception.</param>
        /// <param name="retryError">If error must be rethrown.</param>
        /// <returns>The action object.</returns>
        public ITestAction MarkExceptionSavingDbSet<T>(int tryCount, bool retryError) where T : Exception, new()
        {
            triesBeforeError = 0;
            ShimDbContext.AllInstances.SaveChanges = (ctx) =>
            {
                triesBeforeError++;
                if (triesBeforeError == tryCount)
                {
                    if (retryError)
                        triesBeforeError--;//Case of retry, go back, same error
                    throw new T();
                }
                return 0;
            };
            return this;
        }
        /// <summary>
        /// Fake <see cref="T:System.Data.Entity.Infrastructure.DbChangeTracker" />.
        /// </summary>
        /// <typeparam name="T">A <see cref="T:System.Data.Entity.Infrastructure.DbChangeTracker" /> related entity.</typeparam>
        /// <returns>The action object.</returns>
        public ITestAction UseChangeTracker<T>() where T : class
        {
            ShimDbChangeTracker sct = new ShimDbChangeTracker();

            ShimDbContext.AllInstances.ChangeTrackerGet = (ctx) =>
                {
                    return sct;
                };

            sct.Entries = () =>
            {
                return new List<DbEntityEntry>();
            };

            EntityState st = EntityState.Unchanged;

            ShimDbEntityEntry.AllInstances.StateSetEntityState = (entry, state) => { st = state; };
            ShimDbEntityEntry.AllInstances.StateGet = (entry) => { return st; };
            return this;
        }
    }
}
