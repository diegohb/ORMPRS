// *************************************************
// MMG.Core.Testing.Integration.CustomerEFMapping.cs
// Last Modified: 08/31/2013 5:26 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Data.Entity.ModelConfiguration;
using MMG.Core.Persistence;
using MMG.Core.Testing.Integration.Northwind;

namespace MMG.Core.Testing.Integration.EFPersistence.DBMapping
{
    public class CustomerEFMapping : EntityTypeConfiguration<Customer>, IMapEntityToDb<Customer>
    {
        public CustomerEFMapping()
        {
            ToTable("Customers");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("CustomerID").HasMaxLength(5);
            Property(p => p.Name).HasColumnName("CompanyName").HasMaxLength(40).IsRequired();
        }

        public virtual string ConnectionStringName { get { return Utility.NorthwindDBConnectionName; } }
    }
}