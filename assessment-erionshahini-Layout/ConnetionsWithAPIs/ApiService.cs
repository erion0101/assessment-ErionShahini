
using System.Net.Http;

namespace assessment_erionshahini_Layout.ConnetionsWithAPIs
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task HttpDELETE(string url)
        {
            try
            {
                using var response = await _httpClient.DeleteAsync(url);
                Console.WriteLine($"DELETE RequestUri={response.RequestMessage?.RequestUri} Status={response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"DELETE Error Response: {errorMessage}");
                    throw new HttpRequestException($"Kërkesa dështoi me statusin {response.StatusCode}. Përgjigja e API-së: {errorMessage}");
                }

                // Log success for debugging
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseContent))
                {
                    Console.WriteLine($"DELETE Success Response: {responseContent}");
                }
            }
            catch (HttpRequestException)
            {
                // Re-throw HTTP exceptions
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gabim në kërkesën HTTP DELETE: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<T> HttpGET<T>(string url)
        {
            try
            {
                using var response = await _httpClient.GetAsync(url);
                Console.WriteLine($"RequestUri={response.RequestMessage.RequestUri} Status={response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    // Configure JSON options to handle PascalCase from API (ASP.NET Core default)
                    var options = new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = System.Text.Json.JsonSerializer.Deserialize<T>(jsonString, options);

                    if (result == null)
                        throw new InvalidOperationException($"Failed to deserialize response from {url}");

                    return result;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Kërkesa dështoi me statusin {response.StatusCode}. Përgjigja e API-së: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gabim në kërkesën HTTP GET: {ex.Message}");
                throw;
            }
        }

        public async Task<T> HttpPOST<T>(string url, object postData)
        {
            try
            {
                using var response = await _httpClient.PostAsJsonAsync(url, postData);
                if (response.IsSuccessStatusCode)
                {
                    var options = new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = System.Text.Json.JsonSerializer.Deserialize<T>(jsonString, options);

                    return result;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Kërkesa dështoi me statusin {response.StatusCode}. Përgjigja e API-së: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gabim në kërkesën HTTP POST: {ex.Message}");
                throw;
            }
        }

        public async Task<T> HttpPUT<T>(string url, object putData)
        {
            try
            {
                using var response = await _httpClient.PutAsJsonAsync(url, putData);
                Console.WriteLine($"RequestUri={response.RequestMessage.RequestUri} Status={response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    // Configure JSON options to handle PascalCase from API (ASP.NET Core default)
                    var options = new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = System.Text.Json.JsonSerializer.Deserialize<T>(jsonString, options);

                    if (result == null)
                        throw new InvalidOperationException($"Failed to deserialize response from {url}");

                    return result;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Kërkesa dështoi me statusin {response.StatusCode}. Përgjigja e API-së: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gabim në kërkesën HTTP PUT: {ex.Message}");
                throw;
            }
        }
    }
}
