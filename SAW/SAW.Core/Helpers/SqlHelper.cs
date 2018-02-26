using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Helpers
{
    public class SqlHelper
    {
        public static void Backup(string connectionString, string dbName, string backupFileName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("USE master BACKUP DATABASE {0} TO DISK='{1}' WITH COMPRESSION, INIT", dbName, backupFileName);
                    cmd.CommandTimeout = 600;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Restore(string connectionString, string dbName, string backupFileName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("USE master RESTORE DATABASE {0} FROM DISK='{1}' WITH REPLACE", dbName, backupFileName);
                    cmd.CommandTimeout = 600;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
