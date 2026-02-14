using assessment_erionshahini_Layout.Service;
using System.Net.Http.Headers;

namespace assessment_erionshahini_Layout.ConnetionsWithAPIs
{
    public class JwtTokenHandler : DelegatingHandler
    {
        private readonly TokenStorageService _tokenStorage;

        public JwtTokenHandler(TokenStorageService tokenStorage)
        {
            _tokenStorage = tokenStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Get the access token from storage
            var token = _tokenStorage.GetAccessToken();

            // Add Authorization header if token exists
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
