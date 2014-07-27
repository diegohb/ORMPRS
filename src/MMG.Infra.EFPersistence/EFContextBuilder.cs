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
using System.IO;
using System.Reflection;
using MMG.Core.Persistence;
using System.Linq;
using MMG.Core.Persistence.Exceptions;

namespace MMG.Infra.EFPersistence
{
    public class EFContextBuilder<TContext> : DbModelBuilder, IDbContextBuilder<TContext>
        where TContext : EFDbContext, IDbContext
    {
        private readonly EFContextConfiguration _contextConfig;
        private readonly DbProviderFactory _factory;
        private readonly ConnectionStringSettings _cnStringSettings;
        

        public EFContextBuilder(string pConnectionStringName, EFContextConfiguration pContextConfig)
        {
            _contextConfig = pContextConfig;
            _cnStringSettings = ConfigurationManager.ConnectionStrings[pConnectionStringName];
            if (_cnStringSettings == null)
                throw new PersistenceException
                    (string.Format("Unable to load connection settings for connection string '{0}'.", pConnectionStringName));

            _factory = DbProviderFactories.GetFactory(_cnStringSettings.ProviderName);
            _contextConfig = pContextConfig;

            if (_contextConfig.MappingAssemblies.Count > 0)
                addConfigurations(_contextConfig.MappingAssemblies);
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
            ctx.ContextOptions.LazyLoadingEnabled = _contextConfig.LazyLoadingEnabled;
            ctx.ContextOptions.ProxyCreationEnabled = _contextConfig.ProxyCreationEnabled;
            ctx.ContextOptions.UseLegacyPreserveChangesBehavior = _contextConfig.UseLegacyPreserveChangesBehavior;

            if (!ctx.DatabaseExists())
            {
                ctx.CreateDatabase();
            }
            else if (_contextConfig.RecreateDatabaseIfExists)
            {
                ctx.DeleteDatabase();
                ctx.CreateDatabase();
            }

            return (TContext) new EFDbContext(ctx, true);
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

                Assembly asm;
                
                try
                {
                    asm = AppDomain.CurrentDomain.Load(mappingAssembly);
                }
                catch (Exception)
                {
                    try
                    {
                        asm = Assembly.LoadFrom(makeLoadReadyAssemblyName(mappingAssembly));
                    }
                    catch (Exception e)
                    {
                        throw new PersistenceException
                            (string.Format("Unable to find mapping assembly '{0}'. You must provide an assembly display name or a full path to a dll.", mappingAssembly), e);
                    }
                }
                

                foreach (var type in asm.GetTypes())
                {
                    if (type.IsAbstract) continue;

                    if (type.BaseType == null ||
                        !type.GetInterfaces().Contains(typeof (IMapEntityToDb))
                        || (!isMappingClass(type.BaseType))) continue;
                    
                    // http://areaofinterest.wordpress.com/2010/12/08/dynamically-load-entity-configurations-in-ef-codefirst-ctp5/
                    dynamic mappingInstance = Activator.CreateInstance(type);

                    var configuredConnectionName = ((IMapEntityToDb)mappingInstance).ConnectionStringName;
                    if(!string.IsNullOrEmpty(configuredConnectionName) && !_cnStringSettings.Name.Equals(configuredConnectionName, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    hasMappingClass = true;
                    Configurations.Add(mappingInstance);
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
            var complexConfigType = typeof (ComplexTypeConfiguration<>);
            
            if (pMappingType.IsGenericType
                 && (pMappingType.GetGenericTypeDefinition() == baseType || pMappingType.GetGenericTypeDefinition() == complexConfigType))
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
            var rootPath = !Path.IsPathRooted(pAssemblyName) ? AppDomain.CurrentDomain.RelativeSearchPath : string.Empty;
            var assemblyNameWithExtension = !pAssemblyName.EndsWith(".dll") ? pAssemblyName.Trim() + ".dll" : pAssemblyName.Trim();
            return Path.Combine(rootPath, assemblyNameWithExtension);
        }
    }
}