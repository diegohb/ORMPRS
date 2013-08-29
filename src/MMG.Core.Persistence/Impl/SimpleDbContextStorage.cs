// *************************************************
// MMG.Core.Persistence.SimpleDbContextStorage.cs
// Last Modified: 08/29/2013 4:40 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Collections.Generic;

namespace MMG.Core.Persistence.Impl
{
    public class SimpleDbContextStorage : IDbContextStorage
    {
        private readonly Dictionary<string, IDbContext> _storage = new Dictionary<string, IDbContext>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDbContextStorage"/> class.
        /// </summary>
        public SimpleDbContextStorage() { }

        /// <summary>
        /// Returns the db context associated with the specified key or
        /// null if the specified key is not found.
        /// </summary>
        /// <param name="pKey">The key.</param>
        /// <returns></returns>
        public IDbContext GetDbContextForKey(string pKey)
        {
            IDbContext context;
            if (!_storage.TryGetValue(pKey, out context))
                return null;
            return context;
        }


        /// <summary>
        /// Stores the db context into a dictionary using the specified key.
        /// If an object context already exists by the specified key, 
        /// it gets overwritten by the new object context passed in.
        /// </summary>
        /// <param name="pKey">The key.</param>
        /// <param name="pContext">The db context.</param>
        public void SetDbContextForKey(string pKey, IDbContext pContext)
        {
            _storage.Add(pKey, pContext);
        }

        /// <summary>
        /// Returns all the values of the internal dictionary of db contexts.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDbContext> GetAllDbContexts()
        {
            return _storage.Values;
        }
    }

}