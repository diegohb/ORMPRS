// *************************************************
// MMG.Core.Persistence.IUnitOfWork.cs
// Last Modified: 08/29/2013 12:13 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Data;

namespace MMG.Core.Persistence
{
    public interface IUnitOfWork
    {
        bool IsInTransaction { get; }

        void SaveChanges();

        void BeginTransaction();

        void BeginTransaction(IsolationLevel pIsolationLevel);

        void RollBackTransaction();

        void CommitTransaction();
    }
}