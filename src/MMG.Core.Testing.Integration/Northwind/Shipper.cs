using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MMG.Core.Model;
using MMG.Core.Persistence;

namespace MMG.Core.Testing.Integration.Northwind
{
    [Table("Shippers")]
    [DisplayColumn("Name")]
    public class Shipper : IDbEntity
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string PhoneNumber { get; set; }

        public virtual PriorityTypeADT PriorityLevel { get; set; }
    }

    public enum ShipperPriorityEnum : byte
    {
        High = 1,
        Normal = 2,
        Low = 3
    }

    [ComplexType]
    public class PriorityTypeADT : ByteEnumADT<ShipperPriorityEnum>, IDbEntity
    {
        public PriorityTypeADT() {}

        public PriorityTypeADT(ShipperPriorityEnum pValue) : base(pValue) { }

        public PriorityTypeADT(string pValue) : base(pValue) { }

        public PriorityTypeADT(byte pValue) : base(pValue) { }

        public override byte Value
        {
            get { return _underlyingValue; }
            set { setValue(value); }
        }

        public static implicit operator PriorityTypeADT(ShipperPriorityEnum pValue)
        {
            return new PriorityTypeADT(pValue);
        }
    }
}
