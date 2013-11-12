// *************************************************
// MMG.Core.Model.NullableADTEnumBase.cs
// Last Modified: 11/12/2013 7:19 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;

namespace MMG.Core.Model
{
    public abstract class NullableADTEnumBase<TEnum, TUnderlyingValue> : NullableADT<TUnderlyingValue>, IEquatable<TEnum>
        where TEnum : struct
        where TUnderlyingValue : struct
    {
        protected TEnum? _enumValue;
        protected TUnderlyingValue? _underlyingValue;

        #region Constructors

        public NullableADTEnumBase() {}

        public NullableADTEnumBase(TEnum pEnumValue)
        {
            _enumValue = pEnumValue;
            Value = convertEnumValueToUnderlyingValue(pEnumValue);
        }

        public NullableADTEnumBase(TUnderlyingValue pValue)
        {
            Value = pValue;
        }

        #endregion

        public TEnum? EnumValue
        {
            get { return _enumValue; }
        }

        public override TUnderlyingValue? Value
        {
            get { return _underlyingValue; }
            set { setValue(value); }
        }

        public bool Equals(TEnum pOtherEnumValue)
        {
            return EnumValue.Equals(pOtherEnumValue);
        }

        protected virtual void setValue(TUnderlyingValue? pValue)
        {
            var enumValue = convertUnderlyingValueToEnumValue(pValue);
            if (!_enumValue.Equals(enumValue))
                _enumValue = enumValue;

            _underlyingValue = convertEnumValueToUnderlyingValue(enumValue);
        }

        /// <summary>
        /// This method transforms the enum value to the underlying value.
        /// </summary>
        /// <param name="pEnumValue">The <typeparamref name="TEnum"/> value to convert to <typeparamref name="TUnderlyingValue"/>.</param>
        /// <returns>Returns the <typeparamref name="TUnderlyingValue"/> representation of the <typeparamref name="TEnum"/> value.</returns>
        protected abstract TUnderlyingValue? convertEnumValueToUnderlyingValue(TEnum? pEnumValue);

        /// <summary>
        /// This method transforms the underlying value to the enum value.
        /// </summary>
        /// <param name="pValue">The <typeparamref name="TUnderlyingValue"/> value.</param>
        /// <returns>Returns the <typeparamref name="TEnum"/> value of <typeparamref name="TUnderlyingValue"/>.</returns>
        protected abstract TEnum? convertUnderlyingValueToEnumValue(TUnderlyingValue? pValue);
    }
}