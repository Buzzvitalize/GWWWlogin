using GWWWlogin.Shared;

namespace GWWWlogin.LoginService.Services;

public interface ISessionService
{
    Task<CharacterSelectionResponse> SelectCharacterAsync(SelectCharacterRequest request, CancellationToken cancellationToken);

    Task<SessionSummaryResponse?> GetByTokenAsync(string token, CancellationToken cancellationToken);
}
