// *************************************************
// MMG.Core.Model.ByteEnumADT.cs
// Last Modified: 11/11/2013 10:47 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;

namespace MMG.Core.Model
{
    public class ByteEnumADT<TEnum> : ADTEnumBase<TEnum, byte>
        where TEnum : struct
    {
        #region Constructors

        public ByteEnumADT(TEnum pValue) : base(pValue) {}

        public ByteEnumADT(byte pValue) : base(pValue) {}

        #endregion

        #region Operators

        public static implicit operator ByteEnumADT<TEnum>(TEnum pValue)
        {
            return new ByteEnumADT<TEnum>(pValue);
        }

        public static implicit operator TEnum(ByteEnumADT<TEnum> pEnumTypeADT)
        {
            return pEnumTypeADT.EnumValue;
        }

        public static implicit operator byte(ByteEnumADT<TEnum> pEnumTypeADT)
        {
            return pEnumTypeADT.Value;
        }

        public static implicit operator ByteEnumADT<TEnum>(byte pValue)
        {
            return new ByteEnumADT<TEnum>(pValue);
        }

        #endregion

        protected override byte convertEnumValueToUnderlyingValue(TEnum pEnumValue)
        {
            return Convert.ToByte(pEnumValue);
        }

        protected override TEnum convertUnderlyingValueToEnumValue(byte pValue)
        {
            return (TEnum) Enum.ToObject(typeof (TEnum), pValue);
        }
    }
}