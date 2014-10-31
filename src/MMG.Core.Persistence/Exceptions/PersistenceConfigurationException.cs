// *************************************************
// MMG.Core.Persistence.PersistenceConfigurationException.cs
// Last Modified: 10/27/2014 7:28 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

namespace MMG.Core.Persistence.Exceptions
{
    using System;

    [Serializable]
    public class PersistenceConfigurationException : PersistenceException
    {
        public PersistenceConfigurationException(string pMessage) : base(pMessage) {}

        public PersistenceConfigurationException(string pMessage, Exception pInnerException) : base(pMessage, pInnerException) {}
    }
}