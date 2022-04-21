using Meetup_API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Meetup_API.Dtos.User;

public class UserDto
{ 
    [Required]
    public string Username { get; set; }
    [Required]
    public Gender Gender { get; set; }
    [Required]
    public string Company { get; set; }
    [Required]
    public DateTimeOffset DateOfBirth { get; set; }
    public string AccessToken { get; set; }
}
