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
    public interface IMapEntityToDb
    {
        /// <summary>
        /// The connection string name associated with the data context for the entity it maps.
        /// </summary>
        /// <remarks>This is required when mapping configurations within an assembly are used for more than 1 data context.
        /// It is used by the <see cref="IDbContextBuilder{TContext}"/> when adding mapping configurations.
        /// Should return empty/null string when all implementations of IMapEntityToDb within the assembly can be added to the data context(s).</remarks>
        string ConnectionStringName { get; }
    }

    /// <summary>
    /// marker interface for entity mappers/configuration classes
    /// </summary>
    public interface IMapEntityToDb<TEntity> : IMapEntityToDb
        where TEntity : IDbEntity {}
}