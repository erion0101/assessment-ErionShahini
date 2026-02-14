namespace assessment_erionshahini_Layout.ConnetionsWithAPIs
{
    public interface IApiService
    {
        Task<T> HttpGET<T>(string url);
        Task HttpDELETE(string url);
        Task<T> HttpPOST<T>(string url, object postData);
        Task<T> HttpPUT<T>(string url, object putData);
    }
}
