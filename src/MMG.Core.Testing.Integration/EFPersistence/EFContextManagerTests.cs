// *************************************************
// MMG.Core.Testing.Integration.EFContextManagerTests.cs
// Last Modified: 08/31/2013 11:26 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using MMG.Core.Persistence;
using MMG.Core.Persistence.Impl;
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
            doAction(() => initializeStorage());
            /*Assert.IsNull(EFContextManager.Instance.CurrentFor("TechnicalAssistanceDB"));
            EFContextManager.Instance.AddContextBuilder("TechnicalAssistanceDB", new EFContextConfiguration(new[] { "MMG.Core.Testing.Integration" }));
            var dbContext = EFContextManager.Instance.CurrentFor("TechnicalAssistanceDB");
            Assert.IsNotNull(dbContext);
            Assert.IsInstanceOf<EFDbContext>(dbContext);*/
        }

        private static void initializeStorage()
        {
            Assert.IsNull(EFContextManager.Instance.Storage);
            var storage = new SimpleDbContextStorage();
            EFContextManager.Instance.InitStorage(storage);
            Assert.IsNotNull(EFContextManager.Instance.Storage);
            Assert.IsInstanceOf<IDbContextStorage>(EFContextManager.Instance.Storage);
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