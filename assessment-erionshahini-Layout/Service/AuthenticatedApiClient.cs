using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using assessment_erionshahini_Layout.Data;
using Microsoft.AspNetCore.Components.Forms;

namespace assessment_erionshahini_Layout.Service;

/// <summary>
/// Makes API calls with Bearer token from the current scope (TokenStorageService).
/// Use this in Blazor pages so the token is always from the current circuit.
/// </summary>
public class AuthenticatedApiClient
{
    private readonly IHttpClientFactory _factory;
    private readonly TokenStorageService _tokenStorage;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public AuthenticatedApiClient(IHttpClientFactory factory, TokenStorageService tokenStorage)
    {
        _factory = factory;
        _tokenStorage = tokenStorage;
    }

    public async Task<List<VideoItem>?> GetMyVideosAsync(CancellationToken cancellationToken = default)
    {
        var token = _tokenStorage.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Not signed in. Please log in again.");

        var client = _factory.CreateClient("VideoLabApi");
        using var request = new HttpRequestMessage(HttpMethod.Get, "Videos/GetMyVideos");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Request failed with status {response.StatusCode}. {body}");
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<List<VideoItem>>(json, JsonOptions);
    }

    public async Task<VideoItem?> GetVideoByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var token = _tokenStorage.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Not signed in. Please log in again.");

        var client = _factory.CreateClient("VideoLabApi");
        using var request = new HttpRequestMessage(HttpMethod.Get, $"Videos/GetById/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<VideoItem>(json, JsonOptions);
    }

    public async Task<List<AnnotationItem>?> GetAnnotationsByVideoAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        var token = _tokenStorage.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Not signed in. Please log in again.");
        var client = _factory.CreateClient("VideoLabApi");
        using var request = new HttpRequestMessage(HttpMethod.Get, $"Annotations/GetByVideo/{videoId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<List<AnnotationItem>>(json, JsonOptions);
    }

    public async Task<AnnotationItem?> CreateAnnotationAsync(CreateAnnotationRequest req, CancellationToken cancellationToken = default)
    {
        var token = _tokenStorage.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Not signed in. Please log in again.");
        var client = _factory.CreateClient("VideoLabApi");
        using var request = new HttpRequestMessage(HttpMethod.Post, "Annotations/Create");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(req);
        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Annotation failed: {response.StatusCode}. {body}");
        }
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<AnnotationItem>(json, JsonOptions);
    }

    public async Task DeleteAnnotationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var token = _tokenStorage.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Not signed in. Please log in again.");
        var client = _factory.CreateClient("VideoLabApi");
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"Annotations/Delete/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Delete annotation failed: {response.StatusCode}. {body}");
        }
    }

    public async Task<List<BookmarkItem>?> GetBookmarksByVideoAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        var token = _tokenStorage.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Not signed in. Please log in again.");
        var client = _factory.CreateClient("VideoLabApi");
        using var request = new HttpRequestMessage(HttpMethod.Get, $"Bookmarks/GetByVideo/{videoId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<List<BookmarkItem>>(json, JsonOptions);
    }

    public async Task<BookmarkItem?> CreateBookmarkAsync(CreateBookmarkRequest req, CancellationToken cancellationToken = default)
    {
        var token = _tokenStorage.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Not signed in. Please log in again.");
        var client = _factory.CreateClient("VideoLabApi");
        using var request = new HttpRequestMessage(HttpMethod.Post, "Bookmarks/Create");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(req);
        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Bookmark failed: {response.StatusCode}. {body}");
        }
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<BookmarkItem>(json, JsonOptions);
    }

    public async Task DeleteBookmarkAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var token = _tokenStorage.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Not signed in. Please log in again.");
        var client = _factory.CreateClient("VideoLabApi");
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"Bookmarks/Delete/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Delete bookmark failed: {response.StatusCode}. {body}");
        }
    }

    /// <summary>Upload a video (title + file). Max file size 500 MB.</summary>
    public async Task<VideoItem?> UploadVideoAsync(string title, IBrowserFile file, CancellationToken cancellationToken = default)
    {
        var token = _tokenStorage.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Not signed in. Please log in again.");

        const long maxAllowedSize = 500_000_000; // 500 MB, same as API
        await using var sourceStream = file.OpenReadStream(maxAllowedSize, cancellationToken);
        var memoryStream = new MemoryStream();
        await sourceStream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        var client = _factory.CreateClient("VideoLabApi");
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(title.Trim()), "Title");
        var streamContent = new StreamContent(memoryStream);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType ?? "video/mp4");
        content.Add(streamContent, "File", file.Name);

        using var request = new HttpRequestMessage(HttpMethod.Post, "Videos/Upload");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = content;

        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Upload failed: {response.StatusCode}. {body}");
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<VideoItem>(json, JsonOptions);
    }

    public async Task<List<VideoItem>?> GetAdminVideosAsync(CancellationToken cancellationToken = default)
    {
        var token = _tokenStorage.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Not signed in. Please log in again.");

        var client = _factory.CreateClient("VideoLabApi");
        using var request = new HttpRequestMessage(HttpMethod.Get, "Admin/GetVideos");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Admin videos failed: {response.StatusCode}. {body}");
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<List<VideoItem>>(json, JsonOptions);
    }

    public async Task<List<AnnotationItem>?> GetAdminAnnotationsAsync(CancellationToken cancellationToken = default)
    {
        var token = _tokenStorage.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Not signed in. Please log in again.");

        var client = _factory.CreateClient("VideoLabApi");
        using var request = new HttpRequestMessage(HttpMethod.Get, "Admin/GetAnnotations");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Admin annotations failed: {response.StatusCode}. {body}");
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<List<AnnotationItem>>(json, JsonOptions);
    }

    public async Task<List<BookmarkItem>?> GetAdminBookmarksAsync(CancellationToken cancellationToken = default)
    {
        var token = _tokenStorage.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Not signed in. Please log in again.");

        var client = _factory.CreateClient("VideoLabApi");
        using var request = new HttpRequestMessage(HttpMethod.Get, "Admin/GetBookmarks");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Admin bookmarks failed: {response.StatusCode}. {body}");
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<List<BookmarkItem>>(json, JsonOptions);
    }
}
