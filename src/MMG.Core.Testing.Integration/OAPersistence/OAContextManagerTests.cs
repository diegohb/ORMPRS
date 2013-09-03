// *************************************************
// MMG.Core.Testing.Integration.OAContextManagerTests.cs
// Last Modified: 08/31/2013 5:07 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Linq;
using MMG.Core.OAPersistence;
using MMG.Core.Persistence;
using MMG.Core.Persistence.Exceptions;
using MMG.Core.Persistence.Impl;
using MMG.Core.Testing.Integration.Northwind;
using NUnit.Framework;

namespace MMG.Core.Testing.Integration.OAPersistence
{
    [TestFixture]
    public class OAContextManagerTests
    {
        [Test]
        public void InitializeManager()
        {
            Utility.DoTimedAction(() => initializeStorage());
            Utility.DoTimedAction(() => verifyInitStorageOnlyOnce());
            Utility.DoTimedAction(() => configureNorthwindContext());
            /*Utility.DoTimedAction(() => configureSecondContext());
            Utility.DoTimedAction(() => confirmContextCountInStorage());*/
            Utility.DoTimedAction(() => confirmContextsAreSeparate());
        }

        private static void initializeStorage()
        {
            Assert.IsNull(OAContextManager.Instance.Storage);
            var storage = new SimpleDbContextStorage();
            OAContextManager.Instance.InitStorage(storage);
            Assert.IsNotNull(OAContextManager.Instance.Storage);
            Assert.IsInstanceOf<IDbContextStorage>(OAContextManager.Instance.Storage);
        }

        private static void verifyInitStorageOnlyOnce()
        {
            var secondStorage = new SimpleDbContextStorage();
            Assert.Throws<PersistenceException>(() => OAContextManager.Instance.InitStorage(secondStorage));
        }

        private static void configureNorthwindContext()
        {
            Assert.Throws<PersistenceException>(() => OAContextManager.Instance.CurrentFor(Utility.NorthwindDBConnectionName));
            OAContextManager.Instance.AddContextBuilder
                (Utility.NorthwindDBConnectionName, new OAContextConfiguration(new[] {"MMG.Core.Testing.Integration"}) {BackendConfig = "mssql"});
            var dbContext = OAContextManager.Instance.CurrentFor(Utility.NorthwindDBConnectionName);
            Assert.IsNotNull(dbContext);
            Assert.IsInstanceOf<OADbContext>(dbContext);
        }

        private static void configureSecondContext()
        {
            Assert.Throws<PersistenceException>(() => OAContextManager.Instance.CurrentFor(Utility.NorthwindAltDBConnectionName));
            OAContextManager.Instance.AddContextBuilder
                (Utility.NorthwindAltDBConnectionName, new OAContextConfiguration(new[] {"MMG.Core.Testing.Integration"}) {BackendConfig = "mssql"});
            var dbContext = OAContextManager.Instance.CurrentFor(Utility.NorthwindAltDBConnectionName);
            Assert.IsNotNull(dbContext);
            Assert.IsInstanceOf<OADbContext>(dbContext);
        }

        private static void confirmContextCountInStorage()
        {
            const int expectedCount = 2;
            var actualCount = OAContextManager.Instance.Storage.GetAllDbContexts().Count();
            Assert.AreEqual(expectedCount, actualCount);
            var context1 = OAContextManager.Instance.Storage.GetAllDbContexts().First();
            var context2 = OAContextManager.Instance.Storage.GetAllDbContexts().Last();
            Assert.AreNotSame(context1, context2);
        }

        private static void confirmContextsAreSeparate()
        {
            var repo1 = new OAGenericRepository(Utility.NorthwindDBConnectionName);
            var repo2 = new OAGenericRepository(Utility.NorthwindAltDBConnectionName);

            var bolidCustRepo1 = repo1.GetByKey<Customer>("BOLID");
            var bolidCustRepo1SameContext = repo1.GetByKey<Customer>("BOLID");
            Assert.IsNotNull(bolidCustRepo1);
            Assert.IsNotNull(bolidCustRepo1SameContext);

            var bolidCustRepo2 = repo2.GetByKey<Customer>("BOLID");
            Assert.IsNotNull(bolidCustRepo2);
            

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