using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using assessment_erionshahini_Layout.ConnetionsWithAPIs;
using assessment_erionshahini_Layout.Data;

namespace assessment_erionshahini_Layout.Service;

/// <summary>
/// Service for communicating with the API backend for authentication.
/// Access token is stored in memory; refresh token is in HttpOnly cookie.
/// </summary>
public class ApiAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly TokenStorageService _tokenStorage;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiAuthService(HttpClient httpClient, IConfiguration configuration, TokenStorageService tokenStorage)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _tokenStorage = tokenStorage;
    }

    private string ApiBaseUrl => _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7294";

    /// <summary>
    /// Login with email and password. Access token is stored in memory; refresh token is set in HttpOnly cookie by the API.
    /// </summary>
    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var loginUrl = $"{ApiBaseUrl}/api/auth/Login";
            var json = JsonSerializer.Serialize(new { email = request.Email, password = request.Password });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(loginUrl, content, cancellationToken);
            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var authResult = JsonSerializer.Deserialize<AuthResultDto>(responseJson, JsonOptions);
                if (authResult?.Success == true && !string.IsNullOrEmpty(authResult.AccessToken))
                {
                    _tokenStorage.SetAccessToken(authResult.AccessToken);
                    return new AuthResponse
                    {
                        IsSuccess = true,
                        AccessToken = authResult.AccessToken
                    };
                }
                return new AuthResponse
                {
                    IsSuccess = false,
                    Error = authResult?.Message ?? "Login failed."
                };
            }

            var errorMsg = responseJson;
            try
            {
                using var doc = JsonDocument.Parse(responseJson);
                if (doc.RootElement.TryGetProperty("message", out var messageProp))
                    errorMsg = messageProp.GetString() ?? responseJson;
                else if (doc.RootElement.TryGetProperty("error", out var errorProp))
                    errorMsg = errorProp.GetString() ?? responseJson;
            }
            catch { /* use raw responseJson */ }

            return new AuthResponse { IsSuccess = false, Error = errorMsg };
        }
        catch (Exception ex)
        {
            return new AuthResponse { IsSuccess = false, Error = $"Error: {ex.Message}" };
        }
    }

    /// <summary>
    /// Refresh access token using refresh token from HttpOnly cookie (sent automatically by browser when using WASM).
    /// With Blazor Server, the cookie is not sent unless the request is made from the browser or via BFF.
    /// </summary>
    public async Task<bool> TryRefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var refreshUrl = $"{ApiBaseUrl}/api/auth/refresh";
            var response = await _httpClient.PostAsync(refreshUrl, null, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonSerializer.Deserialize<AuthResultDto>(responseJson, JsonOptions);
                if (result?.Success == true && !string.IsNullOrEmpty(result.AccessToken))
                {
                    _tokenStorage.SetAccessToken(result.AccessToken);
                    return true;
                }
            }
        }
        catch
        {
            // Silent failure - token refresh failed
        }

        return false;
    }

    /// <summary>
    /// Logout: call API to clear refresh token cookie, then clear access token from memory.
    /// </summary>
    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var token = _tokenStorage.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{ApiBaseUrl}/api/auth/logout");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                await _httpClient.SendAsync(request, cancellationToken);
            }
        }
        catch
        {
            // Silent failure
        }
        finally
        {
            _tokenStorage.ClearAccessToken();
        }
    }
}
