// *************************************************
// MMG.Core.Testing.Integration.CustomerOAMapping.cs
// Last Modified: 08/31/2013 5:28 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using MMG.Core.Persistence;
using MMG.Core.Testing.Integration.Northwind;
using Telerik.OpenAccess.Metadata.Fluent;

namespace MMG.Core.Testing.Integration.OAPersistence.DBMapping
{
    public class CustomerOAMapping : MappingConfiguration<Customer>, IMapEntityToDb<Customer>
    {
        public CustomerOAMapping()
        {
            MapType(p => new { CustomerID = p.Id, CompanyName = p.Name }).ToTable("Customers");

            HasProperty(p => p.Id).IsIdentity().HasLength(5);
            HasProperty(p => p.Name).HasLength(40).IsNullable();

            /*//contact complex
            HasProperty(p => p.Contact.Name).HasFieldName("ContactName").HasLength(30);
            HasProperty(p => p.Contact.Title).HasFieldName("ContactTitle").HasLength(30);
            HasProperty(p => p.Contact.Phone).HasFieldName("Phone").HasLength(24);
            HasProperty(p => p.Contact.Fax).HasFieldName("Fax").HasLength(24);

            //address complex
            HasProperty(p => p.Contact.Address.Street).HasFieldName("Address").HasLength(60);
            HasProperty(p => p.Contact.Address.City).HasFieldName("City").HasLength(15);
            HasProperty(p => p.Contact.Address.Region).HasFieldName("Region").HasLength(15);
            HasProperty(p => p.Contact.Address.PostalCode).HasFieldName("PostalCode").HasLength(60);
            HasProperty(p => p.Contact.Address.Country).HasFieldName("Country").HasLength(15);*/
        }

        public string ConnectionStringName { get { return string.Empty; } }
    }
}