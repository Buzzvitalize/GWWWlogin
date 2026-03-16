USE EternaGuerraAuth;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Auth_Register
    @Username NVARCHAR(64),
    @Password NVARCHAR(256),
    @Nickname NVARCHAR(64),
    @Region INT,
    @Locale NVARCHAR(16)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.PlayerAccount WHERE Username = @Username)
    BEGIN
        SELECT CAST(0 AS BIT) AS IsSuccess, N'USERNAME_TAKEN' AS ErrorCode;
        RETURN;
    END

    DECLARE @PasswordSalt NVARCHAR(64) = CONVERT(NVARCHAR(64), NEWID());
    DECLARE @PasswordHash VARBINARY(32) = HASHBYTES('SHA2_256', CONCAT(@PasswordSalt, N':', @Password));

    INSERT INTO dbo.PlayerAccount (
        Username,
        PasswordHash,
        PasswordSalt,
        Nickname,
        Faction,
        Region,
        Locale
    )
    VALUES (
        @Username,
        @PasswordHash,
        @PasswordSalt,
        @Nickname,
        N'Athens',
        @Region,
        @Locale
    );

    SELECT CAST(1 AS BIT) AS IsSuccess;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Character_Create
    @AccessToken UNIQUEIDENTIFIER,
    @CharacterName NVARCHAR(24),
    @ClassCode NVARCHAR(16),
    @Faction NVARCHAR(16)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PlayerId INT;
    SELECT @PlayerId = s.PlayerId
    FROM dbo.AuthSession s
    WHERE s.AccessToken = @AccessToken
      AND s.ExpiresAt > SYSUTCDATETIME()
      AND s.RevokedAt IS NULL;

    IF @PlayerId IS NULL
    BEGIN
        SELECT CAST(0 AS BIT) AS IsSuccess, N'INVALID_SESSION' AS ErrorCode;
        RETURN;
    END

    IF (SELECT COUNT(1) FROM dbo.PlayerCharacter WHERE PlayerId = @PlayerId AND IsDeleted = 0) >= 3
    BEGIN
        SELECT CAST(0 AS BIT) AS IsSuccess, N'CHARACTER_LIMIT_REACHED' AS ErrorCode;
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM dbo.PlayerCharacter WHERE CharacterName = @CharacterName AND IsDeleted = 0)
    BEGIN
        SELECT CAST(0 AS BIT) AS IsSuccess, N'CHARACTER_NAME_TAKEN' AS ErrorCode;
        RETURN;
    END

    INSERT INTO dbo.PlayerCharacter (PlayerId, CharacterName, ClassCode, Faction)
    VALUES (@PlayerId, @CharacterName, @ClassCode, @Faction);

    DECLARE @CharacterId INT = SCOPE_IDENTITY();

    SELECT
        CAST(1 AS BIT) AS IsSuccess,
        @CharacterId AS CharacterId,
        @CharacterName AS CharacterName,
        @ClassCode AS ClassCode,
        @Faction AS Faction,
        1 AS [Level],
        N'TerrAthens_Newbie' AS MapCode;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Character_Select
    @AccessToken UNIQUEIDENTIFIER,
    @CharacterId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SessionId BIGINT;
    DECLARE @PlayerId INT;

    SELECT
        @SessionId = s.SessionId,
        @PlayerId = s.PlayerId
    FROM dbo.AuthSession s
    WHERE s.AccessToken = @AccessToken
      AND s.ExpiresAt > SYSUTCDATETIME()
      AND s.RevokedAt IS NULL;

    IF @SessionId IS NULL
    BEGIN
        SELECT CAST(0 AS BIT) AS IsSuccess, N'INVALID_SESSION' AS ErrorCode;
        RETURN;
    END

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PlayerCharacter c
        WHERE c.CharacterId = @CharacterId
          AND c.PlayerId = @PlayerId
          AND c.IsDeleted = 0
    )
    BEGIN
        SELECT CAST(0 AS BIT) AS IsSuccess, N'CHARACTER_NOT_FOUND' AS ErrorCode;
        RETURN;
    END

    UPDATE dbo.AuthSession
    SET ActiveCharacterId = @CharacterId
    WHERE SessionId = @SessionId;

    SELECT CAST(1 AS BIT) AS IsSuccess;
END
GO
