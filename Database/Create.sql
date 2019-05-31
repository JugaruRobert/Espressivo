CREATE DATABASE EmotionBasedMusicPlayer
USE EmotionBasedMusicPlayer
GO

IF OBJECT_ID('dbo.UsersArtists', 'U') IS NOT NULL 
		DROP TABLE dbo.UsersArtists;
IF OBJECT_ID('dbo.UsersGenres', 'U') IS NOT NULL 
		DROP TABLE dbo.UsersGenres;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL 
		DROP TABLE dbo.Users;
IF OBJECT_ID('dbo.Artists', 'U') IS NOT NULL 
		DROP TABLE dbo.Artists;
IF OBJECT_ID('dbo.Genres', 'U') IS NOT NULL 
		DROP TABLE dbo.Genres;

CREATE TABLE [Users] (
	[UserID] uniqueidentifier,
	[Username] NVARCHAR(20) UNIQUE,
	[Email] NVARCHAR(50) UNIQUE,
	[Password] NVARCHAR(max),
	CONSTRAINT [PK_Users] PRIMARY KEY ([UserID])
);

CREATE TABLE [Artists] (
	[ArtistID] NVARCHAR(50),
	[Name] NVARCHAR(100),
	CONSTRAINT [PK_Artists] PRIMARY KEY ([ArtistID])
);

CREATE TABLE [Genres] (
	[GenreID] UNIQUEIDENTIFIER,
	[Name] NVARCHAR(100),
	CONSTRAINT [PK_Genres] PRIMARY KEY ([GenreID])
);

CREATE TABLE [UsersArtists](
	[UserID] UNIQUEIDENTIFIER,
	[ArtistID] NVARCHAR(50),
	CONSTRAINT [PK_UsersArtists] PRIMARY KEY ([UserID],[ArtistID]),
	CONSTRAINT [FK_UsersArtists] FOREIGN KEY ([UserID])
		REFERENCES [Users]([UserID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ArtistsUsers] FOREIGN KEY ([ArtistID])
		REFERENCES [Artists]([ArtistID]) ON DELETE CASCADE
);

CREATE TABLE [UsersGenres](
	[UserID] UNIQUEIDENTIFIER,
	[GenreID] UNIQUEIDENTIFIER,
	CONSTRAINT [PK_UsersGenres] PRIMARY KEY ([UserID],[GenreID]),
	CONSTRAINT [FK_UsersGenres] FOREIGN KEY ([UserID])
		REFERENCES [Users]([UserID]) ON DELETE CASCADE,
	CONSTRAINT [FK_GenresUsers] FOREIGN KEY ([GenreID])
		REFERENCES [Genres]([GenreID]) ON DELETE CASCADE
);

