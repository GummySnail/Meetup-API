namespace Meetup_API.Entities;

public class UserMeetup
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int MeetupId { get; set; }
    public Meetup Meetup { get; set; }
    public DateTimeOffset DateOfRegistration { get; set; }
}
