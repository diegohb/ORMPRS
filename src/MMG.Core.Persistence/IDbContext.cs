// *************************************************
// MMG.Core.Persistence.IDbContext.cs
// Last Modified: 08/29/2013 12:13 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

namespace MMG.Core.Persistence
{
    /// <summary>
    /// Represents an ORM database context class
    /// </summary>
    public interface IDbContext
    {
        void Add<TEntity>(TEntity pEntity) where TEntity : class, IDbEntity;

        int SaveChanges();

        void CloseConnection();
    }
}