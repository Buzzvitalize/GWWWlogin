USE EternaGuerraAuth;
GO

IF COL_LENGTH('dbo.AuthSession', 'ActiveCharacterId') IS NULL
BEGIN
    ALTER TABLE dbo.AuthSession
    ADD ActiveCharacterId INT NULL;
END
GO

IF OBJECT_ID('dbo.PlayerCharacter', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PlayerCharacter (
        CharacterId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PlayerCharacter PRIMARY KEY,
        PlayerId INT NOT NULL,
        CharacterName NVARCHAR(24) NOT NULL,
        ClassCode NVARCHAR(16) NOT NULL,
        Faction NVARCHAR(16) NOT NULL,
        Level INT NOT NULL CONSTRAINT DF_PlayerCharacter_Level DEFAULT 1,
        MapCode NVARCHAR(64) NOT NULL CONSTRAINT DF_PlayerCharacter_MapCode DEFAULT N'TerrAthens_Newbie',
        PosX FLOAT NOT NULL CONSTRAINT DF_PlayerCharacter_PosX DEFAULT 0,
        PosY FLOAT NOT NULL CONSTRAINT DF_PlayerCharacter_PosY DEFAULT 0,
        PosZ FLOAT NOT NULL CONSTRAINT DF_PlayerCharacter_PosZ DEFAULT 0,
        IsDeleted BIT NOT NULL CONSTRAINT DF_PlayerCharacter_IsDeleted DEFAULT 0,
        CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_PlayerCharacter_CreatedAt DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_PlayerCharacter_PlayerAccount FOREIGN KEY (PlayerId)
            REFERENCES dbo.PlayerAccount(PlayerId)
    );

    CREATE UNIQUE INDEX UX_PlayerCharacter_CharacterName ON dbo.PlayerCharacter(CharacterName) WHERE IsDeleted = 0;
    CREATE INDEX IX_PlayerCharacter_PlayerId ON dbo.PlayerCharacter(PlayerId);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_AuthSession_PlayerCharacter'
)
BEGIN
    ALTER TABLE dbo.AuthSession
    ADD CONSTRAINT FK_AuthSession_PlayerCharacter
        FOREIGN KEY (ActiveCharacterId) REFERENCES dbo.PlayerCharacter(CharacterId);
END
GO
