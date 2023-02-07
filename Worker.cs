using StackExchange.Redis;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using App.WorkerApp.Models;
using Microsoft.Extensions.Options;
using QV_Altipal_TradeGoBackend.DTO;

namespace App.WorkerApp;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServer _server;
    private static IDatabase _cache = null;


    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _cache = RedisConnectorHelper.Connection.GetDatabase();
        _server = RedisConnectorHelper.Connection.GetServer(_cache.IdentifyEndpoint());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            doWorkZonesSite(stoppingToken);
        });
    }

    private async void doWorkZonesSite(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(15));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            string sKey = $"LoggedZones";

            var loggedZones = _cache.StringGet(sKey);
            if (!loggedZones.IsNullOrEmpty)
            {
                var dataInList = JsonConvert.DeserializeObject<List<SalesZoneLogged>>(loggedZones);
                _logger.LogInformation("Zonas activas: {0}", dataInList.Count);
                try
                {
                    dataInList.ForEach(z =>
                    {
                        Task.Run(() => GetPricesByZoneRoute(z.SalesZone, z.Route));
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Worker err at: {time} -> ERROR: {1}", DateTimeOffset.Now, ex.Message);
                }
            }
            _logger.LogInformation("Worker end at: {time} -> INFO: {1}", DateTimeOffset.Now, "");
        }
    }

    private async Task GetPricesByZoneRoute(string sZona, int iRuta)
    {
        using (var con = Connection.Singleton.SqlConnetionFactory)
        {
            SqlCommand sqlCommand = new SqlCommand("[dbo].[SP_APISalesPriceFromSalesZoneRouteNumWOutCust]", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 3600;
            sqlCommand.Parameters.AddWithValue("@SalesZoneId", sZona);
            sqlCommand.Parameters.AddWithValue("@RouteNum", iRuta);
            var table = new DataTable();
            new SqlDataAdapter(sqlCommand).Fill(table);
            if (table.Rows.Count > 0)
            {
                string sKeyList = $"{sZona}Z{iRuta}R-SalesPrice";
                var jsonResult = JsonConvert.SerializeObject(table);
                await _cache.StringSetAsync(sKeyList, jsonResult.ToString());
            }
            con.Close();
        }
    }
    private static Calendar GetRoute()
    {
        DateTime date = DateTime.Now;
        string day = date.Day.ToString().PadLeft(2, '0');
        string month = date.Month.ToString().PadLeft(2, '0');
        string year = date.Year.ToString();
        string currentDate = $"{year}{month}{day}";
        var dateRoute = JsonConvert.DeserializeObject<Calendar>(_cache.StringGet($"Calendar-{currentDate}").ToString());
        return dateRoute;
    }
}
