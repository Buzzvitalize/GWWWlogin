USE EternaGuerraAuth;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Auth_Login
    @Username NVARCHAR(64),
    @Password NVARCHAR(256),
    @Region INT,
    @Locale NVARCHAR(16),
    @ClientVersion NVARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PlayerId INT;
    DECLARE @Nickname NVARCHAR(64);
    DECLARE @Faction NVARCHAR(16);
    DECLARE @PasswordSalt NVARCHAR(64);
    DECLARE @StoredHash VARBINARY(32);
    DECLARE @CalculatedHash VARBINARY(32);

    SELECT
        @PlayerId = p.PlayerId,
        @Nickname = p.Nickname,
        @Faction = p.Faction,
        @PasswordSalt = p.PasswordSalt,
        @StoredHash = p.PasswordHash
    FROM dbo.PlayerAccount p
    WHERE p.Username = @Username
      AND p.IsActive = 1;

    IF @PlayerId IS NULL
    BEGIN
        SELECT CAST(0 AS BIT) AS IsSuccess, N'INVALID_CREDENTIALS' AS ErrorCode;
        RETURN;
    END

    SET @CalculatedHash = HASHBYTES('SHA2_256', CONCAT(@PasswordSalt, N':', @Password));

    IF @CalculatedHash <> @StoredHash
    BEGIN
        SELECT CAST(0 AS BIT) AS IsSuccess, N'INVALID_CREDENTIALS' AS ErrorCode;
        RETURN;
    END

    DECLARE @AccessToken UNIQUEIDENTIFIER = NEWID();
    DECLARE @RefreshToken UNIQUEIDENTIFIER = NEWID();
    DECLARE @ExpiresAt DATETIME2(0) = DATEADD(SECOND, 3600, SYSUTCDATETIME());

    INSERT INTO dbo.AuthSession (PlayerId, AccessToken, RefreshToken, ExpiresAt)
    VALUES (@PlayerId, @AccessToken, @RefreshToken, @ExpiresAt);

    UPDATE dbo.PlayerAccount
    SET LastLoginAt = SYSUTCDATETIME(),
        Region = @Region,
        Locale = @Locale
    WHERE PlayerId = @PlayerId;

    SELECT
        CAST(1 AS BIT) AS IsSuccess,
        CAST(@AccessToken AS NVARCHAR(36)) AS AccessToken,
        CAST(@RefreshToken AS NVARCHAR(36)) AS RefreshToken,
        3600 AS ExpiresIn,
        @PlayerId AS PlayerId,
        @Nickname AS Nickname,
        @Faction AS Faction,
        @ClientVersion AS ClientVersionAccepted;
END
GO
