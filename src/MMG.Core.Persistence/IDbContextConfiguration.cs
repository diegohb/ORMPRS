// *************************************************
// MMG.Core.Persistence.IDbContextConfiguration.cs
// Last Modified: 08/29/2013 2:18 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Collections.Generic;

namespace MMG.Core.Persistence
{
    /// <summary>
    /// This represents an object that stores ORM-specific settings for building a database context object.
    /// </summary>
    public interface IDbContextConfiguration
    {
        IEnumerable<string> MappingAssemblies { get; set; }
    }
}