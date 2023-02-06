
namespace App.WorkerApp.Models
{
    public class SalesZone
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string AdvisorId { get; set; }
        public string DateRetreat { get; set; }
        public string Description { get; set; }
        public string SalesGroupId { get; set; }
        //[Key]
        public string SalesZoneId { get; set; }
        public string Transfer { get; set; }
        public List<Sites> Site { get; set; }
    }
}
