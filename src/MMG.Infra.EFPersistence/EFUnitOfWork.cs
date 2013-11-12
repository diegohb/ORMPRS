// *************************************************
// MMG.Infra.EFPersistence.EFUnitOfWork.cs
// Last Modified: 11/05/2013 6:45 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using MMG.Core.Persistence;
using MMG.Core.Persistence.Exceptions;

namespace MMG.Infra.EFPersistence
{
    internal class EFUnitOfWork : IUnitOfWork
    {
        private DbTransaction _transaction;
        private readonly DbContext _dbContext;
        private bool _disposed;

        public EFUnitOfWork(DbContext pContext)
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
                throw new PersistenceException
                    ("Cannot begin a new transaction while an existing transaction is still running. " +
                     "Please commit or rollback the existing transaction before starting a new one.");
            }
            openConnection();
            _transaction = ((IObjectContextAdapter) _dbContext).ObjectContext.Connection.BeginTransaction(pIsolationLevel);
        }

        public void RollBackTransaction()
        {
            if (_transaction == null)
                throw new PersistenceException("Cannot roll back a transaction while there is no transaction running.");

            if (!IsInTransaction) return;

            _transaction.Rollback();
            releaseCurrentTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
                throw new PersistenceException("There is no transaction running to commit.");

            try
            {
                ((IObjectContextAdapter) _dbContext).ObjectContext.SaveChanges();
                _transaction.Commit();
                releaseCurrentTransaction();
            }
            catch (UpdateException updateException)
            {
                RollBackTransaction();
                throw wrapDataException(updateException);
            }
            catch
            {
                RollBackTransaction();
                throw;
            }
        }

        public void SaveChanges()
        {
            if (IsInTransaction)
            {
                throw new PersistenceException("A transaction is running. Call CommitTransaction instead.");
            }

            try
            {
                ((IObjectContextAdapter) _dbContext).ObjectContext.SaveChanges();
            }
            catch (UpdateException updateException)
            {
                throw wrapDataException(updateException);
            }
        }

        public void SaveChanges(SaveOptions pSaveOptions)
        {
            if (IsInTransaction)
            {
                throw new PersistenceException("A transaction is running. Call CommitTransaction instead.");
            }

            try
            {
                ((IObjectContextAdapter) _dbContext).ObjectContext.SaveChanges(pSaveOptions);
            }
            catch (UpdateException updateException)
            {
                throw wrapDataException(updateException);
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
            if (((IObjectContextAdapter) _dbContext).ObjectContext.Connection.State != ConnectionState.Open)
            {
                ((IObjectContextAdapter) _dbContext).ObjectContext.Connection.Open();
            }
        }

        /// <summary>
        /// Releases the current transaction
        /// </summary>
        private void releaseCurrentTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        private static PersistenceException wrapDataException(UpdateException pUpdateException)
        {
            var msgDetails = pUpdateException.InnerException != null
                ? pUpdateException.InnerException.Message
                : pUpdateException.Message;
            return new PersistenceException
                (string.Format("An error has occurred while trying to commit your changes: {0}.", msgDetails),
                    pUpdateException.InnerException ?? pUpdateException);
        }
    }
}