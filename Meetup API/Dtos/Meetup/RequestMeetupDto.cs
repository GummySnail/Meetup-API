using Meetup_API.Dtos.Tag;
using System.ComponentModel.DataAnnotations;

namespace Meetup_API.Dtos.Meetup;

public class RequestMeetupDto
{
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int HomeNumber { get; set; }
    public DateTimeOffset StartMeetupDateTime { get; set; }
    public DateTimeOffset EndMeetupDateTime { get; set; }
    public ICollection<TagDto> Tags { get; set; }
}
