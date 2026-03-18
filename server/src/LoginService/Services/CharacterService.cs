using GWWWlogin.LoginService.Data;
using GWWWlogin.LoginService.Models;
using GWWWlogin.LoginService.Rules;
using GWWWlogin.Shared;
using GWWWlogin.Shared.Maps;
using Microsoft.EntityFrameworkCore;

namespace GWWWlogin.LoginService.Services;

public sealed class CharacterService(AuthDbContext dbContext, IClientMapCatalog mapCatalog) : ICharacterService
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

        var startingMap = ResolveStartingLocation(faction);

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
            SceneName = startingMap.SceneName,
            Level = GameplayRules.StartingLevel,
            MapId = startingMap.MapId,
            PositionX = startingMap.DefaultSpawnX,
            PositionY = startingMap.DefaultSpawnY,
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

    private ClientMapDefinition ResolveStartingLocation(string faction)
    {
        return mapCatalog.GetStartingMap(faction)
            ?? throw new InvalidOperationException("Unsupported faction.");
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
