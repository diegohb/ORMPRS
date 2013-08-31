// *************************************************
// MMG.Infra.OAPersistence.OAGenericRepository.cs
// Last Modified: 08/30/2013 1:30 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MMG.Core.Persistence;
using MMG.Core.Query;
using Telerik.OpenAccess;
using IUnitOfWork = MMG.Core.Persistence.IUnitOfWork;

namespace MMG.Core.OAPersistence
{
    /// <summary>
    /// Generic repository
    /// </summary>
    public class OAGenericRepository : IRepository
    {
        private readonly string _connectionStringName;
        private OpenAccessContext _context;
        private IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TEntity&gt;"/> class.
        /// </summary>
        public OAGenericRepository()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public OAGenericRepository(string connectionStringName)
        {
            this._connectionStringName = connectionStringName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public OAGenericRepository(OpenAccessContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            _context = context;
        }

        public TEntity GetByKey<TEntity>(object keyValue) where TEntity : class
        {
            var key = getEntityKey<TEntity>(keyValue);

            TEntity foundEntity;
            return DbContext.TryGetObjectByKey(key, out foundEntity)
                       ? foundEntity
                       : default(TEntity);
        }

        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class
        {
            return DbContext.GetAll<TEntity>();
        }

        public IQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return GetQuery<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> GetQuery<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>());
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery<TEntity>().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            }
            return GetQuery<TEntity>().OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery<TEntity>(criteria).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            }
            return GetQuery<TEntity>(criteria).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(ISpecification<TEntity> specification, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return specification.SatisfyingEntitiesFrom(GetQuery<TEntity>()).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            }
            return specification.SatisfyingEntitiesFrom(GetQuery<TEntity>()).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return GetQuery<TEntity>().Single<TEntity>(criteria);
        }

        public TEntity Single<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntityFrom(GetQuery<TEntity>());
        }

        public TEntity SingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return GetQuery<TEntity>().SingleOrDefault<TEntity>(criteria);
        }

        public TEntity SingleOrDefault<TEntity>(ISpecification<TEntity> pSpecification) where TEntity : class
        {
            return pSpecification.SatisfyingEntityFrom(GetQuery<TEntity>());
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return GetQuery<TEntity>().First(predicate);
        }

        public TEntity First<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).First();
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DbContext.Add(entity);
        }

        public TEntity Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            return DbContext.AttachCopy(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            DbContext.Delete(entity);
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            IEnumerable<TEntity> records = Find<TEntity>(criteria);

            foreach (TEntity record in records)
            {
                Delete<TEntity>(record);
            }
        }

        public void Delete<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            IEnumerable<TEntity> records = Find<TEntity>(criteria);
            foreach (TEntity record in records)
            {
                Delete<TEntity>(record);
            }
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return GetQuery<TEntity>().AsEnumerable();
        }

        public TEntity Save<TEntity>(TEntity entity) where TEntity : class
        {
            Add<TEntity>(entity);
            DbContext.SaveChanges();
            return entity;
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class
        {
            var attachedEntity = Attach(entity);
            return attachedEntity;
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return GetQuery<TEntity>().Where(criteria);
        }

        public TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return GetQuery<TEntity>().Where(criteria).FirstOrDefault();
        }

        public TEntity FindOne<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntityFrom(GetQuery<TEntity>());
        }

        public IEnumerable<TEntity> Find<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).AsEnumerable();
        }

        public int Count<TEntity>() where TEntity : class
        {
            return GetQuery<TEntity>().Count();
        }

        public int Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return GetQuery<TEntity>().Count(criteria);
        }

        public int Count<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).Count();
        }

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork ?? (_unitOfWork = new OAUnitOfWork(DbContext)); }
        }

        private ObjectKey getEntityKey<TEntity>(object keyValue) where TEntity : class
        {
            return new ObjectKey(typeof(TEntity).Name, keyValue);
        }
        private OpenAccessContext DbContext
        {
            get
            {
                if (_context == null)
                {
                    if (_connectionStringName == string.Empty)
                        _context = (OpenAccessContext)OAContextManager.Instance.Current;
                    else
                        _context = (OpenAccessContext)OAContextManager.Instance.CurrentFor(this._connectionStringName);
                }
                return this._context;
            }
        }
    }
}