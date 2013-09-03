// *************************************************
// MMG.Infra.EFPersistence.EFDbContext.cs
// Last Modified: 08/30/2013 1:24 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using MMG.Core.Persistence;

namespace MMG.Infra.EFPersistence
{
    /// <summary>
    /// Abstract class that simply wraps the EntityFramework DbContext object and tags the object with <see cref="IDbContext"/>.
    /// </summary>
    public class EFDbContext : DbContext, IDbContext
    {
        #region Constructors

        protected EFDbContext(string pNameOrConnectionString)
            : base(pNameOrConnectionString) {}

        public EFDbContext(ObjectContext pObjectContext, bool pDBContextOwnsObjectContext)
            : base(pObjectContext, pDBContextOwnsObjectContext) {}

        #endregion


        public virtual void Add<TEntity>(TEntity pEntity) where TEntity : class, IDbEntity
        {
            Set<TEntity>().Add(pEntity);
        }

        public virtual void CloseConnection()
        {
            if (Database.Connection.State != ConnectionState.Closed)
                Database.Connection.Close();
        }
    }
}