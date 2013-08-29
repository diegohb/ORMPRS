// *************************************************
// MMG.Core.Query.NotSpecification.cs
// Last Modified: 08/29/2013 4:38 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Linq.Expressions;

namespace MMG.Core.Query
{
    public class NotSpecification<TEntity> : Specification<TEntity>
    {
        public NotSpecification(Expression<Func<TEntity, bool>> pPredicate)
            : base((Expression<Func<TEntity, bool>>) pPredicate.Not()) {}

        public NotSpecification(Specification<TEntity> pPredicate)
            : base((Expression<Func<TEntity, bool>>) pPredicate.Predicate.Not()) {}
    }
}