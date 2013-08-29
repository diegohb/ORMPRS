// *************************************************
// MMG.Core.Query.QueryableTExtensions.cs
// Last Modified: 08/29/2013 12:12 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Linq;

namespace MMG.Core.Query.Extensions
{
    public static class QueryableTExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> pSource, ISpecification<T> pSpecification)
        {
            return pSpecification.SatisfyingEntitiesFrom(pSource);
        }
    }
}