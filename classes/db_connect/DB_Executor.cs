using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace TestApp_Solar_TaskManager.classes.db_connect
{
    class DB_Executor
    {
        SqlConnection conn;

        public DB_Executor()
        {
            this.conn = DB_Connector.GetDBConnection(DB_Data.PATH, DB_Data.DB, DB_Data.USER, DB_Data.PASS);
        }

        public List<string[]> executeQuery(string query, bool outputNeeded)
        {
            List<string[]> result = new List<string[]>();
            conn.Open();
            try
            {

                SqlCommand cmd = new SqlCommand(query, conn);

                if (outputNeeded)
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                        if (reader.HasRows)
                            while (reader.Read())
                                result.Add(new string[] { reader.GetInt32(0).ToString(), reader.GetString(1),
                                    reader.GetDateTime(2).ToString().Substring(0,10), reader.GetBoolean(3).ToString() });
                }
                else
                    cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        public List<string> getTables()
        {
            List<string> result = new List<string>();
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Distinct TABLE_NAME FROM information_schema.TABLES", conn);
                using (DbDataReader reader = cmd.ExecuteReader())
                    if (reader.HasRows)
                        while (reader.Read())
                                result.Add(reader.GetString(0));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
    }
}
