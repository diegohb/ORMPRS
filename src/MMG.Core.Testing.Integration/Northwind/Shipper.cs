using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMG.Core.Testing.Integration.Northwind
{
    [Table("Shippers")]
    [DisplayColumn("Name")]
    public class Shipper
    {
        [Key]
        [Column("Shipper ID")]
        public int Id { get; set; }
        
        [Required]
        [Column("Company Name")]
        [MaxLength(40)]
        public string Name { get; set; }
    }
}
