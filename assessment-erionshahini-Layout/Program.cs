using assessment_erionshahini_Layout.ConnetionsWithAPIs;
using assessment_erionshahini_Layout.Data;
using assessment_erionshahini_Layout.Service;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
