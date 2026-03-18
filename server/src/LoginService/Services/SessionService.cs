using GWWWlogin.LoginService.Data;
using GWWWlogin.Shared;
using Microsoft.EntityFrameworkCore;

namespace GWWWlogin.LoginService.Services;

public sealed class SessionService(AuthDbContext dbContext) : ISessionService
{
    public async Task<CharacterSelectionResponse> SelectCharacterAsync(SelectCharacterRequest request, CancellationToken cancellationToken)
    {
        var token = request.SessionToken.Trim();

        var session = await dbContext.Sessions
            .SingleOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (session is null)
        {
            throw new KeyNotFoundException("Session not found.");
        }

        if (session.ExpiresAtUtc <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("Session expired.");
        }

        var character = await dbContext.Characters
            .SingleOrDefaultAsync(x => x.Id == request.CharacterId && x.AccountId == session.AccountId, cancellationToken);

        if (character is null)
        {
            throw new KeyNotFoundException("Character not found for this session.");
        }

        session.SelectedCharacterId = character.Id;
        session.LastSeenAtUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CharacterSelectionResponse(
            session.Id,
            session.AccountId,
            character.Id,
            character.Name,
            character.MapId,
            character.PositionX,
            character.PositionY,
            character.Level,
            session.ExpiresAtUtc,
            true);
    }

    public async Task<SessionSummaryResponse?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        var normalizedToken = token.Trim();

        var session = await dbContext.Sessions
            .SingleOrDefaultAsync(x => x.Token == normalizedToken, cancellationToken);

        if (session is null)
        {
            return null;
        }

        return new SessionSummaryResponse(
            session.Id,
            session.AccountId,
            session.SelectedCharacterId,
            session.Token,
            session.CreatedAtUtc,
            session.ExpiresAtUtc,
            session.LastSeenAtUtc,
            session.ExpiresAtUtc > DateTime.UtcNow);
    }
}
