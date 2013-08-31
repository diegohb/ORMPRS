using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMG.Core.Testing.Integration.Northwind
{
    [Table("Customers")]
    [DisplayColumn("Name")]
    public class Customer
    {
        [Key]
        [Column("Customer ID")]
        [MaxLength(5)]
        public string Id { get; set; }

        [Required]
        [Column("Company Name")]
        [MaxLength(40)]
        public string Name { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
