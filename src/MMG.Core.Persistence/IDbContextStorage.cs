// *************************************************
// MMG.Core.Persistence.IDbContextStorage.cs
// Last Modified: 08/29/2013 12:13 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Collections.Generic;

namespace MMG.Core.Persistence
{
    /// <summary>
    /// Stores object context
    /// </summary>
    public interface IDbContextStorage
    {
        /// <summary>
        /// Gets the db context for key.
        /// </summary>
        /// <param name="pKey">The key.</param>
        /// <returns></returns>
        IDbContext GetDbContextForKey(string pKey);

        /// <summary>
        /// Sets the db context for key.
        /// </summary>
        /// <param name="pKey">The key.</param>
        /// <param name="pContext">The object context.</param>
        void SetDbContextForKey(string pKey, IDbContext pContext);

        /// <summary>
        /// Gets all db contexts.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IDbContext> GetAllDbContexts();
    }
}