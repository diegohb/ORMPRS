// *************************************************
// MMG.Core.Testing.Integration.Customer.cs
// Last Modified: 08/31/2013 2:57 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using MMG.Core.Persistence;

namespace MMG.Core.Testing.Integration.Northwind
{
    public class Customer : IDbEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        /*public virtual Contact Contact { get; set; }*/
    }
}