using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Helpers
{
    /// <summary>
    /// SQL操作工具类
    /// </summary>
    public class SqlHelper
    {
        /// <summary>
        /// SQL数据库备份（备份到程序默认备份文件夹）
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="backupFileName">备份文件名</param>
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

        /// <summary>
        /// SQL数据库还原（从程序默认备份文件夹还原）
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="backupFileName">备份文件名</param>
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

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dt">数据库表</param>
        public static void Insert(string connectionString, DataTable dt)
        {
            using (SqlBulkCopy sbc = new SqlBulkCopy(connectionString))
            {
                sbc.DestinationTableName = dt.TableName;
                sbc.BatchSize = 50000;
                foreach (DataColumn dc in dt.Columns)
                {
                    sbc.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                }
                sbc.WriteToServer(dt);
            }
        }
    }
}
