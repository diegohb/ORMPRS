// *************************************************
// MMG.Core.Query.DynamicSpecification.cs
// Last Modified: 10/11/2013 12:36 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Linq.Expressions;
using MMG.Core.Query.Extensions;

namespace MMG.Core.Query
{
    /// <summary>
    /// Allows building a chain of specifications (AND or OR).
    /// </summary>
    /// <typeparam name="TEntity">Type of the object to act on.</typeparam>
    public class AggregateSpecification<TEntity> : Specification<TEntity>
    {
        private readonly AggregateTypeEnum _aggregateType;

        public AggregateSpecification(AggregateTypeEnum pAggregateType)
        {
            _aggregateType = pAggregateType;
        }

        public void AddFilter(ISpecification<TEntity> pSpecification)
        {
            AddFilter(pSpecification.Predicate);
        }

        public void AddFilter(Expression<Func<TEntity, bool>> pQueryExpression)
        {
            if (!IsValid())
                Predicate = pQueryExpression;
            else
                Predicate = _aggregateType == AggregateTypeEnum.And
                                ? Predicate.And(pQueryExpression)
                                : Predicate.Or(pQueryExpression);
        }

        public bool IsValid()
        {
            return Predicate != null;
        }
    }
}