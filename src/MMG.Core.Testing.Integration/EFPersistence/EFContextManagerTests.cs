// *************************************************
// MMG.Core.Testing.Integration.EFContextManagerTests.cs
// Last Modified: 08/31/2013 1:12 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Linq;
using MMG.Core.Persistence;
using MMG.Core.Persistence.Exceptions;
using MMG.Core.Persistence.Impl;
using MMG.Core.Testing.Integration.Northwind;
using MMG.Infra.EFPersistence;
using NUnit.Framework;

namespace MMG.Core.Testing.Integration.EFPersistence
{
    [TestFixture]
    public class EFContextManagerTests
    {
        [Test]
        public void InitializeManager()
        {
            Utility.DoTimedAction(() => initializeStorage());
            Utility.DoTimedAction(() => verifyInitStorageOnlyOnce());
            Utility.DoTimedAction(() => configureNorthwindContext());
            Utility.DoTimedAction(() => configureSecondContext());
            Utility.DoTimedAction(() => confirmContextCountInStorage());
            Utility.DoTimedAction(() => confirmContextsAreSeparate());
        }

        private static void initializeStorage()
        {
            DbContextInitializer.Instance().InitializeDbContextOnce(() =>
            {
                Assert.Throws<PersistenceException>(() => EFContextManager.Instance.CurrentFor(Utility.NorthwindDBConnectionName));
                var storage = new SimpleDbContextStorage();
                EFContextManager.Instance.InitStorage(storage);
                EFContextManager.Instance.AddContextBuilder(Utility.NorthwindDBConnectionName, new EFContextConfiguration(new[] { "MMG.Core.Testing.Integration" }));
            });

            Assert.IsNotNull(EFContextManager.Instance.Storage);
            Assert.IsInstanceOf<IDbContextStorage>(EFContextManager.Instance.Storage);
        }

        private static void verifyInitStorageOnlyOnce()
        {
            var secondStorage = new SimpleDbContextStorage();
            Assert.Throws<PersistenceException>(() => EFContextManager.Instance.InitStorage(secondStorage));
        }

        private static void configureNorthwindContext()
        {
            var dbContext = EFContextManager.Instance.CurrentFor(Utility.NorthwindDBConnectionName);
            Assert.IsNotNull(dbContext);
            Assert.IsInstanceOf<EFDbContext>(dbContext);
        }

        private static void configureSecondContext()
        {
            Assert.Throws<PersistenceException>(() => EFContextManager.Instance.CurrentFor(Utility.NorthwindAltDBConnectionName));
            EFContextManager.Instance.AddContextBuilder(Utility.NorthwindAltDBConnectionName, new EFContextConfiguration(new[] { "MMG.Core.Testing.Integration" }));
            var dbContext = EFContextManager.Instance.CurrentFor(Utility.NorthwindAltDBConnectionName);
            Assert.IsNotNull(dbContext);
            Assert.IsInstanceOf<EFDbContext>(dbContext);
        }

        private static void confirmContextCountInStorage()
        {
            const int expectedCount = 2;
            var actualCount = EFContextManager.Instance.Storage.GetAllDbContexts().Count();
            Assert.AreEqual(expectedCount, actualCount);
            var context1 = EFContextManager.Instance.Storage.GetAllDbContexts().First();
            var context2 = EFContextManager.Instance.Storage.GetAllDbContexts().Last();
            Assert.AreNotSame(context1, context2);
        }

        private static void confirmContextsAreSeparate()
        {
            var repo1 = new EFGenericRepository(new ConnectionStringProvider(Utility.NorthwindDBConnectionName));
            var repo2 = new EFGenericRepository(new ConnectionStringProvider(Utility.NorthwindAltDBConnectionName));

            var bolidCustRepo1 = repo1.GetByKey<Customer>("BOLID");
            var bolidCustRepo1SameContext = repo1.GetByKey<Customer>("BOLID");

            //the following would fail with 'MMG.Core.Persistence.Exceptions.PersistenceException : The mapping for entity type 'Customer' can not be found.' without the existence of CustomerEFMappingAlt mapping class.
            var bolidCustRepo2 = repo2.GetByKey<Customer>("BOLID");
            Assert.IsNotNull(bolidCustRepo2);
            Assert.IsNotNull(bolidCustRepo1);

            /*bolidCustRepo1.Contact.Address.Region = "Test";
            repo1.Update(bolidCustRepo1);
            repo1.UnitOfWork.SaveChanges();
            Assert.AreEqual(bolidCustRepo1.Contact.Address.Region, bolidCustRepo1SameContext.Contact.Address.Region);
            Assert.AreSame(bolidCustRepo1, bolidCustRepo1SameContext);
            Assert.IsNullOrEmpty(bolidCustRepo2.Contact.Address.Region); //not connected/stateful entity because its from a diff context
            
            var bolidCustRepo2FreshContext = repo2.GetByKey<Customer>("BOLID");
            Assert.IsNotNull(bolidCustRepo2FreshContext);
            Assert.AreNotSame(bolidCustRepo1, bolidCustRepo2FreshContext);

            //revert changes
            bolidCustRepo1.Contact.Address.Region = null;
            repo1.Update(bolidCustRepo1);
            repo1.UnitOfWork.SaveChanges();*/
        }

    }
}