using GWWWlogin.GatewayService.Data;
using GWWWlogin.GatewayService.Protocols;
using Microsoft.EntityFrameworkCore;

namespace GWWWlogin.GatewayService.Services;

public sealed class GatewaySessionService(GatewayDbContext dbContext) : IGatewaySessionService
{
    public async Task<GatewayCommandResult> HandleCommandAsync(string commandLine, CancellationToken cancellationToken)
    {
        var parts = commandLine.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length != 2)
        {
            return new GatewayCommandResult(false, "ERROR invalid_command_format");
        }

        var command = parts[0].ToUpperInvariant();
        var token = parts[1];

        return command switch
        {
            "HELLO" => await HandleHelloAsync(token, cancellationToken),
            "ENTER_MAP" => await HandleEnterMapAsync(token, cancellationToken),
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
        return new GatewayCommandResult(
            true,
            $"ENTER_MAP_OK {character.MapId} {character.PositionX} {character.PositionY} {character.Level}");
    }
}
