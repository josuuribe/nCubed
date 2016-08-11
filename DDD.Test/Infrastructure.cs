using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaraAvis.nCubed.Core.Exceptions;
using RaraAvis.nCubed.Core.Testing;
using RaraAvis.nCubed.DDD.Core.Specification;
using RaraAvis.nCubed.DDD.Test.FakeObjects;
using RaraAvis.nCubed.DDD.Test.FakeObjects.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DDD.Test
{
    [TestClass]
    public class Infrastructure
    {
        static TestAction testAction = null;
        static FakePersonRepository fpr = new FakePersonRepository();
        //static FakeUoW fuow = new FakeUoW();

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            testAction = new TestAction(context);
            testAction.UseRegisterTestDbSet<Person>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            testAction.TestInitialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // runs after every test
            testAction.TestCleanup();
            fpr.Dispose();
        }

        [ClassCleanup]
        public static void ClassCleanUp()
        {
            // runs after the last test in this class

            testAction.ClassCleanup();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "DbContext can not be null.")]
        public void DDDNullEntityRepository()
        {
            fpr = new FakePersonRepository(null);
        }

        [TestMethod]
        public void DDDAdd()
        {
            int expected = 1;
            int actual = 0;

            Person p = new Person();

            fpr.Add(p);
            actual = fpr.CountPerson();

            Assert.AreEqual(expected, actual, "Different actual and expected adding");
        }

        [TestMethod]
        [ExpectedException(typeof(SemanticException), "Expected ArgumentNullException")]
        public void DDDErrorAddingNullEntity()
        {
            Person p = null;

            fpr.Add(p);
        }

        [TestMethod]
        public void DDDRemove()
        {
            testAction.UseEntry<Person>();

            int expected = 0;
            int actual = 0;

            Person p = new Person();
            fpr.Add(p);
            fpr.Remove(p);
            actual = fpr.CountPerson();

            Assert.AreEqual(expected, actual, "Different actual and expected removing");
        }

        [TestMethod]
        [ExpectedException(typeof(SemanticException), "Expected ArgumentNullException")]
        public void DDDErrorRemovingNullEntity()
        {
            Person p = null;

            fpr.Remove(p);
        }

        [TestMethod]
        public void DDDGet()
        {
            testAction.UseFind<Person>((data, parameters) =>
                {
                    object firstElement = parameters[0];
                    if (firstElement is System.Guid)
                    {
                        Guid id = (System.Guid)firstElement;
                        return data.Where(x => x.EntityId == id).FirstOrDefault();
                    }
                    else if (firstElement is int)
                    {
                        int id = (int)firstElement;
                        return data.Where(x => x.Id == id).FirstOrDefault();
                    }
                    else if (firstElement is string)
                    {
                        string id = (string)firstElement;
                        return data.Where(x => x.Name == id).FirstOrDefault();
                    }
                    else if (firstElement is long)
                    {
                        long id = (long)firstElement;
                        return data.Where(x => x.IdLong == id).FirstOrDefault();
                    }
                    return null;
                });

            int peopleNumber = 1;
            Person p1 = new Person();
            p1.GenerateNewIdentity();
            p1.Id = 1;
            p1.IdLong = 2L;
            p1.Name = "Name";
            fpr.Add(p1);

            Person p2 = fpr.GetById(p1.EntityId);
            Assert.IsNotNull(p2, "Person not found by Guid.");

            Person p3 = fpr.GetById(p1.Id);
            Assert.IsNotNull(p3, "Person not found by Id.");

            Person p4 = fpr.GetById(p1.Name);
            Assert.IsNotNull(p4, "Person not found by Name.");

            Person p5 = fpr.GetById(p1.IdLong);
            Assert.IsNotNull(p5, "Person not found by IdLong.");

            Person p6 = fpr.GetById(Guid.Empty);
            Assert.IsNull(p6, "Person must be null by empty Guid.");

            IEnumerable<Person> people = fpr.All();
            Assert.AreEqual(people.ToList().Count, peopleNumber, "People set does not mismatch.");
        }

        [TestMethod]
        public void DDDMatchesAll()
        {
            Expression<Func<Person, bool>> spec = s => s.EntityId != Guid.Empty;
            ISpecification<Person> AdHocSpecification = new DirectSpecification<Person>(spec);
            int expected = 1;

            Person p1 = new Person();
            p1.GenerateNewIdentity();
            Person p2 = new Person();
            Person p3 = new Person();

            fpr.Add(p1);
            fpr.Add(p2);
            fpr.Add(p3);

            var result = fpr.AllMatching(AdHocSpecification);

            Assert.AreEqual(result.Count(), expected, "Criteria error");
        }

        [TestMethod]
        public void DDDPaged()
        {
            Person p1 = new Person();
            p1.Name = "1";
            Person p2 = new Person();
            p2.Name = "2";

            fpr.Add(p1);
            fpr.Add(p2);

            var resultAsc = fpr.GetPaged<string>(0, 100, p => p.Name, true).ToList();

            Assert.IsTrue(resultAsc[0].Name == "1", "First element error.");
            Assert.IsTrue(resultAsc[1].Name == "2", "Second element error.");

            var resultDesc = fpr.GetPaged<string>(0, 100, p => p.Name, false).ToList();

            Assert.IsTrue(resultDesc[0].Name == "2", "First element error.");
            Assert.IsTrue(resultDesc[1].Name == "1", "Second element error.");

        }

        [TestMethod]
        public void DDDFiltered()
        {
            Expression<Func<Person, bool>> spec = s => s.EntityId == Guid.Empty;
            int expected = 3;

            Person p1 = new Person();
            p1.Name = "1";
            Person p2 = new Person();
            p2.Name = "2";
            Person p3 = new Person();
            p3.Name = "3";

            fpr.Add(p1);
            fpr.Add(p2);
            fpr.Add(p3);

            var result = fpr.GetFiltered(spec);

            Assert.AreEqual(result.Count(), expected, "Filtered error.");
        }

        [TestMethod]
        public void DDDApplyCurrentValues()
        {
            testAction.UseEntry<Person>();

            Person p1 = new Person();
            p1.Name = "1";

            Person p2 = new Person();
            p2.Name = "2";

            fpr.Merge(p1, p2);

            Assert.AreEqual(p1.Name, p2.Name, "Names equals.");
        }

        [TestMethod]
        public void DDDTrackItem()
        {
            testAction.UseEntry<Person>();

            EntityState expected = System.Data.Entity.EntityState.Unchanged;

            Person p = new Person();
            fpr.TrackItem(p);

            Assert.AreEqual((fpr.UnitOfWork as DbContext).Entry(p).State, expected, "Track item.");
        }

        [TestMethod]
        [ExpectedException(typeof(SemanticException), "Expected ArgumentNullException")]
        public void DDDErrorTrackingNullEntity()
        {
            Person p = null;
            fpr.TrackItem(p);
        }

        [TestMethod]
        public void DDDModify()
        {
            testAction.UseEntry<Person>();

            EntityState expected = System.Data.Entity.EntityState.Modified;

            Person p = new Person();
            fpr.Modify(p);

            Assert.AreEqual((fpr.UnitOfWork as DbContext).Entry(p).State, expected, "Modify item.");
        }

        [TestMethod]
        [ExpectedException(typeof(SemanticException), "Expected ArgumentNullException")]
        public void DDDErrorModifyNullEntity()
        {
            Person p = null;
            fpr.Modify(p);
        }
    }
}
