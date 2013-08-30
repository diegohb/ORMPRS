// *************************************************
// MMG.Infra.EFPersistence.EFContextManager.cs
// Last Modified: 08/29/2013 2:33 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Collections.Generic;
using MMG.Core.Persistence;
using MMG.Core.Persistence.Exceptions;

namespace MMG.Infra.EFPersistence
{
    public class EFContextManager : IDbContextManager
    {
        private static readonly Lazy<EFContextManager> _instance = new Lazy<EFContextManager>(() => new EFContextManager());

        /// <summary>
        /// Lock for accessing dictionary of context builders.
        /// </summary>
        private readonly object _syncLock = new object();

        /// <summary>
        /// Maintains a dictionary of db context builders, one per database.  The key is a 
        /// connection string name used to look up the associated database, and used to decorate respective
        /// repositories. If only one database is being used, this dictionary contains a single
        /// factory with a key of <see cref="DefaultConnectionStringName" />.
        /// </summary>
        private readonly Dictionary<string, IDbContextBuilder<IDbContext>> _dbContextBuilders =
            new Dictionary<string, IDbContextBuilder<IDbContext>>();
        
        /// <summary>
        /// An application-specific implementation of IDbContextStorage must be setup either thru
        /// <see cref="InitStorage" /> or one of the <see cref="AddContextBuilder(MMG.Core.Persistence.IDbContextConfiguration)" /> overloads. 
        /// </summary>
        private IDbContextStorage _storage;

        private EFContextManager() {}
        
        /// <summary>
        /// The default connection string name used if only one database is being communicated with.
        /// </summary>
        public string DefaultConnectionStringName
        {
            get { return "EFDefaultDb"; }
        }

        public static EFContextManager Instance
        {
            get { return _instance.Value; }
        }

        public void AddContextBuilder(IDbContextConfiguration pDbContextConfig)
        {
            AddContextBuilder(DefaultConnectionStringName, pDbContextConfig);
        }

        public void AddContextBuilder(string pConnectionStringName, IDbContextConfiguration pDbContextConfig)
        {
            addConfiguration(pConnectionStringName, pDbContextConfig);
        }

        public void InitStorage(IDbContextStorage pStorage)
        {
            if (pStorage == null)
            {
                throw new ArgumentNullException("pStorage");
            }
            if ((Storage != null) && (Storage != pStorage))
            {
                throw new PersistenceException("A storage mechanism has already been configured for this application");
            }
            _storage = pStorage;
        }


        /// <summary>
        /// Used to get the current db context session if you're communicating with a single database.
        /// When communicating with multiple databases, invoke <see cref="CurrentFor(string)" /> instead.
        /// </summary>
        public IDbContext Current
        {
            get { return CurrentFor(DefaultConnectionStringName); }
        }

        /// <summary>
        /// An application-specific implementation of IDbContextStorage must be setup either thru
        /// <see cref="InitStorage" /> or one of the <see cref="AddContextBuilder(MMG.Core.Persistence.IDbContextConfiguration)" /> overloads. 
        /// </summary>
        public IDbContextStorage Storage
        {
            get { return _storage; }
        }

        /// <summary>
        /// Used to get the current DbContext associated with a key; i.e., the key 
        /// associated with an object context for a specific database.
        /// 
        /// If you're only communicating with one database, you should call <see cref="Current" /> instead,
        /// although you're certainly welcome to call this if you have the key available.
        /// </summary>
        public IDbContext CurrentFor(string pKey)
        {
            if (string.IsNullOrEmpty(pKey))
            {
                throw new ArgumentNullException("pKey");
            }

            if (Storage == null)
            {
                throw new PersistenceException("An IDbContextStorage has not been initialized");
            }

            IDbContext context = null;
            lock (_syncLock)
            {
                if (!_dbContextBuilders.ContainsKey(pKey))
                {
                    throw new PersistenceException("An DbContextBuilder does not exist with a key of " + pKey);
                }

                context = Storage.GetDbContextForKey(pKey);

                if (context == null)
                {
                    context = _dbContextBuilders[pKey].BuildDbContext();
                    Storage.SetDbContextForKey(pKey, context);
                }
            }
            return context;
        }

        /// <summary>
        /// This method is used by application-specific db context storage implementations
        /// and unit tests. Its job is to walk thru existing cached object context(s) and Close() each one.
        /// </summary>
        public void CloseAllDbContexts()
        {
            foreach (IDbContext ctx in Storage.GetAllDbContexts())
            {
                ctx.CloseConnection();
            }
        }

        private void addConfiguration(string pConnectionStringName, IDbContextConfiguration pDbContextConfig)
        {
            if (string.IsNullOrEmpty(pConnectionStringName))
                throw new ArgumentNullException("pConnectionStringName");

            if (pDbContextConfig == null)
                throw new ArgumentNullException("pDbContextConfig");

            lock (_syncLock)
            {
                if (_dbContextBuilders.ContainsKey(pConnectionStringName))
                    throw new Exception
                        (string.Format
                             ("A builder configuration has already been specified for key '{0}'. You can only call this method once per connection string name.",
                              pConnectionStringName));

                _dbContextBuilders.Add
                    (pConnectionStringName,
                     new EFContextBuilder<EFDbContext>(pConnectionStringName, (EFContextConfiguration) pDbContextConfig));
            }
        }
    }
}