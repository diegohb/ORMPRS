// *************************************************
// MMG.Core.Model.ADTEnum.cs
// Last Modified: 11/08/2013 2:57 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.ComponentModel;

namespace MMG.Core.Model
{
    public class ADTEnum<TEnum> : ADT<string>
        where TEnum : struct
    {
        protected TEnum? _enumValue;
        protected string _stringValue;

        #region Constructors

        protected ADTEnum() { }

        public ADTEnum(TEnum pEnumValue)
        {
            _enumValue = pEnumValue;
            Value = convertEnumMemberToStringValue(pEnumValue);
        }

        public ADTEnum(string pValue)
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
            var enumValue = convertStringToEnumValue(pStringValue);
            if (!_enumValue.Equals(enumValue))
                _enumValue = enumValue;

            _stringValue = convertEnumMemberToStringValue(enumValue);
        }

        /// <summary>
        /// This method will use Enum.GetName to return the name of the enum member based on the value provided as a string.
        /// </summary>
        /// <param name="pEnumValue">The enum value to conver to a string.</param>
        /// <returns>Returns the string representation of the enum value.</returns>
        /// <remarks>This can be overriden to use reflection and return the value of Description or Display attributes.</remarks>
        protected virtual string convertEnumMemberToStringValue(TEnum pEnumValue)
        {
            return Enum.GetName(typeof (TEnum), pEnumValue);
        }

        /// <summary>
        /// This method will parse a string into an enum value. The default implementation use Enum.Parse which matches the name of one the enum members.
        /// </summary>
        /// <param name="pValue">The string value representing the value of the enum.</param>
        /// <returns>This method should return the enum value from the string provided.</returns>
        /// <remarks>This can be overriden to use reflection and match to the Description or Display attributes.</remarks>
        protected virtual TEnum convertStringToEnumValue(string pValue)
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnumDefined(pValue))
                throw new InvalidEnumArgumentException(string.Format("Invalid enum value '{0}' for enum type '{1}'.", pValue, enumType.Name));

            //parse string matched to enum member name.
            var newEnumValue = (TEnum)Enum.Parse(typeof(TEnum), pValue, true);
            if (!_enumValue.Equals(newEnumValue))
                _enumValue = newEnumValue;

            return newEnumValue;
        }
    }
}