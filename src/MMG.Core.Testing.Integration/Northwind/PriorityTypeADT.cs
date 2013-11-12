// *************************************************
// MMG.Core.Testing.Integration.PriorityTypeADT.cs
// Last Modified: 11/12/2013 7:33 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.ComponentModel.DataAnnotations.Schema;
using MMG.Core.Model;
using MMG.Core.Persistence;

namespace MMG.Core.Testing.Integration.Northwind
{
    public enum ShipperPriorityEnum : byte
    {
        High = 1,
        Normal = 2,
        Low = 3
    }

    [ComplexType]
    public class PriorityTypeADT : NullableByteEnumADT<ShipperPriorityEnum>, IDbEntity
    {
        public PriorityTypeADT() {}

        public PriorityTypeADT(ShipperPriorityEnum pValue) : base(pValue) {}

        public PriorityTypeADT(string pValue) : base(pValue) {}

        public PriorityTypeADT(byte pValue) : base(pValue) {}

        public override byte? Value
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