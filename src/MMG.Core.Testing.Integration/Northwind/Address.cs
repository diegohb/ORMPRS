// *************************************************
// MMG.Core.Testing.Integration.Address.cs
// Last Modified: 08/31/2013 4:04 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using MMG.Core.Persistence;

namespace MMG.Core.Testing.Integration.Northwind
{
    public class Address : IDbEntity
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }
    }
}