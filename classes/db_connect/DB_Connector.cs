using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace TestApp_Solar_TaskManager.classes.db_connect
{
    class DB_Connector
    {
        public static SqlConnection GetDBConnection(string datasource, string database, string username, string password)
        {
            return new SqlConnection(@"Data Source=" + datasource + ";Initial Catalog="
                       + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password); 
        }
    }
}
