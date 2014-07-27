// *************************************************
// MMG.Core.Testing.Integration.CountryEFMapping.cs
// Last Modified: 11/12/2013 8:27 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.Data.Entity.ModelConfiguration;
using MMG.Core.Persistence;
using MMG.Core.Testing.Integration.Northwind;

namespace MMG.Core.Testing.Integration.EFPersistence.DBMapping
{
    public class CountryEFMapping : ComplexTypeConfiguration<CountryStringEnumADT>, IMapEntityToDb<CountryStringEnumADT>
    {
        public CountryEFMapping()
        {
            Property(p => p.Value).HasColumnName("Country").IsOptional().HasMaxLength(15);
        }

        public string ConnectionStringName { get { return string.Empty; } }
    }
}