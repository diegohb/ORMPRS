// *************************************************
// MMG.Core.Testing.Integration.PriorityTypeADT.cs
// Last Modified: 07/27/2014 5:10 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

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