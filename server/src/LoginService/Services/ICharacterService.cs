using GWWWlogin.Shared;

namespace GWWWlogin.LoginService.Services;

public interface ICharacterService
{
    Task<CharacterResponse> CreateAsync(CreateCharacterRequest request, CancellationToken cancellationToken);

    Task<IReadOnlyList<CharacterResponse>> GetByAccountAsync(Guid accountId, CancellationToken cancellationToken);
}
