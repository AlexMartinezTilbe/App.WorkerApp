using System.Data.SqlClient;

namespace App.WorkerApp;

internal class Connection
{
    // Connection's configuration:
    private static string connectionString = ConnectionStrings.DbSalesPrice;
    private static Connection singleton;
    private static SqlConnection sqlConnection;

    public SqlConnection SqlConnetionFactory
    {
        get { return sqlConnection; }
    }

    private Connection() { }

    public static Connection Singleton
    {
        get
        {
            if (singleton == null)
                singleton = new Connection();

            sqlConnection = new SqlConnection(connectionString);
            return singleton;
        }
    }
}
