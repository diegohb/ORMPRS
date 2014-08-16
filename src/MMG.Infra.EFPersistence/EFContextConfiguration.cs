// *************************************************
// MMG.Infra.EFPersistence.EFContextConfiguration.cs
// Last Modified: 08/30/2013 11:41 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Collections.Generic;
using System.Linq;
using MMG.Core.Persistence;

namespace MMG.Infra.EFPersistence
{
    public class EFContextConfiguration : IDbContextConfiguration
    {
        public EFContextConfiguration()
        {
            MappingAssemblies = new List<string>();
            LazyLoadingEnabled = false;
            ProxyCreationEnabled = false;
            UseLegacyPreserveChangesBehavior = false;
        }

        public EFContextConfiguration(IEnumerable<string> pMappingAssemblies)
        {
            MappingAssemblies = pMappingAssemblies.ToList();
        }

        public IList<string> MappingAssemblies { get; set; }

        /// <summary>
        /// if set to <c>true</c> [recreate database if exist].
        /// </summary>
        public bool RecreateDatabaseIfExists { get; set; }

        /// <summary>
        /// if set to <c>true</c> [lazy loading enabled].
        /// </summary>
        public bool LazyLoadingEnabled { get; set; }

        /// <summary>
        /// if set to <c>true</c> then proxy objects will be created by EF.
        /// </summary>
        public bool ProxyCreationEnabled { get; set; }

        /// <summary>
        /// if set to <c>true</c> then legacy behavior will be used for changes.
        /// </summary>
        public bool UseLegacyPreserveChangesBehavior { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that determines whether to use the C# NullComparison behavior.
        /// </summary>
        /// 
        /// <remarks>
        /// This flag determines whether C# behavior should be exhibited when comparing null values in LinqToEntities.
        ///             If this flag is set, then any equality comparison between two operands, both of which are potentially
        ///             nullable, will be rewritten to show C# null comparison semantics. As an example:
        ///             (operand1 = operand2) will be rewritten as
        ///             (((operand1 = operand2) AND NOT (operand1 IS NULL OR operand2 IS NULL)) || (operand1 IS NULL &amp;&amp; operand2 IS NULL))
        ///             The default value is false when using <see cref="T:System.Data.Entity.Core.Objects.ObjectContext"/>.
        /// 
        /// </remarks>
        /// 
        /// <returns>
        /// true if the C# NullComparison behavior should be used; otherwise, false.
        /// </returns>
        /// <remarks>from EF6.1.1</remarks>
        public bool UseCSharpNullComparisonBehavior { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that determines whether to use the consistent NullReference behavior.
        /// </summary>
        /// 
        /// <remarks>
        /// If this flag is set to false then setting the Value property of the <see cref="T:System.Data.Entity.Core.Objects.DataClasses.EntityReference`1"/> for an
        ///             FK relationship to null when it is already null will have no effect. When this flag is set to true, then
        ///             setting the value to null will always cause the FK to be nulled and the relationship to be deleted
        ///             even if the value is currently null. The default value is false when using ObjectContext and true
        ///             when using DbContext.
        /// 
        /// </remarks>
        /// 
        /// <returns>
        /// true if the consistent NullReference behavior should be used; otherwise, false.
        /// </returns>
        /// <remarks>from EF6.1.1</remarks>
        public bool UseConsistentNullReferenceBehavior { get; set; }
    }
}