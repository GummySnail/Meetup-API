using Meetup_API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Meetup_API.Dtos.Meetup;

public class MeetupDto
{
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int HomeNumber { get; set; }
    public int OwnerId { get; set; }
    public DateTimeOffset StartMeetupDateTime { get; set; }
    public DateTimeOffset EndMeetupDateTime { get; set; }
    public ICollection<Tag> Tags { get; set; }
}
