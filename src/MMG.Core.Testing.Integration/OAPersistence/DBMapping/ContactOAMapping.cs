// *************************************************
// MMG.Core.Testing.Integration.ContactOAMapping.cs
// Last Modified: 08/31/2013 5:28 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using MMG.Core.Persistence;
using MMG.Core.Testing.Integration.Northwind;

namespace MMG.Core.Testing.Integration.OAPersistence.DBMapping
{
    public class ContactOAMapping : IMapEntityToDb<Contact>
    {
        public ContactOAMapping()
        {
            /*Property(p => p.Name).HasColumnName("ContactName").HasMaxLength(30);
            Property(p => p.Title).HasColumnName("ContactTitle").HasMaxLength(30);
            Property(p => p.Phone).HasColumnName("Phone").HasMaxLength(24);
            Property(p => p.Fax).HasColumnName("Fax").HasMaxLength(24);*/
        }
    }
}