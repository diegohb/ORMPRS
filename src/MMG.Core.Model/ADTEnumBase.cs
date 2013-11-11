// *************************************************
// MMG.Core.Model.ADTEnumBase.cs
// Last Modified: 11/11/2013 10:34 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

namespace MMG.Core.Model
{
    public abstract class ADTEnumBase<TEnum, TUnderlyingValue> : ADT<TUnderlyingValue>
        where TEnum : struct
    {
        protected TEnum? _enumValue;
        protected TUnderlyingValue _underlyingValue;

        #region Constructors

        protected ADTEnumBase() {}

        public ADTEnumBase(TEnum pEnumValue)
        {
            _enumValue = pEnumValue;
            Value = convertEnumValueToUnderlyingValue(pEnumValue);
        }

        public ADTEnumBase(TUnderlyingValue pValue)
        {
            Value = pValue;
        }

        #endregion

        public TEnum EnumValue
        {
            get { return _enumValue.GetValueOrDefault(); }
        }

        public override TUnderlyingValue Value
        {
            get { return _underlyingValue; }
            set { setValue(value); }
        }

        protected virtual void setValue(TUnderlyingValue pStringValue)
        {
            var enumValue = convertUnderlyingValueToEnumValue(pStringValue);
            if (!_enumValue.Equals(enumValue))
                _enumValue = enumValue;

            _underlyingValue = convertEnumValueToUnderlyingValue(enumValue);
        }

        /// <summary>
        /// This method transforms the enum value to the underlying value.
        /// </summary>
        /// <param name="pEnumValue">The <typeparamref name="TEnum"/> value to convert to <typeparamref name="TUnderlyingValue"/>.</param>
        /// <returns>Returns the <typeparamref name="TUnderlyingValue"/> representation of the <typeparamref name="TEnum"/> value.</returns>
        protected abstract TUnderlyingValue convertEnumValueToUnderlyingValue(TEnum pEnumValue);

        /// <summary>
        /// This method transforms the underlying value to the enum value.
        /// </summary>
        /// <param name="pValue">The <typeparamref name="TUnderlyingValue"/> value.</param>
        /// <returns>Returns the <typeparamref name="TEnum"/> value of <typeparamref name="TUnderlyingValue"/>.</returns>
        protected abstract TEnum convertUnderlyingValueToEnumValue(TUnderlyingValue pValue);
    }
}