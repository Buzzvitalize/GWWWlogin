using GWWWlogin.Shared;

namespace GWWWlogin.LoginService.Services;

public interface IAccountService
{
    Task<AccountResponse> RegisterAsync(RegisterAccountRequest request, CancellationToken cancellationToken);

    Task<LoginResponse> LoginAsync(LoginRequest request, IReadOnlyList<ServerInfo> servers, CancellationToken cancellationToken);
}
