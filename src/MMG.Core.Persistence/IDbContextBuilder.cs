// *************************************************
// MMG.Core.Persistence.IDbContextBuilder.cs
// Last Modified: 08/29/2013 12:13 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

namespace MMG.Core.Persistence
{
    public interface IDbContextBuilder<out TContext>
        where TContext : IDbContext
    {
        TContext BuildDbContext();
    }
}