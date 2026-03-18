using GWWWlogin.LoginService.Data;
using GWWWlogin.LoginService.Models;
using GWWWlogin.LoginService.Rules;
using GWWWlogin.Shared;
using Microsoft.EntityFrameworkCore;

namespace GWWWlogin.LoginService.Services;

public sealed class CharacterService(AuthDbContext dbContext) : ICharacterService
{
    public async Task<CharacterResponse> CreateAsync(CreateCharacterRequest request, CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts
            .SingleOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken);

        if (account is null)
        {
            throw new KeyNotFoundException("Account not found.");
        }

        var name = request.Name.Trim();
        var normalizedName = name.ToUpperInvariant();
        var characterClass = request.Class.Trim();
        var gender = request.Gender.Trim();
        var faction = request.Faction.Trim();

        var (mapId, sceneName) = ResolveStartingLocation(faction);

        if (await dbContext.Characters.AnyAsync(x => x.NormalizedName == normalizedName, cancellationToken))
        {
            throw new InvalidOperationException("Character name already exists.");
        }

        var now = DateTime.UtcNow;
        var character = new Character
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Name = name,
            NormalizedName = normalizedName,
            Class = characterClass,
            Gender = gender,
            Faction = faction,
            SceneName = sceneName,
            Level = GameplayRules.StartingLevel,
            MapId = mapId,
            PositionX = GameplayRules.StartingPositionX,
            PositionY = GameplayRules.StartingPositionY,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        dbContext.Characters.Add(character);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(character);
    }

    public async Task<IReadOnlyList<CharacterResponse>> GetByAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var characters = await dbContext.Characters
            .Where(x => x.AccountId == accountId)
            .OrderBy(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return characters.Select(Map).ToList();
    }

    private static (int MapId, string SceneName) ResolveStartingLocation(string faction)
    {
        return faction.Trim().ToUpperInvariant() switch
        {
            "ATHENS" => (GameplayRules.AthensStartingMapId, GameplayRules.AthensStartingScene),
            "SPARTA" => (GameplayRules.SpartaStartingMapId, GameplayRules.SpartaStartingScene),
            _ => throw new InvalidOperationException("Unsupported faction.")
        };
    }

    private static CharacterResponse Map(Character character)
    {
        return new CharacterResponse(
            character.Id,
            character.AccountId,
            character.Name,
            character.Class,
            character.Gender,
            character.Faction,
            character.SceneName,
            character.Level,
            character.MapId,
            character.PositionX,
            character.PositionY,
            character.CreatedAtUtc);
    }
}
