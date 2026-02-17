using System.ComponentModel.DataAnnotations;

namespace Services.DTOs;

public class AdminCreateUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string RoleName { get; set; } = "User";
}
