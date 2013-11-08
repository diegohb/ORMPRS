// *************************************************
// MMG.Core.Model.ADTBase.cs
// Last Modified: 11/07/2013 7:25 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Globalization;

namespace MMG.Core.Model
{
    /// <summary>
    /// Represents an abstract data type that wraps a primitive type.
    /// </summary>
    /// <typeparam name="TUnderlyingType">The type of the underlying value.</typeparam>
    /// <remarks>see: 
    /// http://programmers.stackexchange.com/questions/148747/abstract-data-type-and-data-structure or
    /// http://en.wikipedia.org/wiki/Abstract_data_type
    /// </remarks>
    public abstract class ADTBase<TUnderlyingType> : IConvertible, IComparable<TUnderlyingType>
        where TUnderlyingType : struct
    {
        public ADTBase() {}

        public ADTBase(TUnderlyingType pValue)
        {
            Value = pValue;
        }

        public abstract TUnderlyingType Value { get; set; }

        public static implicit operator string(ADTBase<TUnderlyingType> pAbtractDataTypeObject)
        {
            return pAbtractDataTypeObject.ToString();
        }

        public virtual int CompareTo(TUnderlyingType pOther)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return ToString(CultureInfo.InvariantCulture);
        }

        #region IConvertible

        public virtual TypeCode GetTypeCode()
        {
            return Type.GetTypeCode(Value.GetType());
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
            return Value.ToString();
        }

        public virtual object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}