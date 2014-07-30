// *************************************************
// MMG.Core.Testing.Integration.SupplierEFMapping.cs
// Last Modified: 11/12/2013 8:29 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Data.Entity.ModelConfiguration;
using MMG.Core.Persistence;
using MMG.Core.Testing.Integration.Northwind;

namespace MMG.Core.Testing.Integration.EFPersistence.DBMapping
{
    public class SupplierEFMapping : EntityTypeConfiguration<Supplier>, IMapEntityToDb<Supplier>
    {
        public SupplierEFMapping()
        {
            ToTable("Suppliers");
            HasKey(p => p.Id);
        }

        public string ConnectionStringName { get { return string.Empty; } }
    }
}