// *************************************************
// MMG.Core.Persistence.WebDbContextStorageBase.cs
// Last Modified: 10/15/2013 10:04 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace MMG.Core.Persistence.Impl
{
    /// <summary>
    /// This base class provides a storage mechanism that must be implemented at the web layer in order to properly use the cache and trigger the OnEndRequest event handler.
    /// </summary>
    public abstract class WebDbContextStorageBase : IDbContextStorage
    {
        private IDbContextManager _contextManager;
        private readonly Func<IDbContextManager> _contextLoaderMethod;

        protected WebDbContextStorageBase(Func<IDbContextManager> pContextLoaderMethod)
        {
            _contextLoaderMethod = pContextLoaderMethod;
        }

        /// <summary>
        /// This is typically HttpContext.Current.Items dictionary of cache objects.
        /// </summary>
        public abstract IDictionary HttpCacheStore { get; }

        public void OnEndRequest(object pSender, EventArgs pArgs)
        {
            getContextManager().CloseAllDbContexts();
            HttpCacheStore.Remove(storageKey);
        }

        public IDbContext GetDbContextForKey(string pKey)
        {
            var storage = getSimpleDbContextStorage();
            return storage.GetDbContextForKey(pKey);
        }

        public void SetDbContextForKey(string pKey, IDbContext pContext)
        {
            var storage = getSimpleDbContextStorage();
            storage.SetDbContextForKey(pKey, pContext);
        }

        public IEnumerable<IDbContext> GetAllDbContexts()
        {
            var storage = getSimpleDbContextStorage();
            return storage.GetAllDbContexts();
        }

        private SimpleDbContextStorage getSimpleDbContextStorage()
        {
            var storage = HttpCacheStore[storageKey] as SimpleDbContextStorage;
            if (storage == null)
            {
                storage = new SimpleDbContextStorage();
                HttpCacheStore[storageKey] = storage;
            }
            return storage;
        }

        private IDbContextManager getContextManager()
        {
            if (_contextManager == null)
            {
                _contextManager = _contextLoaderMethod.Invoke();
            }
            return _contextManager;
        }

        private const string storageKey = "HttpContextDbContextStorageKey";
    }
}