// *************************************************
// MMG.Infra.EFPersistence.EFContextConfiguration.cs
// Last Modified: 08/30/2013 11:41 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Collections.Generic;
using System.Linq;
using MMG.Core.Persistence;

namespace MMG.Infra.EFPersistence
{
    public class EFContextConfiguration : IDbContextConfiguration
    {
        public EFContextConfiguration()
        {
            MappingAssemblies = new List<string>();
            LazyLoadingEnabled = false;
            ProxyCreationEnabled = false;
            UseLegacyPreserveChangesBehavior = false;
        }

        public EFContextConfiguration(IEnumerable<string> pMappingAssemblies)
        {
            MappingAssemblies = pMappingAssemblies.ToList();
        }

        public IList<string> MappingAssemblies { get; set; }

        /// <summary>
        /// if set to <c>true</c> [recreate database if exist].
        /// </summary>
        public bool RecreateDatabaseIfExists { get; set; }

        /// <summary>
        /// if set to <c>true</c> [lazy loading enabled].
        /// </summary>
        public bool LazyLoadingEnabled { get; set; }

        /// <summary>
        /// if set to <c>true</c> then proxy objects will be created by EF.
        /// </summary>
        public bool ProxyCreationEnabled { get; set; }

        /// <summary>
        /// if set to <c>true</c> then legacy behavior will be used for changes.
        /// </summary>
        public bool UseLegacyPreserveChangesBehavior { get; set; }
    }
}