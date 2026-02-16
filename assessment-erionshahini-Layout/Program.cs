using assessment_erionshahini_Layout.ConnetionsWithAPIs;
using assessment_erionshahini_Layout.Data;
using assessment_erionshahini_Layout.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        // Allows video uploads up to ~500 MB (otherwise SignalR limits to ~32 KB and upload fails)
        options.MaximumReceiveMessageSize = 500 * 1024 * 1024; // 500 MB
    });
builder.Services.AddSingleton<WeatherForecastService>();

// Authentication: access token in memory, refresh token in HttpOnly cookie
builder.Services.AddScoped<TokenStorageService>();
builder.Services.AddScoped<ApiAuthService>();
builder.Services.AddHttpClient<ApiAuthService>((sp, client) =>
{
    var baseUrl = sp.GetRequiredService<IConfiguration>()["ApiSettings:BaseUrl"] ?? "https://localhost:7294";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());

// API client with Bearer token (for Videos, Annotations, etc.)
builder.Services.AddScoped<JwtTokenHandler>();
builder.Services.AddHttpClient<IApiService, ApiService>((sp, client) =>
{
    var baseUrl = sp.GetRequiredService<IConfiguration>()["ApiSettings:BaseUrl"] ?? "https://localhost:7294";
    client.BaseAddress = new Uri($"{baseUrl.TrimEnd('/')}/api/");
}).AddHttpMessageHandler<JwtTokenHandler>();

// Named client for per-request token (Blazor Server scope fix: token from current circuit)
builder.Services.AddHttpClient("VideoLabApi", (sp, client) =>
{
    var baseUrl = sp.GetRequiredService<IConfiguration>()["ApiSettings:BaseUrl"] ?? "https://localhost:7294";
    client.BaseAddress = new Uri($"{baseUrl.TrimEnd('/')}/api/");
});
builder.Services.AddScoped<AuthenticatedApiClient>();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IStreamTokenService, StreamTokenService>();
builder.Services.AddHttpClient("StreamProxyApi", (sp, client) =>
{
    var baseUrl = sp.GetRequiredService<IConfiguration>()["ApiSettings:BaseUrl"] ?? "https://localhost:7294";
    client.BaseAddress = new Uri(baseUrl.TrimEnd('/'));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Stream proxy: same-origin video URL so no CORS; requires short-lived token from Watch page.
// Important: pass Range requests + partial-content headers so browser seek works.
app.MapGet("/stream/{id:guid}", async (
    Guid id,
    [FromQuery] string? t,
    [FromQuery] string? st,
    HttpContext httpContext,
    IMemoryCache cache,
    IHttpClientFactory factory,
    CancellationToken ct) =>
{
    if (string.IsNullOrEmpty(t) || !cache.TryGetValue($"stream:{id}:{t}", out _))
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return;
    }

    var client = factory.CreateClient("StreamProxyApi");
    var streamUrl = $"/api/Videos/Stream/{id}";
    using var upstreamRequest = new HttpRequestMessage(HttpMethod.Get, streamUrl);

    if (!string.IsNullOrEmpty(st))
        upstreamRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", st);
    if (httpContext.Request.Headers.TryGetValue("Range", out var rangeValue))
        upstreamRequest.Headers.TryAddWithoutValidation("Range", rangeValue.ToString());

    using var upstreamResponse = await client.SendAsync(upstreamRequest, HttpCompletionOption.ResponseHeadersRead, ct);
    httpContext.Response.StatusCode = (int)upstreamResponse.StatusCode;

    if (upstreamResponse.Headers.TryGetValues("Accept-Ranges", out var acceptRanges))
        httpContext.Response.Headers["Accept-Ranges"] = acceptRanges.ToArray();
    if (upstreamResponse.Content.Headers.TryGetValues("Content-Range", out var contentRange))
        httpContext.Response.Headers["Content-Range"] = contentRange.ToArray();

    if (upstreamResponse.Content.Headers.ContentType is not null)
        httpContext.Response.ContentType = upstreamResponse.Content.Headers.ContentType.ToString();
    if (upstreamResponse.Content.Headers.ContentLength.HasValue)
        httpContext.Response.ContentLength = upstreamResponse.Content.Headers.ContentLength.Value;

    await using var upstreamStream = await upstreamResponse.Content.ReadAsStreamAsync(ct);
    await upstreamStream.CopyToAsync(httpContext.Response.Body, ct);
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
