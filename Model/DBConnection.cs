using System.Configuration;
using System.Data.SqlClient;

namespace NotesWPF.Model
{
    public class DBConnection
    {
        private static SqlConnection? sqlConnection;
        private static readonly object _lock = new object();

        private DBConnection() { }

        public static SqlConnection Connection()
        {
            if(sqlConnection == null)
            {
                lock (_lock)
                {
                    if(sqlConnection == null)
                    {
                        sqlConnection = new SqlConnection();
                    }
                }
            }

            if (sqlConnection.State != System.Data.ConnectionState.Open)
            {
                sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            }

            return sqlConnection;
        }
    }
}
