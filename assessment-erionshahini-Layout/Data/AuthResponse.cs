namespace assessment_erionshahini_Layout.Data;

/// <summary>
/// Result returned to UI after LoginAsync.
/// </summary>
public class AuthResponse
{
    public bool IsSuccess { get; set; }
    public string? AccessToken { get; set; }
    public string? Error { get; set; }
}
