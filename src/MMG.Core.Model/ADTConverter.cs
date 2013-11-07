// *************************************************
// MMG.Core.Model.ADTConverter.cs
// Last Modified: 11/07/2013 2:51 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.ComponentModel;
using System.Globalization;

namespace MMG.Core.Model
{
    public class ADTConverter<TUnderlyingType> : TypeConverter
        where TUnderlyingType : class
    {
        public override bool CanConvertFrom(ITypeDescriptorContext pContext, Type pSourceType)
        {
            if (pSourceType is TUnderlyingType)
                return true;

            return base.CanConvertFrom(pContext, pSourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext pContext, Type pDestinationType)
        {
            if (pDestinationType is TUnderlyingType)
                return true;

            return base.CanConvertTo(pContext, pDestinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext pContext, CultureInfo pCulture, object pValue)
        {
            return base.ConvertFrom(pContext, pCulture, pValue);
        }
    }
}