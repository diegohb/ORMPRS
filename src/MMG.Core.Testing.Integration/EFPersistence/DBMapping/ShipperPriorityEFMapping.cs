// *************************************************
// MMG.Core.Testing.Integration.ShipperPriorityEFMapping.cs
// Last Modified: 11/11/2013 1:21 PM
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
    }
}