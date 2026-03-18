using GWWWlogin.GatewayService.Broadcast;
using GWWWlogin.GatewayService.Data;
using GWWWlogin.GatewayService.Definitions;
using GWWWlogin.GatewayService.Models;
using GWWWlogin.GatewayService.Protocols;
using GWWWlogin.GatewayService.World;
using GWWWlogin.Shared;
using Microsoft.EntityFrameworkCore;

namespace GWWWlogin.GatewayService.Services;

public sealed class GatewaySessionService(GatewayDbContext dbContext, IMapStateService mapStateService, IMapDefinitionService mapDefinitionService, IMapBroadcastService mapBroadcastService, IGameServerBridgeClient gameServerBridgeClient) : IGatewaySessionService
{
    private const float VisibilityRadius = 180f;

    public async Task<GatewayCommandResult> HandleCommandAsync(string commandLine, CancellationToken cancellationToken)
    {
        var parts = commandLine.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length < 2)
        {
            return new GatewayCommandResult(false, "ERROR invalid_command_format");
        }

        var command = parts[0].ToUpperInvariant();
        var token = parts[1];

        return command switch
        {
            "HELLO" when parts.Length == 2 => await HandleHelloAsync(token, cancellationToken),
            "ENTER_MAP" when parts.Length == 2 => await HandleEnterMapAsync(token, cancellationToken),
            "PING" when parts.Length == 2 => await HandlePingAsync(token, cancellationToken),
            "WHOAMI" when parts.Length == 2 => await HandleWhoAmIAsync(token, cancellationToken),
            "MOVE" when parts.Length == 4 && float.TryParse(parts[2], out var x) && float.TryParse(parts[3], out var y) => await HandleMoveAsync(token, x, y, cancellationToken),
            "LEAVE" when parts.Length == 2 => await HandleLeaveAsync(token, cancellationToken),
            "POLL" when parts.Length == 2 => await HandlePollAsync(token, cancellationToken),
            "AROUND" when parts.Length == 2 => await HandleAroundAsync(token, cancellationToken),
            _ => new GatewayCommandResult(false, "ERROR unknown_command")
        };
    }

    private async Task<GatewayCommandResult> HandleHelloAsync(string token, CancellationToken cancellationToken)
    {
        var session = await dbContext.Sessions
            .Include(x => x.SelectedCharacter)
            .SingleOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (session is null)
        {
            return new GatewayCommandResult(false, "ERROR session_not_found");
        }

        if (session.ExpiresAtUtc <= DateTime.UtcNow)
        {
            return new GatewayCommandResult(false, "ERROR session_expired");
        }

        if (session.SelectedCharacter is null)
        {
            return new GatewayCommandResult(false, "ERROR character_not_selected");
        }

        session.LastSeenAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new GatewayCommandResult(
            true,
            GatewayProtocolSerializer.FormatHandshake(
                session.AccountId,
                session.SelectedCharacter,
                VisibilityRadius,
                GatewayProtocolSerializer.WorldEventVersion));
    }

    private async Task<GatewayCommandResult> HandleEnterMapAsync(string token, CancellationToken cancellationToken)
    {
        var session = await dbContext.Sessions
            .Include(x => x.SelectedCharacter)
            .SingleOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (session is null)
        {
            return new GatewayCommandResult(false, "ERROR session_not_found");
        }

        if (session.ExpiresAtUtc <= DateTime.UtcNow)
        {
            return new GatewayCommandResult(false, "ERROR session_expired");
        }

        if (session.SelectedCharacter is null)
        {
            return new GatewayCommandResult(false, "ERROR character_not_selected");
        }

        session.LastSeenAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        var character = session.SelectedCharacter;
        var state = mapStateService.EnterMap(
            session.Id,
            session.AccountId,
            character.Id,
            character.Name,
            character.Faction,
            character.SceneName,
            character.MapId,
            character.PositionX,
            character.PositionY,
            character.Level);

        var population = mapStateService.GetPopulation(state.MapId);

        var enterEvent = mapBroadcastService.Publish(state.MapId, new MapBroadcastEvent(
            0,
            "ENTER",
            state.SessionId,
            state.CharacterName,
            state.SceneName,
            state.PositionX,
            state.PositionY,
            DateTime.UtcNow,
            "player",
            state.CharacterId.ToString()));

        state.LastSeenBroadcastSequence = enterEvent.SequenceId;

        var mapDefinition = mapDefinitionService.GetById(state.MapId);
        var npcCount = mapDefinition?.Npcs.Count ?? 0;
        var monsterCount = mapDefinition?.Monsters.Count ?? 0;

        return new GatewayCommandResult(
            true,
            $"ENTER_MAP_OK {state.SceneName} {state.MapId} {state.PositionX} {state.PositionY} {state.Level} {population} {npcCount} {monsterCount}");
    }

    private async Task<GatewayCommandResult> HandlePingAsync(string token, CancellationToken cancellationToken)
    {
        var session = await dbContext.Sessions
            .SingleOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (session is null)
        {
            return new GatewayCommandResult(false, "ERROR session_not_found");
        }

        if (session.ExpiresAtUtc <= DateTime.UtcNow)
        {
            return new GatewayCommandResult(false, "ERROR session_expired");
        }

        session.LastSeenAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new GatewayCommandResult(true, $"PONG {session.Id}");
    }

    private async Task<GatewayCommandResult> HandleWhoAmIAsync(string token, CancellationToken cancellationToken)
    {
        var session = await dbContext.Sessions
            .Include(x => x.SelectedCharacter)
            .SingleOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (session is null)
        {
            return new GatewayCommandResult(false, "ERROR session_not_found");
        }

        if (session.ExpiresAtUtc <= DateTime.UtcNow)
        {
            return new GatewayCommandResult(false, "ERROR session_expired");
        }

        if (session.SelectedCharacter is null)
        {
            return new GatewayCommandResult(false, "ERROR character_not_selected");
        }

        var mapState = mapStateService.GetBySession(session.Id);
        if (mapState is not null)
        {
            return new GatewayCommandResult(
                true,
                $"PLAYER_INFO {mapState.CharacterName} {mapState.Faction} {mapState.SceneName} {mapState.MapId} {mapState.PositionX} {mapState.PositionY} {mapState.Level}");
        }

        var character = session.SelectedCharacter;
        return new GatewayCommandResult(
            true,
            $"PLAYER_INFO {character.Name} {character.Faction} {character.SceneName} {character.MapId} {character.PositionX} {character.PositionY} {character.Level}");
    }

    private async Task<GatewayCommandResult> HandleMoveAsync(string token, float positionX, float positionY, CancellationToken cancellationToken)
    {
        var session = await dbContext.Sessions
            .SingleOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (session is null)
        {
            return new GatewayCommandResult(false, "ERROR session_not_found");
        }

        if (session.ExpiresAtUtc <= DateTime.UtcNow)
        {
            return new GatewayCommandResult(false, "ERROR session_expired");
        }

        var state = mapStateService.Move(session.Id, positionX, positionY);
        if (state is null)
        {
            return new GatewayCommandResult(false, "ERROR player_not_in_map");
        }

        session.LastSeenAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        var moveEvent = mapBroadcastService.Publish(state.MapId, new MapBroadcastEvent(
            0,
            "MOVE",
            state.SessionId,
            state.CharacterName,
            state.SceneName,
            state.PositionX,
            state.PositionY,
            DateTime.UtcNow,
            "player",
            state.CharacterId.ToString()));

        state.LastSeenBroadcastSequence = moveEvent.SequenceId;

        return new GatewayCommandResult(
            true,
            $"MOVE_OK {state.MapId} {state.PositionX} {state.PositionY}");
    }

    private async Task<GatewayCommandResult> HandleLeaveAsync(string token, CancellationToken cancellationToken)
    {
        var session = await dbContext.Sessions
            .SingleOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (session is null)
        {
            return new GatewayCommandResult(false, "ERROR session_not_found");
        }

        var state = mapStateService.Leave(session.Id);
        if (state is null)
        {
            return new GatewayCommandResult(false, "ERROR player_not_in_map");
        }

        mapBroadcastService.Publish(state.MapId, new MapBroadcastEvent(
            0,
            "LEAVE",
            state.SessionId,
            state.CharacterName,
            state.SceneName,
            state.PositionX,
            state.PositionY,
            DateTime.UtcNow,
            "player",
            state.CharacterId.ToString()));

        session.LastSeenAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new GatewayCommandResult(true, $"LEAVE_OK {state.SceneName} {state.MapId}");
    }

    private async Task<GatewayCommandResult> HandlePollAsync(string token, CancellationToken cancellationToken)
    {
        var session = await dbContext.Sessions
            .SingleOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (session is null)
        {
            return new GatewayCommandResult(false, "ERROR session_not_found");
        }

        var state = mapStateService.GetBySession(session.Id);
        if (state is null)
        {
            return new GatewayCommandResult(false, "ERROR player_not_in_map");
        }

        var events = mapBroadcastService.ReadSince(state.MapId, state.LastSeenBroadcastSequence, 20);
        if (events.Count > 0)
        {
            state.LastSeenBroadcastSequence = events[^1].SequenceId;
        }

        var visibleEvents = events
            .Where(x => x.SessionId != state.SessionId)
            .Where(x => IsWithinVisibilityRange(state, x.PositionX, x.PositionY))
            .ToList();

        return new GatewayCommandResult(true, GatewayProtocolSerializer.FormatWorldEvents(visibleEvents));
    }

    private async Task<GatewayCommandResult> HandleAroundAsync(string token, CancellationToken cancellationToken)
    {
        var session = await dbContext.Sessions
            .SingleOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (session is null)
        {
            return new GatewayCommandResult(false, "ERROR session_not_found");
        }

        var state = mapStateService.GetBySession(session.Id);
        if (state is null)
        {
            return new GatewayCommandResult(false, "ERROR player_not_in_map");
        }

        var players = mapStateService.GetPlayersInRange(state.MapId, state.PositionX, state.PositionY, VisibilityRadius)
            .Where(x => x.SessionId != state.SessionId)
            .ToList();
        var npcs = mapStateService.GetNpcsInRange(state.MapId, state.PositionX, state.PositionY, VisibilityRadius);
        var monsters = await GetVisibleMonstersAsync(state, cancellationToken);

        return new GatewayCommandResult(true, GatewayProtocolSerializer.FormatAround(players, npcs, monsters));
    }

    private async Task<IReadOnlyList<MonsterSpawnState>> GetVisibleMonstersAsync(ActivePlayerState observer, CancellationToken cancellationToken)
    {
        var bridgeMonsters = await gameServerBridgeClient.GetMonstersAsync(observer.MapId, cancellationToken);
        if (bridgeMonsters.Count > 0)
        {
            return bridgeMonsters
                .Where(x => IsWithinVisibilityRange(observer, x.PositionX, x.PositionY))
                .Select(ToMonsterSpawnState)
                .ToList();
        }

        return mapStateService.GetMonstersInRange(observer.MapId, observer.PositionX, observer.PositionY, VisibilityRadius);
    }

    private static MonsterSpawnState ToMonsterSpawnState(WorldBridgeMonsterSnapshot monster)
    {
        return new MonsterSpawnState(
            monster.InstanceId,
            string.Empty,
            monster.DisplayName,
            monster.SceneName,
            monster.MapId,
            monster.PositionX,
            monster.PositionY,
            monster.ZoneKey,
            monster.IsAlive,
            monster.LastUpdatedAtUtc);
    }

    private static bool IsWithinVisibilityRange(ActivePlayerState observer, float targetX, float targetY)
    {
        var deltaX = targetX - observer.PositionX;
        var deltaY = targetY - observer.PositionY;
        var distanceSquared = (deltaX * deltaX) + (deltaY * deltaY);
        var radiusSquared = VisibilityRadius * VisibilityRadius;
        return distanceSquared <= radiusSquared;
    }
}
