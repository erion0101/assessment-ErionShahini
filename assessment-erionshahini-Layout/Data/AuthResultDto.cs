namespace assessment_erionshahini_Layout.Data;

/// <summary>
/// API response for login/refresh (Success, Message, AccessToken). RefreshToken is in HttpOnly cookie.
/// </summary>
public class AuthResultDto
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
