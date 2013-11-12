using System.Data.Entity.ModelConfiguration;
using MMG.Core.Persistence;
using MMG.Core.Testing.Integration.Northwind;

namespace MMG.Core.Testing.Integration.EFPersistence.DBMapping
{
    public class AddressEFMapping : ComplexTypeConfiguration<Address>, IMapEntityToDb<Address>
    {
        public AddressEFMapping()
        {
            Property(p => p.Street).HasColumnName("Address").HasMaxLength(60);
            Property(p => p.City).HasColumnName("City").HasMaxLength(15);
            Property(p => p.Region).HasColumnName("Region").HasMaxLength(15);
            Property(p => p.PostalCode).HasColumnName("PostalCode").HasMaxLength(60);
            //Country mapped via ADT complex type config.
        }
    }
}