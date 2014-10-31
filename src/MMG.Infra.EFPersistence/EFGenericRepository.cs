// *************************************************
// MMG.Infra.EFPersistence.EFGenericRepository.cs
// Last Modified: 10/31/2014 9:35 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

namespace MMG.Infra.EFPersistence
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using Core.Persistence;
    using Core.Persistence.Exceptions;
    using Core.Query;

    /// <summary>
    /// Generic repository
    /// </summary>
    public class EFGenericRepository : IRepository, IDisposable
    {
        private readonly string _connectionStringName;
        private DbContext _context;
        private IUnitOfWork _unitOfWork;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EFGenericRepository"/> class.
        /// </summary>
        /// <param name="pConnectionStringProvider">The connection string provider.</param>
        public EFGenericRepository(IProvideConnectionString pConnectionStringProvider)
        {
            if (pConnectionStringProvider != null && !string.IsNullOrEmpty(pConnectionStringProvider.ConnectionString))
                _connectionStringName = pConnectionStringProvider.ConnectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EFGenericRepository"/> class.
        /// </summary>
        /// <param name="pContext">The context.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public EFGenericRepository(DbContext pContext)
        {
            if (pContext == null)
                throw new ArgumentNullException("pContext");
            _context = pContext;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EFGenericRepository"/> class.
        /// </summary>
        /// <param name="pContext">The context.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public EFGenericRepository(ObjectContext pContext)
        {
            if (pContext == null)
                throw new ArgumentNullException("pContext");
            _context = new DbContext(pContext, true);
        }

        #endregion

        public TEntity GetByKey<TEntity>(object keyValue, params string[] pExpandPropertyNames)
            where TEntity : class
        {
            EntityKey key = getEntityKey<TEntity>(keyValue);

            object originalItem;
            if (((IObjectContextAdapter) DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                var entity = (TEntity) originalItem;

                //this will eager-load related entities.
                expandProperties(entity, pExpandPropertyNames);

                return entity;
            }
            return default(TEntity);
        }

        public IQueryable<TEntity> GetQuery<TEntity>(params Expression<Func<TEntity, object>>[] pExpandPropertySelectors) where TEntity : class
        {
            /* 
             * From CTP4, I could always safely call this to return an IQueryable on DbContext 
             * then performed any with it without any problem:
             */
            // return DbContext.Set<TEntity>();

            /*
             * but with 4.1 release, when I call GetQuery<TEntity>().AsEnumerable(), there is an exception:
             * ... System.ObjectDisposedException : The ObjectContext instance has been disposed and can no longer be used for operations that require a connection.
             */

            // here is a work around: 
            // - cast DbContext to IObjectContextAdapter then get ObjectContext from it
            // - call CreateQuery<TEntity>(entityName) method on the ObjectContext
            // - perform querying on the returning IQueryable, and it works!
            var entityName = getEntityName<TEntity>();
            var objectQuery = ((IObjectContextAdapter) DbContext).ObjectContext.CreateQuery<TEntity>(entityName);
            if (pExpandPropertySelectors != null)
            {
                objectQuery = pExpandPropertySelectors.Aggregate
                    (objectQuery, (pCurrent, pInclude) => pCurrent.Include(pInclude) as ObjectQuery<TEntity>);
            }
            return objectQuery;
        }

        public IQueryable<TEntity> GetQuery<TEntity>
            (Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] pExpandPropertySelectors) where TEntity : class
        {
            return GetQuery(pExpandPropertySelectors).Where(predicate);
        }

        public IQueryable<TEntity> GetQuery<TEntity>
            (ISpecification<TEntity> criteria, params Expression<Func<TEntity, object>>[] pExpandPropertySelectors) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery(pExpandPropertySelectors));
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>
            (Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending)
            where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery<TEntity>().OrderBy(orderBy).Skip((pageIndex - 1)*pageSize).Take(pageSize).AsEnumerable();
            }
            return GetQuery<TEntity>().OrderByDescending(orderBy).Skip((pageIndex - 1)*pageSize).Take(pageSize).AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>
            (Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize,
                SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery<TEntity>(criteria).OrderBy(orderBy).Skip((pageIndex - 1)*pageSize).Take(pageSize).AsEnumerable();
            }
            return GetQuery<TEntity>(criteria).OrderByDescending(orderBy).Skip((pageIndex - 1)*pageSize).Take(pageSize).AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>
            (ISpecification<TEntity> specification, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize,
                SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return
                    specification.SatisfyingEntitiesFrom(GetQuery<TEntity>())
                        .OrderBy(orderBy)
                        .Skip((pageIndex - 1)*pageSize)
                        .Take(pageSize)
                        .AsEnumerable();
            }
            return
                specification.SatisfyingEntitiesFrom(GetQuery<TEntity>())
                    .OrderByDescending(orderBy)
                    .Skip((pageIndex - 1)*pageSize)
                    .Take(pageSize)
                    .AsEnumerable();
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
            {
                throw new ArgumentNullException("entity");
            }
            DbContext.Set<TEntity>().Add(entity);
        }

        public TEntity Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            return DbContext.Set<TEntity>().Attach(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            DbContext.Set<TEntity>().Remove(entity);
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

        public TEntity GetOriginal<TEntity>(object pEntityInstance) where TEntity : class
        {
            var fqen = getEntityName<TEntity>();

            object existingItem;
            var key = ((IObjectContextAdapter) DbContext).ObjectContext.CreateEntityKey(fqen, pEntityInstance);
            if (((IObjectContextAdapter) DbContext).ObjectContext.TryGetObjectByKey(key, out existingItem))
            {
                return (TEntity) existingItem;
            }

            return null;
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
            var fqen = getEntityName<TEntity>();

            object originalItem;
            EntityKey key = ((IObjectContextAdapter) DbContext).ObjectContext.CreateEntityKey(fqen, entity);
            if (((IObjectContextAdapter) DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                ((IObjectContextAdapter) DbContext).ObjectContext.ApplyCurrentValues(key.EntitySetName, entity);
            }
            return entity;
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
            get { return _unitOfWork ?? (_unitOfWork = new EFUnitOfWork(DbContext)); }
        }

        private EntityKey getEntityKey<TEntity>(object keyValue) where TEntity : class
        {
            var entitySetName = getEntityName<TEntity>();
            var objectSet = ((IObjectContextAdapter) DbContext).ObjectContext.CreateObjectSet<TEntity>();
            var keyPropertyName = objectSet.EntitySet.ElementType.KeyMembers[0].ToString();
            var entityKey = new EntityKey(entitySetName, new[] {new EntityKeyMember(keyPropertyName, keyValue)});
            return entityKey;
        }

        private string getEntityName<TEntity>() where TEntity : class
        {
            // original - http://huyrua.wordpress.com/2011/04/13/entity-framework-4-poco-repository-and-specification-pattern-upgraded-to-ef-4-1/#comment-688
            var className = typeof (TEntity).Name;
            var objContext = ((IObjectContextAdapter) DbContext).ObjectContext;
            var container = objContext.MetadataWorkspace.GetEntityContainer(objContext.DefaultContainerName, DataSpace.CSpace);
            var entitySetName = container.BaseEntitySets
                .Where(pMeta => pMeta.ElementType.Name == className)
                .Select(pMeta => pMeta.Name).FirstOrDefault();

            if (entitySetName == null)
                throw new PersistenceException(string.Format("The mapping for entity type '{0}' can not be found.", className));

            return string.Format("{0}.{1}", objContext.DefaultContainerName, entitySetName);
        }

        /// <summary>
        /// Will walk through a dot-seperated string of property names to load related entities (navigation properties) on a single object.
        /// </summary>
        /// <param name="pEntity">The entity to expand properties on.</param>
        /// <param name="pExpandPropertyNames">List of properties to expand.</param>
        private void expandProperties<TEntity>(TEntity pEntity, string[] pExpandPropertyNames) where TEntity : class
        {
            foreach (var expandPropertyName in pExpandPropertyNames)
            {
                if (expandPropertyName.Contains("."))
                {
                    var navPropName = expandPropertyName.Split('.')[0];
                    var entity = pEntity.GetType().GetProperty(navPropName).GetValue(pEntity, null);
                    expandProperties(entity, new[] {expandPropertyName.Substring(expandPropertyName.IndexOf('.') + 1)});
                }
                else
                {
                    var expandProp = pEntity.GetType().GetProperty(expandPropertyName);
                    if (expandProp == null)
                        return;

                    var isGeneric = expandProp.PropertyType.IsGenericType;
                    //TODO: need better way to determine if it is a nav prop.
                    if (!isGeneric)
                        DbContext.Entry(pEntity).Reference(expandPropertyName).Load();
                    else
                        DbContext.Entry(pEntity).Collection(expandPropertyName).Load();
                }
            }
        }

        private DbContext DbContext
        {
            get
            {
                if (_context == null)
                {
                    if (_connectionStringName == string.Empty)
                        _context = (DbContext) EFContextManager.Instance.Current;
                    else
                        _context = (DbContext) EFContextManager.Instance.CurrentFor(_connectionStringName);
                }
                return _context;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool pDisposing)
        {
            if (!pDisposing) return;
            if (_context == null) return;

            _context.Dispose();
            _context = null;
        }
    }
}