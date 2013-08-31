// *************************************************
// LGCYDAPI.Domain.Persistence.PersistenceException.cs
// Last Modified: 08/30/2013 7:09 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;

namespace MMG.Core.Persistence.Exceptions
{
    [Serializable]
    public class PersistenceException : ApplicationException
    {
        public PersistenceException() {}

        public PersistenceException(string pMessage) 
            : base(pMessage) {}

        public PersistenceException(string pMessage, Exception pInnerException) 
            : base(pMessage, pInnerException) {}
    }
}