﻿// *************************************************
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
            Utility.DoTimedAction(() => configureNorthwindContext());
            Utility.DoTimedAction(() => configureSecondContext());
            Utility.DoTimedAction(() => confirmContextCountInStorage());
            Utility.DoTimedAction(() => confirmContextsAreSeparate());
        }

        private static void initializeStorage()
        {
            Assert.IsNull(EFContextManager.Instance.Storage);
            var storage = new SimpleDbContextStorage();
            EFContextManager.Instance.InitStorage(storage);
            Assert.IsNotNull(EFContextManager.Instance.Storage);
            Assert.IsInstanceOf<IDbContextStorage>(EFContextManager.Instance.Storage);
        }

        private static void configureNorthwindContext()
        {
            Assert.Throws<PersistenceException>(() => EFContextManager.Instance.CurrentFor(Utility.NorthwindDBConnectionName));
            EFContextManager.Instance.AddContextBuilder(Utility.NorthwindDBConnectionName, new EFContextConfiguration(new[] { "MMG.Core.Testing.Integration" }));
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
            var repo1 = new EFGenericRepository(Utility.NorthwindDBConnectionName);
            var repo2 = new EFGenericRepository(Utility.NorthwindAltDBConnectionName);

            var bolidCustRepo1 = repo1.GetByKey<Customer>("BOLID");
            var bolidCustRepo2 = repo2.GetByKey<Customer>("BOLID");
            
            Assert.IsNotNull(bolidCustRepo1);
            Assert.IsNotNull(bolidCustRepo2);

        }

    }
}