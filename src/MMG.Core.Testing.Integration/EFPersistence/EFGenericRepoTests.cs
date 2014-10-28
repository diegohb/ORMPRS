using System.Collections;
using System.Linq;
using MMG.Core.Persistence;
using MMG.Core.Persistence.Exceptions;
using MMG.Core.Persistence.Impl;
using MMG.Core.Testing.Integration.Northwind;
using MMG.Infra.EFPersistence;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace MMG.Core.Testing.Integration.EFPersistence
{
    [TestFixture]
    public class EFGenericRepoTests
    {
        private NorthwindDB _nwDB;
        private EFGenericRepository _repo;

        [SetUp]
        public void Init()
        {
            initializeStorage();
            configureNorthwindContext();
            Assert.IsNotNull(_nwDB);
        }

        [TearDown]
        public void Destroy()
        {
            _nwDB.ShippersSet.Where(p => p.Name.Contains("nUnit")).ToList().ForEach(pShipper => _nwDB.ShippersSet.Remove(pShipper));
            _nwDB.SaveChanges();
        }

        [Test]
        public void FetchShippers_ShouldMatch()
        {
            //ARRANGE
            var expected = _nwDB.ShippersSet.ToList();

            //ACT
            var actual = _repo.GetAll<Shipper>().ToList();

            //ASSERT
            CollectionAssert.AreEquivalent(expected, actual);
        }


        [Test]
        public void FetchSuppliersWithQuery_ShouldMatch()
        {
            //ARRANGE
            var expected = _nwDB.SuppliersSet.Where(pSupplier => pSupplier.Id > 2).ToList();

            //ACT
            var actual = _repo.GetQuery<Supplier>(pSupplier => pSupplier.Id > 2).ToList();

            //ASSERT
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void InsertAndGetOriginal_ShouldReturnOriginalObject()
        {
            //ARRANGE
            var newShipper = new Shipper() {Name = "Test Shipper", PhoneNumber = "123-565-8989", PriorityLevel = ShipperPriorityEnum.High};
            _nwDB.Add(newShipper);
            _nwDB.SaveChanges();
            Assert.IsTrue(newShipper.Id > 0);

            //ACT
            newShipper.PriorityLevel = ShipperPriorityEnum.Low;
            var originalShipper = _repo.GetOriginal<Shipper>(newShipper);
            
            //ASSERT
            Assert.IsNotNull(originalShipper);
            Assert.AreEqual(ShipperPriorityEnum.High, originalShipper.PriorityLevel.EnumValue);
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

        private void configureNorthwindContext()
        {
            var dbContext = EFContextManager.Instance.CurrentFor(Utility.NorthwindDBConnectionName);
            Assert.IsNotNull(dbContext);
            Assert.IsInstanceOf<EFDbContext>(dbContext);

            _nwDB = new NorthwindDB();
            _nwDB.Configuration.ProxyCreationEnabled = false;
            _repo = new EFGenericRepository((EFDbContext)dbContext);
        }

    }
}