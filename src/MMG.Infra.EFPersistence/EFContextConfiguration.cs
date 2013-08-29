// *************************************************
// MMG.Infra.EFPersistence.EFContextConfiguration.cs
// Last Modified: 08/29/2013 2:30 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Collections.Generic;
using MMG.Core.Persistence;

namespace MMG.Infra.EFPersistence
{
    public class EFContextConfiguration : IDbContextConfiguration
    {
        public EFContextConfiguration(IEnumerable<string> pMappingAssemblies)
        {
            MappingAssemblies = pMappingAssemblies;
        }

        public string ConnectionStringName { get; set; }

        public IEnumerable<string> MappingAssemblies { get; set; }

        public bool RecreateDatabaseIfExists { get; set; }

        public bool LazyLoadingEnabled { get; set; }
    }
}