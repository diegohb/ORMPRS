// *************************************************
// MMG.Infra.EFPersistence.EFContextBuilder.cs
// Last Modified: 08/29/2013 3:03 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using System.Reflection;
using MMG.Core.Persistence;
using System.Linq;

namespace MMG.Infra.EFPersistence
{
    public class EFContextBuilder<TContext> : DbModelBuilder, IDbContextBuilder<TContext>
        where TContext : DbContext, IDbContext
    {
        private readonly DbProviderFactory _factory;
        private readonly ConnectionStringSettings _cnStringSettings;
        private readonly bool _recreateDatabaseIfExists;
        private readonly bool _lazyLoadingEnabled;

        public EFContextBuilder(string pConnectionStringName, EFContextConfiguration pContextConfig)
        {
            _cnStringSettings = ConfigurationManager.ConnectionStrings[pConnectionStringName];
            _factory = DbProviderFactories.GetFactory(_cnStringSettings.ProviderName);
            _recreateDatabaseIfExists = pContextConfig.RecreateDatabaseIfExists;
            _lazyLoadingEnabled = pContextConfig.LazyLoadingEnabled;

            if (pContextConfig.MappingAssemblies.Count > 0)
                addConfigurations(pContextConfig.MappingAssemblies);
        }

        /// <summary>
        /// Creates a new <see cref="TContext"/>.
        /// </summary>
        /// <returns></returns>
        public TContext BuildDbContext()
        {
            var cn = _factory.CreateConnection();
            cn.ConnectionString = _cnStringSettings.ConnectionString;

            var dbModel = base.Build(cn);

            var ctx = dbModel.Compile().CreateObjectContext<ObjectContext>(cn);
            ctx.ContextOptions.LazyLoadingEnabled = _lazyLoadingEnabled;

            if (!ctx.DatabaseExists())
            {
                ctx.CreateDatabase();
            }
            else if (_recreateDatabaseIfExists)
            {
                ctx.DeleteDatabase();
                ctx.CreateDatabase();
            }

            return (TContext) new DbContext(ctx, true);
        }

        /// <summary>
        /// Adds mapping classes contained in provided assemblies and register entities as well
        /// </summary>
        /// <param name="pMappingAssemblies"></param>
        private void addConfigurations(ICollection<string> pMappingAssemblies)
        {
            if (pMappingAssemblies == null || pMappingAssemblies.Count == 0)
            {
                throw new ArgumentNullException("pMappingAssemblies", "You must specify at least one mapping assembly");
            }

            var hasMappingClass = false;
            foreach (var mappingAssembly in pMappingAssemblies)
            {
                var asm = Assembly.LoadFrom(makeLoadReadyAssemblyName(mappingAssembly));

                foreach (var type in asm.GetTypes())
                {
                    if (type.IsAbstract) continue;

                    if (type.BaseType == null ||
                        !type.GetInterfaces().Contains(typeof (IMapEntityToDb))
                        || (!isMappingClass(type.BaseType))) continue;
                    
                    hasMappingClass = true;
                    // http://areaofinterest.wordpress.com/2010/12/08/dynamically-load-entity-configurations-in-ef-codefirst-ctp5/
                    dynamic configurationInstance = Activator.CreateInstance(type);
                    Configurations.Add(configurationInstance);
                }
            }

            if (!hasMappingClass)
                throw new ArgumentException("No mapping class found!");
        }

        /// <summary>
        /// Determines whether a type is a subclass of entity mapping type
        /// </summary>
        /// <param name="pMappingType">Type of the mapping.</param>
        /// <returns>
        /// 	<c>true</c> if it is mapping class; otherwise, <c>false</c>.
        /// </returns>
        private static bool isMappingClass(Type pMappingType)
        {
            var baseType = typeof (EntityTypeConfiguration<>);
            
            if (pMappingType.IsGenericType
                 && pMappingType.GetGenericTypeDefinition() == baseType)
                return true;

            if ((pMappingType.BaseType != null) &&
                !pMappingType.BaseType.IsAbstract &&
                pMappingType.BaseType.IsGenericType)
            {
                return isMappingClass(pMappingType.BaseType);
            }

            return false;
        }

        /// <summary>
        /// Ensures the assembly name is qualified
        /// </summary>
        /// <param name="pAssemblyName">The name of the assembly to fix.</param>
        /// <returns></returns>
        private static string makeLoadReadyAssemblyName(string pAssemblyName)
        {
            return (!pAssemblyName.EndsWith(".dll"))
                       ? pAssemblyName.Trim() + ".dll"
                       : pAssemblyName.Trim();
        }
    }
}