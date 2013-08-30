// *************************************************
// MMG.Infra.OAPersistence.DynamicMetadataSource.cs
// Last Modified: 08/30/2013 9:22 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Collections.Generic;
using System.Linq;
using Telerik.OpenAccess.Metadata.Fluent;

namespace MMG.Core.OAPersistence
{
    public class DynamicMetadataSource : FluentMetadataSource
    {
        private readonly IList<MappingConfiguration> _configurations;

        public DynamicMetadataSource()
        {
            _configurations = new List<MappingConfiguration>();
        }

        public DynamicMetadataSource(IEnumerable<MappingConfiguration> pMappingConfigurations)
        {
            _configurations = pMappingConfigurations.ToList();
        }

        protected override IList<MappingConfiguration> PrepareMapping()
        {
            return _configurations;
        }
    }

}