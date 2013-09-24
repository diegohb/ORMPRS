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
        #region Constructors

        public AndSpecification(Specification<TEntity> pLeftSide, Specification<TEntity> pRightSide)
            : base(pLeftSide, pRightSide) {}

        public AndSpecification(Expression<Func<TEntity, bool>> pLeftSide, Specification<TEntity> pRightSide) : base(pLeftSide, pRightSide) {}

        public AndSpecification(Specification<TEntity> pLeftSide, Expression<Func<TEntity, bool>> pRightSide) : base(pLeftSide, pRightSide) {}

        public AndSpecification(Expression<Func<TEntity, bool>> pLeftSide, Expression<Func<TEntity, bool>> pRightSide) : base(pLeftSide, pRightSide) {}

        #endregion


        public override Expression<Func<TEntity, bool>> Predicate
        {
            get { return _leftSide.Predicate.And(_rightSide.Predicate); }
        }
    }
}