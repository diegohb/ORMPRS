// *************************************************
// MMG.Core.Testing.Integration.ADTMappingTests.cs
// Last Modified: 11/11/2013 1:22 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Linq;
using MMG.Core.Persistence;
using MMG.Core.Persistence.Impl;
using MMG.Core.Testing.Integration.Northwind;
using MMG.Infra.EFPersistence;
using NUnit.Framework;

namespace MMG.Core.Testing.Integration.EFPersistence
{
    [TestFixture]
    public class ADTMappingTests
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
        public void InsertShipperWithPriority()
        {
            const ShipperPriorityEnum expectedPriority = ShipperPriorityEnum.High;
            var shipper = new Shipper
            {
                Name = "nUnit Shipper",
                PriorityLevel = expectedPriority
            };

            Assert.AreEqual(expectedPriority, shipper.PriorityLevel);
            _repo.Add(shipper);
            _repo.UnitOfWork.SaveChanges();
            Assert.True(shipper.Id > 0);

            var shipperFromDB = _nwDB.ShippersSet.SingleOrDefault(p => p.Id == shipper.Id);
            Assert.IsNotNull(shipperFromDB);
            Assert.AreEqual(expectedPriority, shipperFromDB.PriorityLevel);

            shipper.PriorityLevel = ShipperPriorityEnum.Low;
            var updatedShipper = _repo.Update(shipper);
            _repo.UnitOfWork.SaveChanges();

            Assert.AreEqual(ShipperPriorityEnum.Low, updatedShipper.PriorityLevel);
        }

        private static void initializeStorage()
        {
            Assert.IsNull(EFContextManager.Instance.Storage);
            var storage = new SimpleDbContextStorage();
            EFContextManager.Instance.InitStorage(storage);
            Assert.IsNotNull(EFContextManager.Instance.Storage);
            Assert.IsInstanceOf<IDbContextStorage>(EFContextManager.Instance.Storage);
        }

        private void configureNorthwindContext()
        {
            EFContextManager.Instance.AddContextBuilder(Utility.NorthwindDBConnectionName, new EFContextConfiguration(new[] { "MMG.Core.Testing.Integration" }));
            var dbContext =  EFContextManager.Instance.CurrentFor(Utility.NorthwindDBConnectionName);
            Assert.IsNotNull(dbContext);
            Assert.IsInstanceOf<EFDbContext>(dbContext);

            _nwDB = new NorthwindDB();
            _repo = new EFGenericRepository((EFDbContext) dbContext);
        }

    }
}