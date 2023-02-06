namespace App.WorkerApp.Models
{
    public class Calendar
    {
        public int Id { get; set; }
        public string CalendarId { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public int Day { get; set; }
        public string DayName { get; set; }
        public int Route { get; set; }
        public int ExtraRoute { get; set; }
    }
}
