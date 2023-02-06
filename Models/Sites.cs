
namespace App.WorkerApp.Models
{
    public class Sites
    {
        public int Id { get; set; }
        public string InventLocationId { get; set; }
        public string WMSLocationId { get; set; }
        public bool PreSale { get; set; }
        public bool AutoSale { get; set; }
        public bool Consignment { get; set; }
        public bool Direct { get; set; }
        public bool Focused { get; set; }
        public string AutoSalesQuotaLimit { get; set; }
    }
}
