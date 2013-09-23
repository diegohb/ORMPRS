// *************************************************
// MMG.Core.Query.OrSpecification.cs
// Last Modified: 08/29/2013 12:12 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Linq.Expressions;
using MMG.Core.Query.Extensions;

namespace MMG.Core.Query
{
    public class OrSpecification<TEntity> : CompositeSpecification<TEntity>
    {
        #region Constructors

        public OrSpecification(Specification<TEntity> pLeftSide, Specification<TEntity> pRightSide)
            : base(pLeftSide, pRightSide) {}

        public OrSpecification(Expression<Func<TEntity, bool>> pLeftSide, Specification<TEntity> pRightSide) : base(pLeftSide, pRightSide) {}

        public OrSpecification(Specification<TEntity> pLeftSide, Expression<Func<TEntity, bool>> pRightSide) : base(pLeftSide, pRightSide) {}

        public OrSpecification(Expression<Func<TEntity, bool>> pLeftSide, Expression<Func<TEntity, bool>> pRightSide) : base(pLeftSide, pRightSide) {}

        #endregion


        public override Expression<Func<TEntity, bool>> Predicate
        {
            get { return _leftSide.Predicate.Or(_rightSide.Predicate); }
        }
    }
}