// *************************************************
// MMG.Core.Testing.Integration.EFContextManagerTests.cs
// Last Modified: 08/31/2013 1:12 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using MMG.Core.Persistence;
using MMG.Core.Persistence.Exceptions;
using MMG.Core.Persistence.Impl;
using MMG.Infra.EFPersistence;
using NUnit.Framework;

namespace MMG.Core.Testing.Integration.EFPersistence
{
    [TestFixture]
    public class EFContextManagerTests
    {
        private const string northwindDBConnectionName = "NorthwindDB";

        [Test]
        public void InitializeManager()
        {
            doAction(() => initializeStorage());
            doAction(() => configureNorthwindContext());
            doAction(()=> configureSecondContext());
            doAction(()=> confirmContextCountInStorage());
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
            Assert.Throws<PersistenceException>(() => EFContextManager.Instance.CurrentFor(northwindDBConnectionName));
            EFContextManager.Instance.AddContextBuilder(northwindDBConnectionName, new EFContextConfiguration());
            var dbContext = EFContextManager.Instance.CurrentFor(northwindDBConnectionName);
            Assert.IsNotNull(dbContext);
            Assert.IsInstanceOf<EFDbContext>(dbContext);
        }

        private static void configureSecondContext()
        {
            const string altDBName = northwindDBConnectionName + "Alt";
            Assert.Throws<PersistenceException>(() => EFContextManager.Instance.CurrentFor(altDBName));
            EFContextManager.Instance.AddContextBuilder(altDBName, new EFContextConfiguration());
            var dbContext = EFContextManager.Instance.CurrentFor(altDBName);
            Assert.IsNotNull(dbContext);
            Assert.IsInstanceOf<EFDbContext>(dbContext);
        }

        private static void confirmContextCountInStorage()
        {
            const int expectedCount = 2;
            var actualCount = EFContextManager.Instance.Storage.GetAllDbContexts().Count();
            Assert.AreEqual(expectedCount, actualCount);
        }

        protected static void doAction(Expression<Action> pAction)
        {
            Console.Write("Executing {0} ... ", pAction.Body);
            var timer = new Stopwatch();
            var act = pAction.Compile();
            timer.Start();
            act.Invoke();
            timer.Stop();
            Console.WriteLine("Executed successfully in {0} ms.", timer.ElapsedMilliseconds);
        }
    }
}