// *************************************************
// MMG.Core.Testing.Integration.NorthwindDB.cs
// Last Modified: 11/11/2013 2:18 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Data.Entity;
using MMG.Core.Testing.Integration.EFPersistence.DBMapping;

namespace MMG.Core.Testing.Integration.Northwind
{
    public partial class NorthwindDB
    {
        protected override void OnModelCreating(DbModelBuilder pModelBuilder)
        {
            pModelBuilder.Configurations.Add(new CustomerEFMapping());
            pModelBuilder.Configurations.Add(new ShipperEFMapping());
            pModelBuilder.Configurations.Add(new AddressEFMapping());
            pModelBuilder.Configurations.Add(new ContactEFMapping());
            pModelBuilder.Configurations.Add(new ShipperPriorityEFMapping());
            pModelBuilder.Configurations.Add(new CountryEFMapping());
        }
    }
}