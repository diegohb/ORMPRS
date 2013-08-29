// *************************************************
// MMG.Core.Persistence.IDbContextManager.cs
// Last Modified: 08/29/2013 2:27 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

namespace MMG.Core.Persistence
{
    public interface IDbContextManager
    {
        /// <summary>
        /// Represents the default key (connection string name) to use for the default Builder accessed by <see cref="Current"/>.
        /// </summary>
        string DefaultConnectionStringName { get; }

        /// <summary>
        /// Will configure the default context builder under key <see cref="DefaultConnectionStringName" />
        /// </summary>
        /// <param name="pDbContextConfig">The orm-specific configuration object to pass to the context builder.</param>
        void AddContextBuilder(IDbContextConfiguration pDbContextConfig);

        /// <summary>
        /// Will add configuration for context builder with specified key.
        /// </summary>
        /// <param name="pConnectionStringName">The key to use to store the context builder.</param>
        /// <param name="pDbContextConfig">The orm-specific configuration object to pass to the context builder.</param>
        void AddContextBuilder(string pConnectionStringName, IDbContextConfiguration pDbContextConfig);

        void InitStorage(IDbContextStorage pStorage);

        /// <summary>
        /// Used to get the current db context session if you're communicating with a single database.
        /// When communicating with multiple databases, invoke <see cref="CurrentFor(string)" /> instead.
        /// </summary>
        IDbContext Current { get; }

        /// <summary>
        /// Used to get the current DbContext associated with a key; i.e., the key 
        /// associated with an object context for a specific database.
        /// 
        /// If you're only communicating with one database, you should call <see cref="IDbContextManager.Current" /> instead,
        /// although you're certainly welcome to call this if you have the key available.
        /// </summary>
        IDbContext CurrentFor(string pKey);

        /// <summary>
        /// This method is used by application-specific db context storage implementations
        /// and unit tests. Its job is to walk thru existing cached object context(s) and Close() each one.
        /// </summary>
        void CloseAllDbContexts();
    }
}