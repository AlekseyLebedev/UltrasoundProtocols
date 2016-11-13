using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolTemplateLib
{
    public class DataBaseSettings
    {
        public DataBaseSettings()
        {

        }

        public string GetConnectionString()
        {
            if (ServerName == TestLocalServerName)
            {
                return String.Format(TestLocalConnection, Login);
            }
            else
            {
                return String.Format(ConnectionPropertiesString, ServerName, Login, Password);
            }

        }

        public string Login { get; set; }

        public string Password { get; set; }

        public string ServerName { get; set; }

        private const string TestLocalServerName =
            "testLocalDB";
        private const string TestLocalConnection =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={0};Integrated Security=True;Connect Timeout=30";
        private const string ConnectionPropertiesString =
            "Data Source='{0}'; Integrated Security=FALSE; Initial Catalog=UltraSoundProtocolsDB; User='{1}'; Password='{2}'";

    }
}
