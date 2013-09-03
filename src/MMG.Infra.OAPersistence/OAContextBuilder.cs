// *************************************************
// MMG.Infra.OAPersistence.OAContextBuilder.cs
// Last Modified: 08/30/2013 9:23 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MMG.Core.Persistence;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata.Fluent;

namespace MMG.Core.OAPersistence
{
    public class OAContextBuilder<TContext> : IDbContextBuilder<TContext>
    where TContext : OADbContext, IDbContext
    {
        private readonly DynamicMetadataSource _metadataSource;
        private readonly string _connectionStringName;
        private readonly bool _updateDatabaseSchema;
        private readonly string _backendConfig;

        public OAContextBuilder(string pConnectionStringName, OAContextConfiguration pContextConfig)
        {
            if (string.IsNullOrEmpty(pConnectionStringName))
                throw new ArgumentNullException("pConnectionStringName");

            if (string.IsNullOrEmpty(pContextConfig.BackendConfig))
                throw new ArgumentException("The property 'BackendConfig' must be specified.", "pContextConfig");

            _connectionStringName = pConnectionStringName;
            _updateDatabaseSchema = pContextConfig.UpdateDatabaseSchema;
            _backendConfig = pContextConfig.BackendConfig;
            _metadataSource = new DynamicMetadataSource(getMappingConfigurations(pContextConfig.MappingAssemblies));
        }

        public TContext BuildDbContext()
        {
            var backendConfig = new BackendConfiguration { Backend = _backendConfig };
            var dbContext = (TContext) new OADbContext(_connectionStringName, backendConfig, _metadataSource);
            if (_updateDatabaseSchema)
                updateSchema(dbContext.GetSchemaHandler());

            return dbContext;
        }

        private void updateSchema(ISchemaHandler pSchemaHandler)
        {
            string script = null;
            try
            {
                script = pSchemaHandler.CreateUpdateDDLScript(null);
            }
            catch
            {
                bool throwException = false;
                try
                {
                    pSchemaHandler.CreateDatabase();
                    script = pSchemaHandler.CreateDDLScript();
                }
                catch
                {
                    throwException = true;
                }
                if (throwException)
                    throw;
            }

            if (string.IsNullOrEmpty(script) == false)
            {
                pSchemaHandler.ExecuteDDLScript(script);
            }
        }

        private IEnumerable<MappingConfiguration> getMappingConfigurations(IEnumerable<string> pMappingAssemblies)
        {
            var foundMappingClass = false;
            var assemblies = pMappingAssemblies.Select(Assembly.Load);
            var configurations = new List<MappingConfiguration>();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes().Where
                    (pType => !pType.IsAbstract
                              && pType.GetInterfaces().Contains(typeof(IMapEntityToDb))
                              && isMappingClass(pType.BaseType)))
                {
                    foundMappingClass = true;
                    dynamic mappingInstance = Activator.CreateInstance(type);
                    configurations.Add(mappingInstance);
                }
            }

            if (!foundMappingClass)
                throw new ArgumentException("No mapping class found!");

            return configurations;
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
            var baseType = typeof(MappingConfiguration<>);

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
    }

}