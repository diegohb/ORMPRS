// *************************************************
// MMG.Core.Model.ADTStringEnum.cs
// Last Modified: 11/11/2013 10:35 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.ComponentModel;

namespace MMG.Core.Model
{
    public class ADTStringEnum<TEnum> : ADTEnumBase<TEnum, string>
        where TEnum : struct
    {
        #region Constructors

        public ADTStringEnum() {}

        public ADTStringEnum(TEnum pEnumValue)
            : base(pEnumValue) {}

        public ADTStringEnum(string pValue)
            : base(pValue) {}

        #endregion

        /// <summary>
        /// This method will use Enum.GetName to return the name of the enum member based on the value provided as a string.
        /// </summary>
        /// <param name="pEnumValue">The enum value to conver to a string.</param>
        /// <returns>Returns the string representation of the enum value.</returns>
        /// <remarks>This can be overriden to use reflection and return the value of Description or Display attributes.</remarks>
        protected override string convertEnumValueToUnderlyingValue(TEnum pEnumValue)
        {
            return Enum.GetName(typeof (TEnum), pEnumValue);
        }

        /// <summary>
        /// This method will parse a string into an enum value. The default implementation use Enum.Parse which matches the name of one the enum members.
        /// </summary>
        /// <param name="pValue">The string value representing the value of the enum.</param>
        /// <returns>This method should return the enum value from the string provided.</returns>
        /// <remarks>This can be overriden to use reflection and match to the Description or Display attributes.</remarks>
        protected override TEnum convertUnderlyingValueToEnumValue(string pValue)
        {
            var enumType = typeof (TEnum);
            if (!enumType.IsEnumDefined(pValue))
                throw new InvalidEnumArgumentException(string.Format("Invalid enum value '{0}' for enum type '{1}'.", pValue, enumType.Name));

            //parse string matched to enum member name.
            return (TEnum) Enum.Parse(typeof (TEnum), pValue, true);
        }
    }
}