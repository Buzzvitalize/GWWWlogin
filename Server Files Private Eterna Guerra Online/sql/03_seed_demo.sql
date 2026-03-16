USE EternaGuerraAuth;
GO

DECLARE @Username NVARCHAR(64) = N'admin';
DECLARE @Password NVARCHAR(256) = N'Admin123*';
DECLARE @PasswordSalt NVARCHAR(64) = CONVERT(NVARCHAR(64), NEWID());
DECLARE @PasswordHash VARBINARY(32) = HASHBYTES('SHA2_256', CONCAT(@PasswordSalt, N':', @Password));

IF NOT EXISTS (SELECT 1 FROM dbo.PlayerAccount WHERE Username = @Username)
BEGIN
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
        N'Admin',
        N'Athens',
        11,
        N'en_us'
    );
END
GO
