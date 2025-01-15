namespace SQL_API.Models
{
    public class ShiftCreateModel
    {
        public string Name { get; set; }
        public List<ShiftDetailCreateModel> Details { get; set; }
        public int CreatedBy {  get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class ShiftDetailCreateModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int TargetDay { get; set; }
    }


    public sealed record ShiftItem(int Type, int Group, DateTime StartDate, DateTime EndDate) { }
}
