// *************************************************
// MMG.Core.Testing.Integration.CustomerEFMappingAlt.cs
// Last Modified: 07/27/2014 5:37 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

namespace MMG.Core.Testing.Integration.EFPersistence.DBMapping
{
    public class CustomerEFMappingAlt : CustomerEFMapping
    {
        public override string ConnectionStringName
        {
            get { return Utility.NorthwindAltDBConnectionName; }
        }
    }
}