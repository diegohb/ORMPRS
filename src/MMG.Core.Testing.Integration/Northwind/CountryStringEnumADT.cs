// *************************************************
// MMG.Core.Testing.Integration.CountryStringEnumADT.cs
// Last Modified: 11/12/2013 8:02 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.ComponentModel;
using MMG.Common.Extensions;
using MMG.Core.Model;
using MMG.Core.Persistence;

namespace MMG.Core.Testing.Integration.Northwind
{
    public enum CountryEnum
    {
        [Description("UK")]
        UnitedKingdom,
        [Description("USA")]
        UnitedStates,
        Australia,
        Brazil,
        Canada,
        Denmark,
        Finland,
        France,
        Germany,
        Italy,
        Japan,
        Netherlands,
        Norway,
        Singapore,
        Spain,
        Sweden
    }

    public class CountryStringEnumADT : ADTStringEnum<CountryEnum>, IDbEntity
    {
        #region Constructors

        public CountryStringEnumADT() {}

        public CountryStringEnumADT(CountryEnum pValue) : base(pValue) { }

        public CountryStringEnumADT(string pValue) : base(pValue) { }

        #endregion

        public static implicit operator CountryStringEnumADT(CountryEnum pValue)
        {
            return new CountryStringEnumADT(pValue);
        }

        public static implicit operator CountryStringEnumADT(string pValue)
        {
            return new CountryStringEnumADT(pValue);
        }

        protected override string convertEnumValueToUnderlyingValue(CountryEnum pEnumValue)
        {
            try
            {
                return pEnumValue.ToDescriptionString();
            }
            catch (Exception e)
            {
                return base.convertEnumValueToUnderlyingValue(pEnumValue);
            }
        }

        protected override CountryEnum convertUnderlyingValueToEnumValue(string pValue)
        {
            try
            {
                return pValue.ToEnum<CountryEnum>();
            }
            catch (Exception e)
            {
                return base.convertUnderlyingValueToEnumValue(pValue);
            }
        }
    }
}