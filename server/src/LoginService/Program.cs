using GWWWlogin.LoginService.Data;
using GWWWlogin.LoginService.Extensions;
using GWWWlogin.LoginService.Services;
using GWWWlogin.Shared;
using GWWWlogin.Shared.Maps;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDb")));

builder.Services.AddSingleton<IClientMapCatalog, ClientMapCatalog>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<ISessionService, SessionService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

var servers = new List<ServerInfo>
{
    new(1, "Local Development", "127.1.1.110", 5999, "Online")
};

app.MapGet("/health", async (AuthDbContext dbContext, IClientMapCatalog mapCatalog, CancellationToken cancellationToken) =>
{
    var accountCount = await dbContext.Accounts.CountAsync(cancellationToken);
    var characterCount = await dbContext.Characters.CountAsync(cancellationToken);
    var sessionCount = await dbContext.Sessions.CountAsync(cancellationToken);

    return Results.Ok(new
    {
        service = "login-service",
        status = "ok",
        utc = DateTime.UtcNow,
        accounts = accountCount,
        characters = characterCount,
        sessions = sessionCount,
        startingMaps = new
        {
            Athens = mapCatalog.GetStartingMap("Athens")?.SceneName,
            Sparta = mapCatalog.GetStartingMap("Sparta")?.SceneName
        }
    });
});

app.MapGet("/api/servers", () => Results.Ok(servers));

app.MapPost("/api/accounts/register", async (
    RegisterAccountRequest request,
    IAccountService accountService,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.Username) ||
        string.IsNullOrWhiteSpace(request.Password) ||
        string.IsNullOrWhiteSpace(request.Email))
    {
        return Results.BadRequest(new
        {
            message = "Username, password and email are required."
        });
    }

    try
    {
        var response = await accountService.RegisterAsync(request, cancellationToken);
        return Results.Created($"/api/accounts/{response.Id}", response);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new
        {
            message = ex.Message
        });
    }
});

app.MapPost("/api/login", async (
    LoginRequest request,
    IAccountService accountService,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest(new LoginResponse(
            false,
            "Username and password are required.",
            null,
            null,
            null,
            Array.Empty<ServerInfo>()));
    }

    var response = await accountService.LoginAsync(request, servers, cancellationToken);
    return response.Success
        ? Results.Ok(response)
        : Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
});

app.MapGet("/api/accounts/{accountId:guid}/characters", async (
    Guid accountId,
    ICharacterService characterService,
    CancellationToken cancellationToken) =>
{
    var characters = await characterService.GetByAccountAsync(accountId, cancellationToken);
    return Results.Ok(characters);
});

app.MapPost("/api/characters", async (
    CreateCharacterRequest request,
    ICharacterService characterService,
    CancellationToken cancellationToken) =>
{
    if (!request.Name.HasValidCharacterPayload(3, 24) ||
        !request.Class.HasValidCharacterPayload(3, 24) ||
        !request.Gender.HasValidCharacterPayload(3, 16) ||
        !request.Faction.HasValidCharacterPayload(3, 16))
    {
        return Results.BadRequest(new
        {
            message = "Character name, class, gender and faction are required with valid lengths."
        });
    }

    if (!request.Class.IsAllowedCharacterClass())
    {
        return Results.BadRequest(new
        {
            message = "Class must be Warrior, Champion, Priest or Mage."
        });
    }

    if (!request.Gender.IsAllowedCharacterGender())
    {
        return Results.BadRequest(new
        {
            message = "Gender must be Male or Female."
        });
    }

    if (!request.Faction.IsAllowedFaction())
    {
        return Results.BadRequest(new
        {
            message = "Faction must be Athens or Sparta."
        });
    }

    try
    {
        var response = await characterService.CreateAsync(request, cancellationToken);
        return Results.Created($"/api/characters/{response.Id}", response);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(new
        {
            message = ex.Message
        });
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new
        {
            message = ex.Message
        });
    }
});

app.MapGet("/api/sessions/{token}", async (
    string token,
    ISessionService sessionService,
    CancellationToken cancellationToken) =>
{
    var session = await sessionService.GetByTokenAsync(token, cancellationToken);
    return session is null ? Results.NotFound() : Results.Ok(session);
});

app.MapPost("/api/sessions/select-character", async (
    SelectCharacterRequest request,
    ISessionService sessionService,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.SessionToken) || request.CharacterId == Guid.Empty)
    {
        return Results.BadRequest(new
        {
            message = "Session token and character id are required."
        });
    }

    try
    {
        var response = await sessionService.SelectCharacterAsync(request, cancellationToken);
        return Results.Ok(response);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(new
        {
            message = ex.Message
        });
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new
        {
            message = ex.Message
        });
    }
});

app.MapPost("/api/sessions/{token}/gateway-ticket", async (
    string token,
    ISessionService sessionService,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(token))
    {
        return Results.BadRequest(new
        {
            message = "Session token is required."
        });
    }

    try
    {
        var response = await sessionService.PrepareGatewayAsync(token, cancellationToken);
        return Results.Ok(response);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(new
        {
            message = ex.Message
        });
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new
        {
            message = ex.Message
        });
    }
});

app.Run();
