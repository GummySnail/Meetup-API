using System.ComponentModel.DataAnnotations;

namespace Meetup_API.Dtos.User;

public class UserLoginDto
{
    [Required]
    [StringLength(12, MinimumLength = 4)]
    public string Username { get; set; } = string.Empty;
    [Required]
    [StringLength(16, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}
