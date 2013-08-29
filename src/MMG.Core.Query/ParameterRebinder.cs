// *************************************************
// MMG.Core.Query.ParameterRebinder.cs
// Last Modified: 08/29/2013 12:12 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Collections.Generic;
using System.Linq.Expressions;

namespace MMG.Core.Query
{
    /// <summary>
    /// http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx
    /// </summary>
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> pMap)
        {
            _map = pMap ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> pMap, Expression pExpression)
        {
            return new ParameterRebinder(pMap).Visit(pExpression);
        }

        protected override Expression VisitParameter(ParameterExpression pExpression)
        {
            ParameterExpression replacement;
            if (_map.TryGetValue(pExpression, out replacement))
            {
                pExpression = replacement;
            }
            return base.VisitParameter(pExpression);
        }
    }
}