namespace Meetup_API.Dtos.Meetup;

public class MeetupDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int HomeNumber { get; set; }
    public DateTimeOffset StartMeetupDateTime { get; set; }
    public DateTimeOffset EndMeetupDateTime { get; set; }
    public string OwnerName { get; set; } // OwnerId
    public string Tag { get; set; } // Tags Collection<>
}
