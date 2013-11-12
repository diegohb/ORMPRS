// *************************************************
// MMG.Core.Model.NullableADT.cs
// Last Modified: 11/12/2013 7:13 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;

namespace MMG.Core.Model
{
    public class NullableADT<TUnderlyingType> : IConvertible, IEquatable<TUnderlyingType?>
        where TUnderlyingType : struct
    {
        public NullableADT() { }

        public NullableADT(TUnderlyingType pValue)
        {
            Value = pValue;
        }

        public virtual TUnderlyingType? Value { get; set; }

        public static implicit operator TUnderlyingType?(NullableADT<TUnderlyingType> pAbtractDataTypeObject)
        {
            return pAbtractDataTypeObject.Value;
        }

        public static implicit operator NullableADT<TUnderlyingType>(TUnderlyingType pValue)
        {
            return new NullableADT<TUnderlyingType>(pValue);
        }

        public bool Equals(TUnderlyingType? pOther)
        {
            return Value.Equals(pOther);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        #region IConvertible

        public virtual TypeCode GetTypeCode()
        {
            return Type.GetTypeCode(typeof(TUnderlyingType));
        }

        public virtual bool ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(Value, provider);
        }

        public virtual char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(Value, provider);
        }

        public virtual sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(Value, provider);
        }

        public virtual byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(Value, provider);
        }

        public virtual short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(Value, provider);
        }

        public virtual ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(Value, provider);
        }

        public virtual int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(Value, provider);
        }

        public virtual uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(Value, provider);
        }

        public virtual long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(Value, provider);
        }

        public virtual ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(Value, provider);
        }

        public virtual float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(Value, provider);
        }

        public virtual double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(Value, provider);
        }

        public virtual decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(Value, provider);
        }

        public virtual DateTime ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(Value, provider);
        }

        public virtual string ToString(IFormatProvider provider)
        {
            return string.Format(provider, "{0}", Value);
        }

        public virtual object ToType(Type pConversionType, IFormatProvider pProvider)
        {
            return Convert.ChangeType(Value, pConversionType);
        }

        #endregion
    }
}