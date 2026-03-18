using GWWWlogin.LoginService.Data;
using GWWWlogin.LoginService.Extensions;
using GWWWlogin.LoginService.Services;
using GWWWlogin.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDb")));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();

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

app.MapGet("/health", async (AuthDbContext dbContext, CancellationToken cancellationToken) =>
{
    var accountCount = await dbContext.Accounts.CountAsync(cancellationToken);
    var characterCount = await dbContext.Characters.CountAsync(cancellationToken);

    return Results.Ok(new
    {
        service = "login-service",
        status = "ok",
        utc = DateTime.UtcNow,
        accounts = accountCount,
        characters = characterCount
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
        !request.Gender.HasValidCharacterPayload(3, 16))
    {
        return Results.BadRequest(new
        {
            message = "Character name, class and gender are required with valid lengths."
        });
    }

    if (!request.Class.IsAllowedCharacterClass())
    {
        return Results.BadRequest(new
        {
            message = "Class must be Warrior, Mage or Champion."
        });
    }

    if (!request.Gender.IsAllowedCharacterGender())
    {
        return Results.BadRequest(new
        {
            message = "Gender must be Male or Female."
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

app.Run();
