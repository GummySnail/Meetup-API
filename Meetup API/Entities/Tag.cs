namespace Meetup_API.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MeetupId { get; set; }
    public Meetup Meetup { get; set; }
}
