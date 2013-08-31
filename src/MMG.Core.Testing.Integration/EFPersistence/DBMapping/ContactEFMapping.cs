using System.Data.Entity.ModelConfiguration;
using MMG.Core.Persistence;
using MMG.Core.Testing.Integration.Northwind;

namespace MMG.Core.Testing.Integration.EFPersistence.DBMapping
{
    public class ContactEFMapping : ComplexTypeConfiguration<Contact>, IMapEntityToDb<Contact>
    {
        public ContactEFMapping()
        {
            Property(p => p.Name).HasColumnName("ContactName").HasMaxLength(30);
            Property(p => p.Title).HasColumnName("ContactTitle").HasMaxLength(30);
            Property(p => p.Phone).HasColumnName("Phone").HasMaxLength(24);
            Property(p => p.Fax).HasColumnName("Fax").HasMaxLength(24);
        }
    }
}