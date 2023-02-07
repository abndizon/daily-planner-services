namespace DailyPlannerServices.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int UserId { get; set; }
        public Category Category { get; set; }
        public User User { get; set; }

        public ToDoItem()
        {
        }

        public ToDoItem(int id, string name, int categoryId, DateTime date, string starTime, string endTime, int userId)
        {
            this.Id = id;
            this.Name = name;
            this.CategoryId = categoryId;
            this.Date = date;
            this.StartTime = starTime;
            this.EndTime = endTime;
            this.UserId = userId;
        }
    }
}