// *************************************************
// MMG.Infra.OAPersistence.OAUnitOfWork.cs
// Last Modified: 08/30/2013 1:50 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Data;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Data.Common;
using IUnitOfWork = MMG.Core.Persistence.IUnitOfWork;
using IOAUnitOfWork = Telerik.OpenAccess.IUnitOfWork;

namespace MMG.Core.OAPersistence
{
    internal class OAUnitOfWork : IUnitOfWork
    {
        private OATransaction _transaction;
        private readonly OpenAccessContext _dbContext;
        private bool _disposed;

        public OAUnitOfWork(OpenAccessContext pContext)
        {
            _dbContext = pContext;
        }

        public bool IsInTransaction
        {
            get { return _transaction != null; }
        }

        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel pIsolationLevel)
        {
            if (_transaction != null)
            {
                throw new ApplicationException
                    ("Cannot begin a new transaction while an existing transaction is still running. " +
                     "Please commit or rollback the existing transaction before starting a new one.");
            }
            openConnection();
            _transaction = _dbContext.Connection.BeginTransaction(pIsolationLevel);
        }

        public void RollBackTransaction()
        {
            if (_transaction == null)
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");

            if (!IsInTransaction) return;

            _dbContext.ClearChanges();
            releaseCurrentTransaction();
        }

        public void CommitTransaction()
        {
            CommitTransaction(false);
        }

        public void CommitTransaction(bool pIgnoreErrors)
        {
            if (!IsInTransaction)
                throw new ApplicationException("Can not commit changes while there is no transaction running.");

            try
            {
                SaveChanges(pIgnoreErrors);
            }
            catch
            {
                RollBackTransaction();
                throw;
            }
        }

        public void SaveChanges()
        {
            SaveChanges(false);
        }

        public void SaveChanges(bool pIgnoreErrors)
        {
            if (IsInTransaction)
            {
                throw new ApplicationException("A transaction is running. Call CommitTransaction instead.");
            }

            try
            {
                _dbContext.SaveChanges
                    (pIgnoreErrors ? ConcurrencyConflictsProcessingMode.AggregateAll : ConcurrencyConflictsProcessingMode.StopOnFirst);
            }
            catch
            {
                throw;
            }
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes off the managed and unmanaged resources used.
        /// </summary>
        /// <param name="pDisposing"></param>
        private void dispose(bool pDisposing)
        {
            if (!pDisposing)
                return;

            if (_disposed)
                return;

            _disposed = true;
        }

        #endregion

        private void openConnection()
        {
            if(_dbContext.Connection.State != ConnectionState.Open)
                _dbContext.Connection.Open();
        }

        /// <summary>
        /// Releases the current transaction
        /// </summary>
        private void releaseCurrentTransaction()
        {
            if (_transaction == null) return;

            _transaction.Dispose();
            _transaction = null;
        }
    }
}