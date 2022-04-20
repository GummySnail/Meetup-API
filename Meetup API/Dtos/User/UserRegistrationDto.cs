using Meetup_API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Meetup_API.Dtos.User;

public class UserRegistrationDto
{
    [Required]
    [StringLength(12, MinimumLength = 4)]
    public string Username { get; set; } = string.Empty;
    [Required]
    [StringLength(16, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
    [Required]
    public Gender? Gender { get; set; }
    [Required]
    public Role Role { get; set; }
    [Required]
    public string Company { get; set; }
    [Required]
    public DateTimeOffset DateOfBirth { get; set; }
}
