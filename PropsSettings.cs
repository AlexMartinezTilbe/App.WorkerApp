
namespace App.WorkerApp
{
    static class PropsSettings
    {
        
    }
    public class ConnectionStrings
    {
        public static string DbSalesPrice { get; set; }
        public static string RedisUrl { get; set; }
    }

    public class WorkerProps
    {
        public static int Time { get; set; }
    }
}
