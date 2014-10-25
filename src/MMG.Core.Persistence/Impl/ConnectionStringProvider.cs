// *************************************************
// MMG.Core.Persistence.ConnectionStringProvider.cs
// Last Modified: 10/25/2014 6:46 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

namespace MMG.Core.Persistence.Impl
{
    public class ConnectionStringProvider : IProvideConnectionString
    {
        private readonly string _connectionString;

        public ConnectionStringProvider(string pConnectionString)
        {
            _connectionString = pConnectionString;
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }
    }
}