// *************************************************
// MMG.Core.Query.Specification.cs
// Last Modified: 08/29/2013 12:12 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Linq;
using System.Linq.Expressions;
using MMG.Core.Query.Extensions;

namespace MMG.Core.Query
{
    public class Specification<TEntity> : ISpecification<TEntity>
    {
        protected Specification() {}

        public Specification(Expression<Func<TEntity, bool>> pPredicate)
        {
            Predicate = pPredicate;
        }

        public Specification(CompositeSpecification<TEntity> pCompositeSpecification)
        {
            Predicate = pCompositeSpecification.Predicate;
        }

        public Expression<Func<TEntity, bool>> Predicate { get; protected set; }

        public Specification<TEntity> And(Specification<TEntity> pSpecification)
        {
            return new Specification<TEntity>(Predicate.And(pSpecification.Predicate));
        }

        public Specification<TEntity> And(Expression<Func<TEntity, bool>> pPredicate)
        {
            return new Specification<TEntity>(Predicate.And(pPredicate));
        }

        public Specification<TEntity> Or(Specification<TEntity> pSpecification)
        {
            return new Specification<TEntity>(Predicate.Or(pSpecification.Predicate));
        }

        public Specification<TEntity> Or(Expression<Func<TEntity, bool>> pPredicate)
        {
            return new Specification<TEntity>(Predicate.Or(pPredicate));
        }

        public bool IsSatisfiedBy(TEntity pObject)
        {
            return Predicate.Compile().Invoke(pObject);
        }

        public TEntity SatisfyingEntityFrom(IQueryable<TEntity> pQuery)
        {
            return pQuery.Where(Predicate).SingleOrDefault();
        }

        public IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> pQuery)
        {
            return pQuery.Where(Predicate);
        }
    }
}