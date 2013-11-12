using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MMG.Core.Persistence;

namespace MMG.Core.Testing.Integration.Northwind
{
    [Table("Suppliers")]
    [DisplayColumn("Name")]
    public class Supplier : IDbEntity, IEquatable<Supplier>
    {
        [Key]
        [Column("SupplierID")]
        public int Id { get; set; }

        [Required]
        [Column("CompanyName")]
        [MaxLength(40)]
        public string Name { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public bool Equals(Supplier pOther)
        {
            return Id.Equals(pOther.Id);
        }
    }
}
