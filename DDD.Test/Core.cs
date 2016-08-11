using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaraAvis.nCubed.Core.Containers.DI;
using RaraAvis.nCubed.Core.Exceptions;
using RaraAvis.nCubed.DDD.Core;
using RaraAvis.nCubed.DDD.Core.Exceptions;
using RaraAvis.nCubed.DDD.Core.Services;
using RaraAvis.nCubed.DDD.Core.Specification;
using RaraAvis.nCubed.DDD.Test.FakeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Test
{
    [TestClass]
    public class Core
    {
        [ClassInitialize]
        public static void OnClassInitialize(TestContext context)
        {

        }

        [ClassCleanup]
        public static void OnClassCleanup()
        {
            // runs after the last test in this class
        }

        public PersonDTO CustomConvertToDTO(PersonDTO command, object state)
        {
            command.Name = "As Dto";
            return command;
        }

        public Person CustomConvertToEntity(Person command, object state)
        {
            command.Name = "As Entity";
            return command;
        }

        [TestMethod]
        public void DDDApplicationExceptionCheckExceptionsStructReturnValue()
        {
            int result = DDDExceptionProcessor.ProcessApplication<int>(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception)
                {
                    return 1;
                }
            });
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void DDDApplicationExceptionCheckExceptionsObjectReturnValue()
        {
            object result = DDDExceptionProcessor.ProcessApplication<object>(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception)
                {
                    return null;
                }
            });
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void DDDInfrastructureExceptionCheckExceptionsStructReturnValue()
        {
            int result = DDDExceptionProcessor.ProcessDataAccess<int>(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch(Exception)
                {
                    return 1;
                }
            });
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void DDDInfrastructureExceptionCheckExceptionsObjectReturnValue()
        {
            object result = DDDExceptionProcessor.ProcessDataAccess<object>(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch(Exception)
                {
                    return null;
                }
            });
            Assert.AreEqual(null, result);
        }


        [TestMethod]
        public void DDDInfrastructureExceptionCheckExceptions()
        {
            DDDExceptionProcessor.ProcessDataAccess(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    Exception outEx = null;
                    DDDExceptionProcessor.HandleExceptionDataAccess(ex, out outEx);
                    Assert.AreNotEqual(ex, outEx);
                }
            });
        }

        [TestMethod]
        public void DDDDomainExceptionCheckExceptions()
        {
            DDDExceptionProcessor.ProcessDomain(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    Exception outEx = null;
                    DDDExceptionProcessor.HandleExceptionDomain(ex, out outEx);
                    DDDExceptionProcessor.HandleExceptionDomain(ex);
                    Assert.AreNotEqual(ex, outEx);
                }
            });
        }

        [TestMethod]
        public void DDDDomainExceptionCheckExceptionsStructReturnValue()
        {
            DDDExceptionProcessor.ProcessDomain<int>(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    Exception outEx = null;
                    DDDExceptionProcessor.HandleExceptionDomain(ex, out outEx);
                    DDDExceptionProcessor.HandleExceptionDomain(ex);
                    Assert.AreNotEqual(ex, outEx);
                }
                return 0;
            });
        }

        [TestMethod]
        public void DDDDomainExceptionCheckExceptionsObjectReturnValue()
        {
            DDDExceptionProcessor.ProcessDomain<object>(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    Exception outEx = null;
                    DDDExceptionProcessor.HandleExceptionDomain(ex, out outEx);
                    DDDExceptionProcessor.HandleExceptionDomain(ex);
                    Assert.AreNotEqual(ex, outEx);
                }
                return null;
            });
        }

        [TestMethod]
        public void DDDDomainExceptionCheckExceptionsObjectReturnValueDefaultValue()
        {
            DDDExceptionProcessor.ProcessDomain<object>(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    Exception outEx = null;
                    DDDExceptionProcessor.HandleExceptionDomain(ex, out outEx);
                    DDDExceptionProcessor.HandleExceptionDomain(ex);
                    Assert.AreNotEqual(ex, outEx);
                }
                return null;
            }, null);
        }

        [TestMethod]
        public void DDDDomainExceptionCheckExceptionsStructReturnValueDefaultValue()
        {
            DDDExceptionProcessor.ProcessDomain<int>(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    Exception outEx = null;
                    DDDExceptionProcessor.HandleExceptionDomain(ex, out outEx);
                    DDDExceptionProcessor.HandleExceptionDomain(ex);
                    Assert.AreNotEqual(ex, outEx);
                }
                return 0;
            }, 0);
        }

        [TestMethod]
        public void DDDApplicationExceptionCheckExceptions()
        {
            DDDExceptionProcessor.ProcessApplication(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    Exception outEx = null;
                    DDDExceptionProcessor.HandleExceptionApplication(ex, out outEx);
                    Assert.AreNotEqual(ex, outEx);
                }
            });
        }

        [TestMethod]
        public void DDDWebServerExceptionCheckExceptions()
        {
            DDDExceptionProcessor.ProcessWebserver(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    Exception outEx = null;
                    DDDExceptionProcessor.HandleExceptionWebserver(ex);
                    Assert.AreNotEqual(ex, outEx);
                }
            });
        }

        [TestMethod]
        public void DDDWebServerExceptionCheckExceptionsStructReturnValue()
        {
            DDDExceptionProcessor.ProcessWebserver<int>(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    Exception outEx = null;
                    DDDExceptionProcessor.HandleExceptionWebserver(ex);
                    Assert.AreNotEqual(ex, outEx);
                }
                return 0;
            });
        }

        [TestMethod]
        public void DDDWebServerExceptionCheckExceptionsObjectReturnValue()
        {
            DDDExceptionProcessor.ProcessWebserver<object>(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    Exception outEx = null;
                    DDDExceptionProcessor.HandleExceptionWebserver(ex);
                    Assert.AreNotEqual(ex, outEx);
                }
                return null;
            });
        }

        [TestMethod]
        public void DDDWebServerExceptionCheckExceptionsStructReturnValueDefaultValue()
        {
            DDDExceptionProcessor.ProcessWebserver<int>(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    Exception outEx = null;
                    DDDExceptionProcessor.HandleExceptionWebserver(ex);
                    Assert.AreNotEqual(ex, outEx);
                }
                return 0;
            }, 0);
        }

        [TestMethod]
        public void DDDWebServerExceptionCheckExceptionsObjectReturnValueDefaultValue()
        {
            DDDExceptionProcessor.ProcessWebserver<object>(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    Exception outEx = null;
                    DDDExceptionProcessor.HandleExceptionWebserver(ex);
                    Assert.AreNotEqual(ex, outEx);
                }
                return null;
            }, null);
        }

        [TestMethod]
        public void DDDTestProjections()
        {
            PersonDTO fcDto = new PersonDTO();
            fcDto.Name = "Testing";
            Person fc = fcDto.ProjectAsEntity<PersonDTO, Person>();
            Assert.AreEqual<string>(fc.Name, fcDto.Name, "Same Projection Id To DTO");


            fc.Name = "String changed";
            fcDto = fc.ProjectAsDto<Person, PersonDTO>();
            Assert.AreEqual<string>(fc.Name, fcDto.Name, "Same Projection Id From DTO");


            fc.Name = "Testing collection";
            List<Person> commands = new List<Person>();
            commands.Add(fc);
            var dtos = new List<PersonDTO>(commands.ProjectAsDto<Person, PersonDTO>());
            Assert.AreEqual<string>(commands[0].Name, dtos[0].Name, "Same Projection Id To Collection DTO");


            fcDto.Name = "String collection changed";
            List<PersonDTO> fakeDTOs = new List<PersonDTO>();
            fakeDTOs.Add(fcDto);
            List<Person> Persons = new List<Person>(fakeDTOs.ProjectAsEntity<PersonDTO, Person>());
            Assert.AreEqual<string>(commands[0].Name, dtos[0].Name, "Same Projection Id From Collection DTO");


            var fcdto = fc.ProjectAsDto<Person, PersonDTO>(CustomConvertToDTO);
            Assert.AreEqual<string>(fcdto.Name, "As Dto", "Same custom Projection string From DTO");

            fc = fcdto.ProjectAsEntity<PersonDTO, Person>(CustomConvertToEntity);
            Assert.AreEqual<string>(fc.Name, "As Entity", "Same custom Projection string From command");

            dtos = new List<PersonDTO>(commands.ProjectAsDto<Person, PersonDTO>(CustomConvertToDTO));
            Assert.AreEqual<string>(dtos[0].Name, "As Dto", "Same custom Projection string From DTO");

            Persons = new List<Person>(fakeDTOs.ProjectAsEntity<PersonDTO, Person>(CustomConvertToEntity));
            Assert.AreEqual<string>(Persons[0].Name, "As Entity", "Same custom Projection Id From Collection DTO");
        }

        [TestMethod()]
        public void DDDCreateObject()
        {
            Person p = null;

            p = SystemFactory<DDDContainer>.Container.Constructor.CreateRequiredObject<Person>();
            Assert.IsNotNull(p);
        }

        #region ·   Entities    ·

        [TestMethod()]
        public void DDDCheckTransientHashCode()
        {
            SampleEntity entity = new SampleEntity(Guid.NewGuid());

            Assert.IsTrue(entity.GetHashCode() != 0);
        }

        [TestMethod()]
        public void DDDCheckHashCode()
        {
            SampleEntity entity = new SampleEntity(Guid.Empty);
            int hashCode = entity.GetHashCode();

            Assert.IsTrue(entity.GetHashCode() != 0);
        }

        [TestMethod()]
        public void DDDSameIdentityProduceEqualsTrueTest()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            var entityLeft = new SampleEntity();
            var entityRight = new SampleEntity();

            entityLeft.ChangeCurrentIdentity(id);
            entityRight.ChangeCurrentIdentity(id);

            //Act
            bool resultOnEquals = entityLeft.Equals(entityRight);
            bool resultOnOperator = entityLeft == entityRight;

            //Assert
            Assert.IsTrue(resultOnEquals);
            Assert.IsTrue(resultOnOperator);

        }
        [TestMethod()]
        public void DDDDiferentIdProduceEqualsFalseTest()
        {
            //Arrange

            var entityLeft = new SampleEntity();
            var entityRight = new SampleEntity();

            entityLeft.GenerateNewIdentity();
            entityRight.GenerateNewIdentity();


            //Act
            bool resultOnEquals = entityLeft.Equals(entityRight);
            bool resultOnOperator = entityLeft == entityRight;

            //Assert
            Assert.IsFalse(resultOnEquals);
            Assert.IsFalse(resultOnOperator);

        }
        [TestMethod()]
        public void DDDCompareUsingEqualsOperatorsAndNullOperandsTest()
        {
            //Arrange

            SampleEntity entityLeft = null;
            SampleEntity entityRight = new SampleEntity();

            entityRight.GenerateNewIdentity();

            //Act
            if (!(entityLeft == (Entity)null))//this perform ==(left,right)
                Assert.Fail();

            if (!(entityRight != (Entity)null))//this perform !=(left,right)
                Assert.Fail();

            if (!((Entity)null == entityLeft))//this perform ==(left,right)
                Assert.Fail();

            if (!((Entity)null != entityRight))//this perform !=(left,right)
                Assert.Fail();

            entityRight = null;

            //Act
            if (!(entityLeft == entityRight))//this perform ==(left,right)
                Assert.Fail();

            if (entityLeft != entityRight)//this perform !=(left,right)
                Assert.Fail();


        }
        [TestMethod()]
        public void DDDCompareTheSameReferenceReturnTrueTest()
        {
            //Arrange
            var entityLeft = new SampleEntity();
            SampleEntity entityRight = entityLeft;


            //Act
            if (!entityLeft.Equals(entityRight))
                Assert.Fail();

            if (!(entityLeft == entityRight))
                Assert.Fail();

        }
        [TestMethod()]
        public void DDDCompareWhenLeftIsNullAndRightIsNullReturnFalseTest()
        {
            //Arrange

            SampleEntity entityLeft = null;
            var entityRight = new SampleEntity();

            entityRight.GenerateNewIdentity();

            //Act
            if (!(entityLeft == (Entity)null))//this perform ==(left,right)
                Assert.Fail();

            if (!(entityRight != (Entity)null))//this perform !=(left,right)
                Assert.Fail();
        }

        [TestMethod()]
        public void DDDSetIdentitySetANonTransientEntity()
        {
            //Arrange
            var entity = new SampleEntity();

            //Act
            entity.GenerateNewIdentity();

            //Assert
            Assert.IsFalse(entity.IsTransient);
        }

        [TestMethod()]
        public void DDDChangeIdentitySetNewIdentity()
        {
            //Arrange
            var entity = new SampleEntity();
            entity.GenerateNewIdentity();

            //act
            Guid expected = entity.EntityId;
            entity.ChangeCurrentIdentity(Guid.NewGuid());

            //assert
            Assert.AreNotEqual(expected, entity.EntityId);
        }

        [TestMethod()]
        public void DDDNotEqualsIfTransient()
        {
            var entity1 = new SampleEntity();
            var entity2 = new SampleEntity();

            Assert.IsTrue(entity1 != entity2);
        }
        #endregion

        #region ·   Specification   ·

        [TestMethod]
        public void DDDCreateNewDirectSpecificationTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> adHocSpecification;
            Expression<Func<SampleEntity, bool>> spec = s => s.EntityId == Guid.NewGuid();

            //Act
            adHocSpecification = new DirectSpecification<SampleEntity>(spec);

            //Assert
            Assert.ReferenceEquals(new PrivateObject(adHocSpecification).GetField("matchingCriteria"), spec);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DDDCreateDirectSpecificationNullSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> adHocSpecification;
            Expression<Func<SampleEntity, bool>> spec = null;

            //Act
            adHocSpecification = new DirectSpecification<SampleEntity>(spec);
        }
        [TestMethod()]
        public void DDDCreateAndSpecificationTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.EntityId == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            AndSpecification<SampleEntity> composite = new AndSpecification<SampleEntity>(leftAdHocSpecification, rightAdHocSpecification);

            //Assert
            Assert.IsNotNull(composite.SatisfiedBy());
            Assert.ReferenceEquals(leftAdHocSpecification, composite.LeftSideSpecification);
            Assert.ReferenceEquals(rightAdHocSpecification, composite.RightSideSpecification);

            var list = new List<SampleEntity>();
            var sampleA = new SampleEntity() { SampleProperty = "1" };
            sampleA.ChangeCurrentIdentity(identifier);

            var sampleB = new SampleEntity() { SampleProperty = "the sample property" };
            sampleB.ChangeCurrentIdentity(identifier);

            list.AddRange(new SampleEntity[] { sampleA, sampleB });


            var result = list.AsQueryable().Where(composite.SatisfiedBy()).ToList();

            Assert.IsTrue(result.Count == 1);
        }
        [TestMethod()]
        public void DDDCreateOrSpecificationTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.EntityId == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            OrSpecification<SampleEntity> composite = new OrSpecification<SampleEntity>(leftAdHocSpecification, rightAdHocSpecification);

            //Assert
            Assert.IsNotNull(composite.SatisfiedBy());
            Assert.ReferenceEquals(leftAdHocSpecification, composite.LeftSideSpecification);
            Assert.ReferenceEquals(rightAdHocSpecification, composite.RightSideSpecification);

            var list = new List<SampleEntity>();

            var sampleA = new SampleEntity() { SampleProperty = "1" };
            sampleA.ChangeCurrentIdentity(identifier);

            var sampleB = new SampleEntity() { SampleProperty = "the sample property" };
            sampleB.GenerateNewIdentity();

            list.AddRange(new SampleEntity[] { sampleA, sampleB });


            var result = list.AsQueryable().Where(composite.SatisfiedBy()).ToList();

            Assert.IsTrue(result.Count() == 2);



        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DDDCreateAndSpecificationNullLeftSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            Expression<Func<SampleEntity, bool>> leftSpec = s => s.EntityId == Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            AndSpecification<SampleEntity> composite = new AndSpecification<SampleEntity>(null, rightAdHocSpecification);

        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DDDCreateAndSpecificationNullRightSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            Expression<Func<SampleEntity, bool>> rightSpec = s => s.EntityId == Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            AndSpecification<SampleEntity> composite = new AndSpecification<SampleEntity>(leftAdHocSpecification, null);

        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DDDCreateOrSpecificationNullLeftSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            Expression<Func<SampleEntity, bool>> leftSpec = s => s.EntityId == Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            OrSpecification<SampleEntity> composite = new OrSpecification<SampleEntity>(null, rightAdHocSpecification);

        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DDDCreateOrSpecificationNullRightSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            Expression<Func<SampleEntity, bool>> rightSpec = s => s.EntityId == Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            OrSpecification<SampleEntity> composite = new OrSpecification<SampleEntity>(leftAdHocSpecification, null);

        }
        [TestMethod]
        public void DDDUseSpecificationLogicAndOperatorTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.EntityId == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;


            //Act
            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            ISpecification<SampleEntity> andSpec = leftAdHocSpecification & rightAdHocSpecification;

            //Assert

            var list = new List<SampleEntity>();
            var sampleA = new SampleEntity() { SampleProperty = "1" };
            sampleA.ChangeCurrentIdentity(identifier);

            var sampleB = new SampleEntity() { SampleProperty = "the sample property" };
            sampleB.GenerateNewIdentity();

            var sampleC = new SampleEntity() { SampleProperty = "the sample property" };
            sampleC.ChangeCurrentIdentity(identifier);

            list.AddRange(new SampleEntity[] { sampleA, sampleB, sampleC });

            var result = list.AsQueryable().Where(andSpec.SatisfiedBy()).ToList();

            Assert.IsTrue(result.Count == 1);
        }
        [TestMethod]
        public void DDDUseSpecificationAndOperatorTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.EntityId == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;


            //Act
            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            ISpecification<SampleEntity> andSpec = leftAdHocSpecification && rightAdHocSpecification;

            var list = new List<SampleEntity>();

            var sampleA = new SampleEntity() { SampleProperty = "1" };
            sampleA.ChangeCurrentIdentity(identifier);

            var sampleB = new SampleEntity() { SampleProperty = "the sample property" };
            sampleB.GenerateNewIdentity();

            var sampleC = new SampleEntity() { SampleProperty = "the sample property" };
            sampleC.ChangeCurrentIdentity(identifier);

            list.AddRange(new SampleEntity[] { sampleA, sampleB, sampleC });


            var result = list.AsQueryable().Where(andSpec.SatisfiedBy()).ToList();

            Assert.IsTrue(result.Count == 1);

        }
        [TestMethod]
        public void DDDUseSpecificationBitwiseOrOperatorTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.EntityId == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;


            //Act
            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            var orSpec = leftAdHocSpecification | rightAdHocSpecification;


            //Assert
            var list = new List<SampleEntity>();

            var sampleA = new SampleEntity() { SampleProperty = "1" };
            sampleA.ChangeCurrentIdentity(identifier);

            var sampleB = new SampleEntity() { SampleProperty = "the sample property" };
            sampleB.GenerateNewIdentity();

            list.AddRange(new SampleEntity[] { sampleA, sampleB });

            var result = list.AsQueryable().Where(orSpec.SatisfiedBy()).ToList();
        }
        [TestMethod]
        public void DDDUseSpecificationOrOperatorTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.EntityId == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;


            //Act
            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            var orSpec = leftAdHocSpecification || rightAdHocSpecification;


            //Assert
            var list = new List<SampleEntity>();
            var sampleA = new SampleEntity() { SampleProperty = "1" };
            sampleA.ChangeCurrentIdentity(identifier);

            var sampleB = new SampleEntity() { SampleProperty = "the sample property" };
            sampleB.GenerateNewIdentity();

            list.AddRange(new SampleEntity[] { sampleA, sampleB });

            var result = list.AsQueryable().Where(orSpec.SatisfiedBy()).ToList();

            Assert.IsTrue(result.Count() == 2);
        }
        [TestMethod()]
        public void DDDCreateNotSpecificationithSpecificationTest()
        {
            //Arrange
            Expression<Func<SampleEntity, bool>> specificationCriteria = t => t.EntityId == Guid.NewGuid();
            DirectSpecification<SampleEntity> specification = new DirectSpecification<SampleEntity>(specificationCriteria);

            //Act
            NotSpecification<SampleEntity> notSpec = new NotSpecification<SampleEntity>(specification);
            Expression<Func<SampleEntity, bool>> resultCriteria = new PrivateObject(notSpec).GetField("_OriginalCriteria") as Expression<Func<SampleEntity, bool>>;

            //Assert
            Assert.IsNotNull(notSpec);
            Assert.IsNotNull(resultCriteria);
            Assert.IsNotNull(notSpec.SatisfiedBy());

        }
        [TestMethod()]
        public void DDDCreateNotSpecificationWithCriteriaTest()
        {
            //Arrange
            Expression<Func<SampleEntity, bool>> specificationCriteria = t => t.EntityId == Guid.NewGuid();


            //Act
            NotSpecification<SampleEntity> notSpec = new NotSpecification<SampleEntity>(specificationCriteria);

            //Assert
            Assert.IsNotNull(notSpec);
            Assert.IsNotNull(new PrivateObject(notSpec).GetField("_OriginalCriteria"));
        }
        [TestMethod()]
        public void DDDCreateNotSpecificationFromNegationOperator()
        {
            //Arrange
            Expression<Func<SampleEntity, bool>> specificationCriteria = t => t.EntityId == Guid.NewGuid();


            //Act
            DirectSpecification<SampleEntity> spec = new DirectSpecification<SampleEntity>(specificationCriteria);
            ISpecification<SampleEntity> notSpec = !spec;

            //Assert
            Assert.IsNotNull(notSpec);

        }
        [TestMethod()]
        public void DDDCheckNotSpecificationOperators()
        {
            //Arrange
            Expression<Func<SampleEntity, bool>> specificationCriteria = t => t.EntityId == Guid.NewGuid();


            //Act
            Specification<SampleEntity> spec = new DirectSpecification<SampleEntity>(specificationCriteria);
            Specification<SampleEntity> notSpec = !spec;
            ISpecification<SampleEntity> resultAnd = notSpec && spec;
            ISpecification<SampleEntity> resultOr = notSpec || spec;

            //Assert
            Assert.IsNotNull(notSpec);
            Assert.IsNotNull(resultAnd);
            Assert.IsNotNull(resultOr);

        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DDDCreateNotSpecificationNullSpecificationThrowArgumentNullExceptionTest()
        {
            //Arrange
            NotSpecification<SampleEntity> notSpec;

            //Act
            notSpec = new NotSpecification<SampleEntity>((ISpecification<SampleEntity>)null);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DDDCreateNotSpecificationNullCriteriaThrowArgumentNullExceptionTest()
        {
            //Arrange
            NotSpecification<SampleEntity> notSpec;

            //Act
            notSpec = new NotSpecification<SampleEntity>((Expression<Func<SampleEntity, bool>>)null);
        }
        [TestMethod()]
        public void DDDCreateTrueSpecificationTest()
        {
            //Arrange
            ISpecification<SampleEntity> trueSpec = new TrueSpecification<SampleEntity>();
            bool expected = true;
            bool actual = trueSpec.SatisfiedBy().Compile()(new SampleEntity());
            //Assert
            Assert.IsNotNull(trueSpec);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ·   ValueObjects  ·
        [TestMethod()]
        public void DDDIdenticalDataEqualsIsTrueTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");

            //Act
            bool resultEquals = address1.Equals(address2);
            bool resultEqualsSimetric = address2.Equals(address1);
            bool resultEqualsOnThis = address1.Equals(address1);

            //Assert
            Assert.IsTrue(resultEquals);
            Assert.IsTrue(resultEqualsSimetric);
            Assert.IsTrue(resultEqualsOnThis);
        }

        [TestMethod()]
        public void DDDIdenticalDataEqualOperatorIsTrueTest()
        {
            //Arraneg
            Address address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address1Repeated = address1;

            //Act
            bool resultEquals = (address1 == address2);
            bool resultEqualsSimetric = (address2 == address1);
            bool resultEqualsOnThis = (address1 == address1Repeated);

            //Assert
            Assert.IsTrue(resultEquals);
            Assert.IsTrue(resultEqualsSimetric);
            Assert.IsTrue(resultEqualsOnThis);
        }

        [TestMethod()]
        public void DDDIdenticalDataIsNotEqualOperatorIsFalseTest()
        {
            //Arraneg
            Address address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address1Repeated = address1;

            //Act
            bool resultEquals = (address1 != address2);
            bool resultEqualsSimetric = (address2 != address1);
            bool resultEqualsOnThis = (address1 != address1Repeated);

            //Assert
            Assert.IsFalse(resultEquals);
            Assert.IsFalse(resultEqualsSimetric);
            Assert.IsFalse(resultEqualsOnThis);
        }

        [TestMethod()]
        public void DDDDiferentDataEqualsIsFalseTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address2 = new Address("streetLine2", "streetLine1", "city", "zipcode");

            //Act
            bool result = address1.Equals(address2);
            bool resultSimetric = address2.Equals(address1);

            //Assert
            Assert.IsFalse(result);
            Assert.IsFalse(resultSimetric);
        }

        [TestMethod()]
        public void DDDDiferentDataIsNotEqualOperatorIsTrueTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address2 = new Address("streetLine2", "streetLine1", "city", "zipcode");

            //Act
            bool result = (address1 != address2);
            bool resultSimetric = (address2 != address1);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(resultSimetric);
        }

        [TestMethod()]
        public void DDDDiferentDataEqualOperatorIsFalseTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address2 = new Address("streetLine2", "streetLine1", "city", "zipcode");

            //Act
            bool result = (address1 == address2);
            bool resultSimetric = (address2 == address1);

            //Assert
            Assert.IsFalse(result);
            Assert.IsFalse(resultSimetric);
        }

        [TestMethod()]
        public void DDDSameDataInDiferentPropertiesIsEqualsFalseTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", null, null);
            Address address2 = new Address("streetLine2", "streetLine1", null, null);

            //Act
            bool result = address1.Equals(address2);


            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void DDDSameDataInDiferentPropertiesEqualOperatorFalseTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", null, null);
            Address address2 = new Address("streetLine2", "streetLine1", null, null);

            //Act
            bool result = (address1 == address2);


            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void DDDDiferentDataInDiferentPropertiesProduceDiferentHashCodeTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", null, null);
            Address address2 = new Address("streetLine2", "streetLine1", null, null);

            //Act
            int address1HashCode = address1.GetHashCode();
            int address2HashCode = address2.GetHashCode();


            //Assert
            Assert.AreNotEqual(address1HashCode, address2HashCode);
        }
        [TestMethod()]
        public void DDDSameDataInDiferentPropertiesProduceDiferentHashCodeTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", null, null, "streetLine1");
            Address address2 = new Address(null, "streetLine1", "streetLine1", null);

            //Act
            int address1HashCode = address1.GetHashCode();
            int address2HashCode = address2.GetHashCode();


            //Assert
            Assert.AreNotEqual(address1HashCode, address2HashCode);
        }
        [TestMethod()]
        public void DDDSameReferenceEqualsTrueTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", null, null, "streetLine1");
            Address address2 = address1;


            //Act
            if (!address1.Equals(address2))
                Assert.Fail();

            if (!(address1 == address2))
                Assert.Fail();

        }
        [TestMethod()]
        public void DDDSameDataSameHashCodeTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", null, null);
            Address address2 = new Address("streetLine1", "streetLine2", null, null);

            //Act
            int address1HashCode = address1.GetHashCode();
            int address2HashCode = address2.GetHashCode();


            //Assert
            Assert.AreEqual(address1HashCode, address2HashCode);
        }

        [TestMethod()]
        public void DDDSelfReferenceNotProduceInfiniteLoop()
        {
            //Arrange
            SelfReference aReference = new SelfReference();
            SelfReference bReference = new SelfReference();

            //Act
            aReference.Value = bReference;
            bReference.Value = aReference;

            //Assert

            Assert.AreNotEqual(aReference, bReference);
        }

        [TestMethod()]
        public void DDDCheckValueObjectSideIsNull()
        {
            //Arrange
            Address address1 = new Address(string.Empty, string.Empty, string.Empty, string.Empty);
            Address address2 = null;

            //Assert
            Assert.IsTrue(address1 != address2);

            Assert.IsTrue(address2 != address1);

            address1 = null;

            Assert.IsTrue(address2 == address1);

            Assert.IsTrue(address1 == address2);
        }

        [TestMethod()]
        public void DDDCheckValueObjectWithNoPublicPropertiesSide()
        {
            //Arrange
            NoAddress address1 = new NoAddress();
            NoAddress address2 = new NoAddress();

            //Assert
            Assert.IsTrue(address1 == address2);

            Assert.IsTrue(address2 == address1);
        }

        [TestMethod()]
        public void DDDCheckValueObjectWithNoPublicPropertiesSideIsNull()
        {
            //Arrange
            NoAddress address1 = new NoAddress();
            NoAddress address2 = null;

            //Assert
            Assert.IsTrue(address1 != address2);

            Assert.IsTrue(address2 != address1);

            address1 = null;

            Assert.IsTrue(address2 == address1);

            Assert.IsTrue(address1 == address2);
        }
        #endregion
    }
}
