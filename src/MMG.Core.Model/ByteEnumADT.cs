// *************************************************
// MMG.Core.Model.ByteEnumADT.cs
// Last Modified: 11/11/2013 10:47 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.ComponentModel;

namespace MMG.Core.Model
{
    public class ByteEnumADT<TEnum> : ADTEnumBase<TEnum, byte>
        where TEnum : struct
    {
        #region Constructors

        public ByteEnumADT(TEnum pValue) : base(pValue) {}

        /// <summary>
        /// Creates the ADT based on enum member name.
        /// </summary>
        /// <param name="pValue">The name of the member.</param>
        public ByteEnumADT(string pValue)
        {
            setValueFromEnumMemberName(pValue);
        }

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

        /// <summary>
        /// This method will parse a string into an enum value. The default implementation use Enum.Parse which matches the name of one the enum members.
        /// </summary>
        /// <param name="pValue">The string value representing the value of the enum.</param>
        /// <returns>This method should return the enum value from the string provided.</returns>
        private void setValueFromEnumMemberName(string pValue)
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnumDefined(pValue))
                throw new InvalidEnumArgumentException(string.Format("Invalid enum value '{0}' for enum type '{1}'.", pValue, enumType.Name));

            //parse string matched to enum member name.
            var newEnumValue = (TEnum)Enum.Parse(typeof(TEnum), pValue, true);
            _enumValue = newEnumValue;
            _underlyingValue = convertEnumValueToUnderlyingValue(_enumValue.Value);
        }
    }
}