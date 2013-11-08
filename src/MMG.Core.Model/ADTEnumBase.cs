// *************************************************
// MMG.Core.Model.ADTEnumBase.cs
// Last Modified: 11/08/2013 2:57 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;

namespace MMG.Core.Model
{
    public abstract class ADTEnumBase<TEnum> : ADT<string>
        where TEnum : struct
    {
        protected TEnum? _enumValue;
        protected string _stringValue;

        #region Constructors

        protected ADTEnumBase() { }

        public ADTEnumBase(TEnum pEnumValue)
        {
            _enumValue = pEnumValue;
            Value = convertEnumMemberToStringValue(pEnumValue);
        }

        public ADTEnumBase(string pValue)
        {
            Value = pValue;
        }

        #endregion

        public TEnum EnumValue
        {
            get { return _enumValue.GetValueOrDefault(); }
        }

        public override string Value
        {
            get { return _stringValue; }
            set { setStringValue(value); }
        }

        protected void setStringValue(string pStringValue)
        {
            var enumType = typeof(TEnum);
            if (enumType.IsEnumDefined(pStringValue))
            {
                //parse string matched to enum member name.
                var newEnumValue = (TEnum)Enum.Parse(typeof(TEnum), pStringValue, true);
                if (!_enumValue.Equals(newEnumValue))
                    _enumValue = newEnumValue;

                _enumValue = newEnumValue;
                try
                {
                    var enumStringValue = convertEnumMemberToStringValue(newEnumValue);
                    _stringValue = string.IsNullOrEmpty(enumStringValue) ? pStringValue : enumStringValue;
                }
                catch (Exception e)
                {
                    _stringValue = pStringValue;
                }
            }
            else
            {
                //parse string based on implementation by derived class, typically Description or Display attribute.
                var newEnumValue = convertStringToEnumValue(pStringValue);
                if (!_enumValue.Equals(newEnumValue))
                    _enumValue = newEnumValue;
                
                _stringValue = pStringValue;
            }
        }

        protected abstract string convertEnumMemberToStringValue(TEnum pEnumValue);

        protected abstract TEnum convertStringToEnumValue(string pValue);
    }
}