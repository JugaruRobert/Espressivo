CREATE DATABASE EmotionBasedAudioPlayer
USE EmotionBasedAudioPlayer
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
	[Username] NVARCHAR(20),
	[Email] NVARCHAR(50) UNIQUE,
	[Password] NVARCHAR(50),
	CONSTRAINT [PK_Users] PRIMARY KEY ([Username])
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
	[Username] NVARCHAR(20),
	[ArtistID] NVARCHAR(50),
	CONSTRAINT [PK_UsersArtists] PRIMARY KEY ([Username],[ArtistID]),
	CONSTRAINT [FK_UsersArtists] FOREIGN KEY ([Username])
		REFERENCES [Users]([Username]) ON DELETE CASCADE,
	CONSTRAINT [FK_ArtistsUsers] FOREIGN KEY ([ArtistID])
		REFERENCES [Artists]([ArtistID]) ON DELETE CASCADE
);

CREATE TABLE [UsersGenres](
	[Username] NVARCHAR(20),
	[GenreID] UNIQUEIDENTIFIER,
	CONSTRAINT [PK_UsersGenres] PRIMARY KEY ([Username],[GenreID]),
	CONSTRAINT [FK_UsersGenres] FOREIGN KEY ([Username])
		REFERENCES [Users]([Username]) ON DELETE CASCADE,
	CONSTRAINT [FK_GenresUsers] FOREIGN KEY ([GenreID])
		REFERENCES [Genres]([GenreID]) ON DELETE CASCADE
);

