namespace Services;

/// <summary>Validates short-lived stream tokens issued by the Blazor proxy (same secret).</summary>
public interface IStreamTokenValidator
{
    /// <summary>Returns true if the token is valid and allows streaming the given video id.</summary>
    bool TryValidate(string? bearerToken, Guid videoId, out string? error);
}
