using Services.DTOs;

namespace Services;

public interface IRegisterService
{
    Task<Result<AuthResult>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
}
