// *************************************************
// MMG.Core.Testing.Integration.CountryStringEnumADT.cs
// Last Modified: 11/12/2013 8:02 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System.ComponentModel;
using MMG.Common.Extensions;
using MMG.Core.Model;
using MMG.Core.Persistence;

namespace MMG.Core.Testing.Integration.Northwind
{
    public enum CountryEnum
    {
        [Description("United Kingdom")]
        UK,
        [Description("United States")]
        USA,
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


        protected override string convertEnumValueToUnderlyingValue(CountryEnum pEnumValue)
        {
            return pEnumValue.ToDescriptionString();
        }

        protected override CountryEnum convertUnderlyingValueToEnumValue(string pValue)
        {
            return pValue.ToEnum<CountryEnum>();
        }
    }
}