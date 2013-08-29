﻿// *************************************************
// MMG.Infra.EFPersistence.EFDbContext.cs
// Last Modified: 08/29/2013 3:10 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Data;
using System.Data.Entity;
using MMG.Core.Persistence;

namespace MMG.Infra.EFPersistence
{
    /// <summary>
    /// Abstract class that simply wraps the EntityFramework DbContext object and tags the object with <see cref="IDbContext"/>.
    /// </summary>
    public abstract class EFDbContext : DbContext, IDbContext
    {
        public void Add<TEntity>(TEntity pEntity) where TEntity : class, IDbEntity
        {
            Set<TEntity>().Add(pEntity);
        }

        public void CloseConnection()
        {
            if (Database.Connection.State != ConnectionState.Closed)
                Database.Connection.Close();
        }
    }
}