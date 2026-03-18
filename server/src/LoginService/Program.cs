using GWWWlogin.Shared;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var servers = new List<ServerInfo>
{
    new(1, "Local Development", "127.1.1.110", 5999, "Online")
};

app.MapGet("/health", () => Results.Ok(new
{
    service = "login-service",
    status = "ok",
    utc = DateTime.UtcNow
}));

app.MapGet("/api/servers", () => Results.Ok(servers));

app.MapPost("/api/login", (LoginRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest(new LoginResponse(
            false,
            "Username and password are required.",
            null,
            Array.Empty<ServerInfo>()));
    }

    var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

    return Results.Ok(new LoginResponse(
        true,
        $"Welcome, {request.Username}.",
        token,
        servers));
});

app.Run();
