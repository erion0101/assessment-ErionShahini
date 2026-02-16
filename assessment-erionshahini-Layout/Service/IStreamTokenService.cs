namespace assessment_erionshahini_Layout.Service;

/// <summary>Creates short-lived JWTs for video stream access. API validates with same secret.</summary>
public interface IStreamTokenService
{
    string CreateStreamToken(Guid videoId);
}
