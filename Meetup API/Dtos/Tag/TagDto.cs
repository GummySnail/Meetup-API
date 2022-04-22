using System.ComponentModel.DataAnnotations;

namespace Meetup_API.Dtos.Tag;

public class TagDto
{
    [Required]
    public string Name { get; set; }
}
