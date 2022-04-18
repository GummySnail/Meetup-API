namespace Meetup_API.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Gender? Gender { get; set; }
    public Role Role { get; set; }
    public string Company { get; set; }
    public string Password { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public ICollection<UserMeetup> UserMeetups { get; set; }
}

public enum Role
{ 
    Organizer = 1,
    User
}
public enum Gender
{
    Male = 1,
    Female
}
