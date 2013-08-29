// *************************************************
// MMG.Core.Query.CompositeSpecification.cs
// Last Modified: 08/29/2013 12:12 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Linq;
using System.Linq.Expressions;

namespace MMG.Core.Query
{
    /// <summary>
    /// http://devlicio.us/blogs/jeff_perrin/archive/2006/12/13/the-specification-pattern.aspx
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class CompositeSpecification<TEntity> : ISpecification<TEntity>
    {
        protected readonly Specification<TEntity> _leftSide;
        protected readonly Specification<TEntity> _rightSide;

        public CompositeSpecification(Specification<TEntity> pLeftSide, Specification<TEntity> pRightSide)
        {
            _leftSide = pLeftSide;
            _rightSide = pRightSide;
        }

        public abstract Expression<Func<TEntity, bool>> Predicate { get; }

        public bool IsSatisfiedBy(TEntity pObject)
        {
            return Predicate.Compile().Invoke(pObject);
        }

        public virtual TEntity SatisfyingEntityFrom(IQueryable<TEntity> pQuery)
        {
            //TODO: test single instead of firstordefault()
            return SatisfyingEntitiesFrom(pQuery).SingleOrDefault();
        }

        public virtual IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> pQuery)
        {
            return pQuery.Where(Predicate);
        }
    }
}