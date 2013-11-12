// *************************************************
// MMG.Core.Model.NullableByteEnumADT.cs
// Last Modified: 11/12/2013 7:25 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.ComponentModel;

namespace MMG.Core.Model
{
    public class NullableByteEnumADT<TEnum> : NullableADTEnumBase<TEnum, byte>
        where TEnum : struct
    {
        #region Constructors

        public NullableByteEnumADT() {}

        public NullableByteEnumADT(TEnum pValue) : base(pValue) {}

        /// <summary>
        /// Creates the ADT based on enum member name.
        /// </summary>
        /// <param name="pValue">The name of the member.</param>
        public NullableByteEnumADT(string pValue)
        {
            setValueFromEnumMemberName(pValue);
        }

        public NullableByteEnumADT(byte pValue) : base(pValue) {}

        #endregion

        #region Operators

        public static implicit operator NullableByteEnumADT<TEnum>(TEnum pValue)
        {
            return new NullableByteEnumADT<TEnum>(pValue);
        }

        public static implicit operator TEnum?(NullableByteEnumADT<TEnum> pEnumTypeADT)
        {
            return pEnumTypeADT.EnumValue;
        }

        public static implicit operator byte?(NullableByteEnumADT<TEnum> pEnumTypeADT)
        {
            return pEnumTypeADT.Value;
        }

        public static implicit operator NullableByteEnumADT<TEnum>(byte pValue)
        {
            return new NullableByteEnumADT<TEnum>(pValue);
        }

        #endregion

        protected override byte? convertEnumValueToUnderlyingValue(TEnum? pEnumValue)
        {
            return pEnumValue.HasValue ? Convert.ToByte(pEnumValue) : (byte?) null;
        }

        protected override TEnum? convertUnderlyingValueToEnumValue(byte? pValue)
        {
            return pValue.HasValue
                ? (TEnum) Enum.ToObject(typeof (TEnum), pValue)
                : (TEnum?) null;
        }

        /// <summary>
        /// This method will parse a string into an enum value. The default implementation use Enum.Parse which matches the name of one the enum members.
        /// </summary>
        /// <param name="pValue">The string value representing the value of the enum.</param>
        /// <returns>This method should return the enum value from the string provided.</returns>
        private void setValueFromEnumMemberName(string pValue)
        {
            var enumType = typeof (TEnum);
            if (!enumType.IsEnumDefined(pValue))
                throw new InvalidEnumArgumentException(string.Format("Invalid enum value '{0}' for enum type '{1}'.", pValue, enumType.Name));

            //parse string matched to enum member name.
            var newEnumValue = (TEnum) Enum.Parse(typeof (TEnum), pValue, true);
            _enumValue = newEnumValue;
            _underlyingValue = convertEnumValueToUnderlyingValue(_enumValue.Value);
        }
    }
}