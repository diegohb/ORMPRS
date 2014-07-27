// *************************************************
// MMG.Core.Testing.Integration.ShipperPriorityEFMapping.cs
// Last Modified: 11/12/2013 8:28 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Data.Entity.ModelConfiguration;
using MMG.Core.Persistence;
using MMG.Core.Testing.Integration.Northwind;

namespace MMG.Core.Testing.Integration.EFPersistence.DBMapping
{
    public class ShipperPriorityEFMapping : ComplexTypeConfiguration<PriorityTypeADT>, IMapEntityToDb<PriorityTypeADT>
    {
        public ShipperPriorityEFMapping()
        {
            Property(p => p.Value).HasColumnName("Priority").IsOptional();
        }

        public string ConnectionStringName { get { return string.Empty; } }
    }
}