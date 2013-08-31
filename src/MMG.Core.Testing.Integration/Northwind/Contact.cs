// *************************************************
// MMG.Core.Testing.Integration.Contact.cs
// Last Modified: 08/31/2013 4:04 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Data.Entity.ModelConfiguration;
using MMG.Core.Persistence;

namespace MMG.Core.Testing.Integration.Northwind
{
    public class Contact : IDbEntity
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public virtual Address Address { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }
    }

    public class ContactMapping : ComplexTypeConfiguration<Contact>, IMapEntityToDb<Contact>
    {
        public ContactMapping()
        {
            Property(p => p.Name).HasColumnName("ContactName").HasMaxLength(30);
            Property(p => p.Title).HasColumnName("ContactTitle").HasMaxLength(30);
            Property(p => p.Phone).HasColumnName("Phone").HasMaxLength(24);
            Property(p => p.Fax).HasColumnName("Fax").HasMaxLength(24);
        }
    }
}