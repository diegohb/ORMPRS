// *************************************************
// MMG.Infra.OAPersistence.OAContextConfiguration.cs
// Last Modified: 08/30/2013 11:43 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Collections.Generic;
using System.Linq;
using MMG.Core.Persistence;

namespace MMG.Core.OAPersistence
{
    public class OAContextConfiguration : IDbContextConfiguration
    {
        public OAContextConfiguration()
        {
            MappingAssemblies = new List<string>();
        }

        public OAContextConfiguration(IEnumerable<string> pMappingAssemblies)
        {
            MappingAssemblies = pMappingAssemblies.ToList();
        }

        public IList<string> MappingAssemblies { get; set; }

        public bool UpdateDatabaseSchema { get; set; }

        public string BackendConfig { get; set; }
    }
}