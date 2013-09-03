// *************************************************
// MMG.Infra.OAPersistence.OADbContext.cs
// Last Modified: 09/03/2013 12:28 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Data;
using MMG.Core.Persistence;
using Telerik.OpenAccess;

namespace MMG.Core.OAPersistence
{
    /// <summary>
    /// Abstract class that simply wraps the EntityFramework DbContext object and tags the object with <see cref="IDbContext"/>.
    /// </summary>
    public class OADbContext : OpenAccessContext, IDbContext
    {
        #region Constructors

        public OADbContext(string pConnectionString, BackendConfiguration pBackendConfiguration, DynamicMetadataSource pMetadataSource)
            : base(pConnectionString, pBackendConfiguration, pMetadataSource) {}

        protected OADbContext(OpenAccessContextBase pOtherContext) : base(pOtherContext) {}

        #endregion

        public void Add<TEntity>(TEntity pEntity) where TEntity : class, IDbEntity
        {
            base.Add(pEntity);
        }

        public new int SaveChanges()
        {
            base.SaveChanges();
            return 0;
        }

        public void CloseConnection()
        {
            if (Connection.State == ConnectionState.Open)
                Connection.StoreConnection.Close();
        }
    }
}