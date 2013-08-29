// *************************************************
// MMG.Core.Persistence.IMapEntityToDb.cs
// Last Modified: 08/29/2013 12:13 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

namespace MMG.Core.Persistence
{
    /// <summary>
    /// marker interface for entity mappers/configuration classes
    /// </summary>
    public interface IMapEntityToDb {}

    /// <summary>
    /// marker interface for entity mappers/configuration classes
    /// </summary>
    public interface IMapEntityToDb<TEntity> : IMapEntityToDb
        where TEntity : IDbEntity {}
}