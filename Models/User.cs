namespace DailyPlannerServices.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public List<ToDoItem> ToDoItems { get; set; }

    public User(int id, string firstName, string lastName, string emailAddress)
    {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.EmailAddress = emailAddress;
    }
}
