// *************************************************
// MMG.Core.Testing.Integration.EFContextBuilderTests.cs
// Last Modified: 08/31/2013 2:53 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Data.Entity;
using System.Linq;
using MMG.Core.Testing.Integration.Northwind;
using MMG.Infra.EFPersistence;
using NUnit.Framework;

namespace MMG.Core.Testing.Integration.EFPersistence
{
    [TestFixture]
    public class EFContextBuilderTests
    {
        [Test]
        public void BuildContext_ShouldCreateContextThatConnectsToDatabase()
        {
            var builder = new EFContextBuilder<EFDbContext>
                (Utility.NorthwindDBConnectionName, new EFContextConfiguration(new[] { "MMG.Core.Testing.Integration" }));
            var db = builder.BuildDbContext();
            Assert.IsNotNull(db);
            Assert.IsInstanceOf<EFDbContext>(db);
            Assert.IsInstanceOf<DbContext>(db);
            Assert.IsTrue(db.Set<Customer>().Any());
            Assert.IsNotNull(db.Set<Customer>().First());
        }
    }
}