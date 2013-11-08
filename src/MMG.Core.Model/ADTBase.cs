// *************************************************
// MMG.Core.Model.ADTBase.cs
// Last Modified: 11/07/2013 7:25 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

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
    public abstract class ADTBase<TUnderlyingType>
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

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}