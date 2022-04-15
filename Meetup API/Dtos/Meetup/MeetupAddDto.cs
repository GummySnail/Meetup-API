using System.ComponentModel.DataAnnotations;
namespace Meetup_API.Dtos.Meetup;

public class MeetupAddDto
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int HomeNumber { get; set; }
    public DateTimeOffset StartMeetupDateTime { get; set; }
    public DateTimeOffset EndMeetupDateTime { get; set; }
    [Required]
    public string OwnerName { get; set; } // OwnerId
    [Required]
    public string Tag { get; set; }
}
