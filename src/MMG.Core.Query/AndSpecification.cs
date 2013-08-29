// *************************************************
// MMG.Core.Query.AndSpecification.cs
// Last Modified: 08/29/2013 12:12 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Linq.Expressions;
using MMG.Core.Query.Extensions;

namespace MMG.Core.Query
{
    public class AndSpecification<TEntity> : CompositeSpecification<TEntity>
    {
        public AndSpecification(Specification<TEntity> pLeftSide, Specification<TEntity> pRightSide)
            : base(pLeftSide, pRightSide) {}

        public override Expression<Func<TEntity, bool>> Predicate
        {
            get { return _leftSide.Predicate.And(_rightSide.Predicate); }
        }
    }
}