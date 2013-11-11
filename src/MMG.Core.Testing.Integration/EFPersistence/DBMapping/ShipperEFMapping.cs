// *************************************************
// MMG.Core.Testing.Integration.ShipperEFMapping.cs
// Last Modified: 11/11/2013 2:01 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MMG.Core.Persistence;
using MMG.Core.Testing.Integration.Northwind;

namespace MMG.Core.Testing.Integration.EFPersistence.DBMapping
{
    public class ShipperEFMapping : EntityTypeConfiguration<Shipper>, IMapEntityToDb<Shipper>
    {
        public ShipperEFMapping()
        {
            ToTable("Shippers");
            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("ShipperID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.Name).HasColumnName("CompanyName").HasMaxLength(40).IsRequired();
            Property(p => p.PhoneNumber).HasColumnName("Phone").HasMaxLength(24).IsOptional();
        }
    }
}