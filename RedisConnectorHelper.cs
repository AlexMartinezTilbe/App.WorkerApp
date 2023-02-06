using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;

namespace App.WorkerApp
{
    public class RedisConnectorHelper
    {

        static RedisConnectorHelper()
        {
            
            RedisConnectorHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(ConnectionStrings.RedisUrl);
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
