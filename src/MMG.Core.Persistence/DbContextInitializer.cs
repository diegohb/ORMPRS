// *************************************************
// MMG.Core.Persistence.DbContextInitializer.cs
// Last Modified: 10/15/2013 10:33 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Collections.Generic;
using MMG.Core.Persistence.Exceptions;

namespace MMG.Core.Persistence
{
    public class DbContextInitializer
    {
        private static readonly object _syncLock = new object();
        private static DbContextInitializer _instance;

        protected DbContextInitializer() {}

        /// <summary>
        /// pair of: unique hash code (int) AND initialize = true/false
        /// </summary>
        private readonly ICollection<int> _initializers = new HashSet<int>();

        public static DbContextInitializer Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new DbContextInitializer();
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// This is the method which should be given the call to intialize the DbContext; e.g.,
        /// DbContextInitializer.Instance().InitializeDbContextOnce(() => InitializeDbContext());
        /// where InitializeDbContext() is a method which calls DbContextManager.InitStorage()
        /// </summary>
        /// <param name="pInitMethod">Method to run once and only once.</param>
        /// <param name="pAppInstanceHashCode">Optional. The hash code of the entry app when it may be called more than once.</param>
        /// <remarks>If a hash code is provided, then error thrown from initStorage on subsequent calls will be caught and muted. 
        /// This is intended to help with the NullRef error with http modules and event registrations.</remarks>
        public void InitializeDbContextOnce(Action pInitMethod, int? pAppInstanceHashCode = null)
        {
            var rethrowSubsequentErrors = !pAppInstanceHashCode.HasValue;
            if (!pAppInstanceHashCode.HasValue)
                pAppInstanceHashCode = GetHashCode();

            lock (_syncLock)
            {
                if (_initializers.Contains(pAppInstanceHashCode.Value)) return;
                try
                {
                    pInitMethod();
                    _initializers.Add(pAppInstanceHashCode.Value);
                }
                catch (PersistenceException exPersistence)
                {
                    if (rethrowSubsequentErrors && _initializers.Count == 0)
                        throw;
                }
            }
        }
    }
}