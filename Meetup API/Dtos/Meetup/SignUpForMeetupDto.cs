using System.ComponentModel.DataAnnotations;

namespace Meetup_API.Dtos.Meetup;

public class SignUpForMeetupDto
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public int MeetupId { get; set; }
}
