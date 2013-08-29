// *************************************************
// MMG.Core.Query.ISpecification.cs
// Last Modified: 08/29/2013 12:12 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Linq;
using System.Linq.Expressions;

namespace MMG.Core.Query
{
    /// <summary>
    /// In simple terms, a specification is a small piece of logic which is independent and give an answer 
    /// to the question “does this match ?”. With Specification, we isolate the logic that do the selection 
    /// into a reusable business component that can be passed around easily from the entity we are selecting.
    /// </summary>
    /// <see cref="http://en.wikipedia.org/wiki/Specification_pattern"/>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISpecification<TEntity>
    {
        Expression<Func<TEntity, bool>> Predicate { get; }

        bool IsSatisfiedBy(TEntity pObject);

        TEntity SatisfyingEntityFrom(IQueryable<TEntity> pQuery);

        IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> pQuery);
    }
}