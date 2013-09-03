// *************************************************
// MMG.Core.Testing.Integration.AddressOAMapping.cs
// Last Modified: 08/31/2013 5:28 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using MMG.Core.Persistence;
using MMG.Core.Testing.Integration.Northwind;

namespace MMG.Core.Testing.Integration.OAPersistence.DBMapping
{
    public class AddressOAMapping : IMapEntityToDb<Address>
    {
        public AddressOAMapping()
        {
           /* Property(p => p.Street).HasColumnName("Address").HasMaxLength(60);
            Property(p => p.City).HasColumnName("City").HasMaxLength(15);
            Property(p => p.Region).HasColumnName("Region").HasMaxLength(15);
            Property(p => p.PostalCode).HasColumnName("PostalCode").HasMaxLength(60);
            Property(p => p.Country).HasColumnName("Country").HasMaxLength(15);*/
        }
    }
}