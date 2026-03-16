IF DB_ID('EternaGuerraAuth') IS NULL
BEGIN
    CREATE DATABASE EternaGuerraAuth;
END
GO

USE EternaGuerraAuth;
GO

IF OBJECT_ID('dbo.PlayerAccount', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PlayerAccount (
        PlayerId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PlayerAccount PRIMARY KEY,
        Username NVARCHAR(64) NOT NULL,
        PasswordHash VARBINARY(32) NOT NULL,
        PasswordSalt NVARCHAR(64) NOT NULL,
        Nickname NVARCHAR(64) NOT NULL,
        Faction NVARCHAR(16) NOT NULL CONSTRAINT DF_PlayerAccount_Faction DEFAULT N'Athens',
        Region INT NOT NULL CONSTRAINT DF_PlayerAccount_Region DEFAULT 11,
        Locale NVARCHAR(16) NOT NULL CONSTRAINT DF_PlayerAccount_Locale DEFAULT N'en_us',
        IsActive BIT NOT NULL CONSTRAINT DF_PlayerAccount_IsActive DEFAULT 1,
        LastLoginAt DATETIME2(0) NULL,
        CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_PlayerAccount_CreatedAt DEFAULT SYSUTCDATETIME()
    );

    CREATE UNIQUE INDEX UX_PlayerAccount_Username ON dbo.PlayerAccount(Username);
END
GO

IF OBJECT_ID('dbo.AuthSession', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.AuthSession (
        SessionId BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuthSession PRIMARY KEY,
        PlayerId INT NOT NULL,
        AccessToken UNIQUEIDENTIFIER NOT NULL,
        RefreshToken UNIQUEIDENTIFIER NOT NULL,
        ExpiresAt DATETIME2(0) NOT NULL,
        CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_AuthSession_CreatedAt DEFAULT SYSUTCDATETIME(),
        RevokedAt DATETIME2(0) NULL,
        CONSTRAINT FK_AuthSession_PlayerAccount FOREIGN KEY (PlayerId)
            REFERENCES dbo.PlayerAccount(PlayerId)
    );

    CREATE INDEX IX_AuthSession_PlayerId ON dbo.AuthSession(PlayerId);
    CREATE UNIQUE INDEX UX_AuthSession_AccessToken ON dbo.AuthSession(AccessToken);
    CREATE UNIQUE INDEX UX_AuthSession_RefreshToken ON dbo.AuthSession(RefreshToken);
END
GO
