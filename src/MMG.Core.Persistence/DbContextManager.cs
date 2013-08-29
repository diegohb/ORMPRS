// *************************************************
// MMG.Core.Persistence.DbContextManager.cs
// Last Modified: 08/29/2013 12:13 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Collections.Generic;

namespace MMG.Core.Persistence
{
    public class DbContextManager
    {
        /// <summary>
        /// Maintains a dictionary of db context builders, one per database.  The key is a 
        /// connection string name used to look up the associated database, and used to decorate respective
        /// repositories. If only one database is being used, this dictionary contains a single
        /// factory with a key of <see cref="DefaultConnectionStringName" />.
        /// </summary>
        protected static readonly Dictionary<string, IDbContextBuilder<IDbContext>> _dbContextBuilders =
            new Dictionary<string, IDbContextBuilder<IDbContext>>();

        protected static readonly object _syncLock = new object();

        /// <summary>
        /// An application-specific implementation of IDbContextStorage must be setup either thru
        /// <see cref="InitStorage" /> or one of the <see cref="Init(string[],bool)" /> overloads. 
        /// </summary>
        protected static IDbContextStorage Storage
        {
            get { return _storage; }
        }

        public static void Init(string[] pMappingAssemblies, bool pUpdateDatabaseSchema = false)
        {
            Init(DefaultConnectionStringName, pMappingAssemblies, pUpdateDatabaseSchema);
        }

        public static void Init(string pConnectionStringName, string[] pMappingAssemblies, bool pUpdateDatabaseSchema = false)
        {
            addConfiguration(pConnectionStringName, pMappingAssemblies, pUpdateDatabaseSchema);
        }

        public static void InitStorage(IDbContextStorage pStorage)
        {
            if (pStorage == null)
            {
                throw new ArgumentNullException("pStorage");
            }
            if ((Storage != null) && (Storage != pStorage))
            {
                throw new ApplicationException("A storage mechanism has already been configured for this application");
            }
            _storage = pStorage;
        }

        /// <summary>
        /// The default connection string name used if only one database is being communicated with.
        /// </summary>
        public static readonly string DefaultConnectionStringName = "DefaultDb";

        private static IDbContextStorage _storage;

        /// <summary>
        /// Used to get the current db context session if you're communicating with a single database.
        /// When communicating with multiple databases, invoke <see cref="CurrentFor(string)" /> instead.
        /// </summary>
        public static IDbContext Current
        {
            get { return CurrentFor(DefaultConnectionStringName); }
        }

        /// <summary>
        /// Used to get the current DbContext associated with a key; i.e., the key 
        /// associated with an object context for a specific database.
        /// 
        /// If you're only communicating with one database, you should call <see cref="Current" /> instead,
        /// although you're certainly welcome to call this if you have the key available.
        /// </summary>
        public static IDbContext CurrentFor(string pKey)
        {
            if (string.IsNullOrEmpty(pKey))
            {
                throw new ArgumentNullException("pKey");
            }

            if (Storage == null)
            {
                throw new ApplicationException("An IDbContextStorage has not been initialized");
            }

            IDbContext context = null;
            lock (_syncLock)
            {
                if (!_dbContextBuilders.ContainsKey(pKey))
                {
                    throw new ApplicationException("An DbContextBuilder does not exist with a key of " + pKey);
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
        public static void CloseAllDbContexts()
        {
            foreach (IDbContext ctx in Storage.GetAllDbContexts())
            {
                ctx.CloseConnection();
            }
        }

        private static void addConfiguration(string pConnectionStringName, IEnumerable<string> pMappingAssemblies, bool pUpdateDatabaseSchema = false)
        {
            if (string.IsNullOrEmpty(pConnectionStringName))
                throw new ArgumentNullException("pConnectionStringName");

            if (pMappingAssemblies == null)
                throw new ArgumentNullException("pMappingAssemblies");

            lock (_syncLock)
            {
                /*_dbContextBuilders.Add(pConnectionStringName,
                    new DbContextBuilder<IDbContext>(pConnectionStringName, pMappingAssemblies, pUpdateDatabaseSchema));*/
            }
        }
    }
}