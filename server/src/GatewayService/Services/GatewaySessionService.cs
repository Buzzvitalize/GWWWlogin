using GWWWlogin.GatewayService.Data;
using GWWWlogin.GatewayService.Protocols;
using GWWWlogin.GatewayService.World;
using Microsoft.EntityFrameworkCore;

namespace GWWWlogin.GatewayService.Services;

public sealed class GatewaySessionService(GatewayDbContext dbContext, IMapStateService mapStateService) : IGatewaySessionService
{
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
            $"HANDSHAKE_OK {session.AccountId} {session.SelectedCharacter.Id} {session.SelectedCharacter.MapId}");
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
            character.MapId,
            character.PositionX,
            character.PositionY,
            character.Level);

        var population = mapStateService.GetPopulation(state.MapId);

        return new GatewayCommandResult(
            true,
            $"ENTER_MAP_OK {state.MapId} {state.PositionX} {state.PositionY} {state.Level} {population}");
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
                $"PLAYER_INFO {mapState.CharacterName} {mapState.MapId} {mapState.PositionX} {mapState.PositionY} {mapState.Level}");
        }

        var character = session.SelectedCharacter;
        return new GatewayCommandResult(
            true,
            $"PLAYER_INFO {character.Name} {character.MapId} {character.PositionX} {character.PositionY} {character.Level}");
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

        return new GatewayCommandResult(
            true,
            $"MOVE_OK {state.MapId} {state.PositionX} {state.PositionY}");
    }
}
