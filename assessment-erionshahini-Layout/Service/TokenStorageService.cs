namespace assessment_erionshahini_Layout.Service
{
    public class TokenStorageService
    {
        private string? _accessToken;

        public void SetAccessToken(string token)
        {
            _accessToken = token;
        }

        public string? GetAccessToken()
        {
            return _accessToken;
        }

        public void ClearAccessToken()
        {
            _accessToken = null;
        }

        public bool HasToken()
        {
            return !string.IsNullOrEmpty(_accessToken);
        }
    }
}
