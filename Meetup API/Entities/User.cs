namespace Meetup_API.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public Gender? Gender { get; set; }
    public Role Role { get; set; } = Role.User;
    public string Company { get; set; }
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
