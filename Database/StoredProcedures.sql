USE EmotionBasedMusicPlayer
GO

--Users
CREATE PROCEDURE [Users_Insert]
	@UserID UNIQUEIDENTIFIER,
	@Username NVARCHAR(20),
	@Email NVARCHAR(50),
	@Password NVARCHAR(max)
AS
BEGIN
	INSERT INTO Users([UserID],[Username],[Email],[Password])
	VALUES (@UserID,@Username,@Email,@Password)
END
GO

CREATE PROCEDURE [Users_ReadByUsernameOrEmail]
	@UserID UNIQUEIDENTIFIER,
	@Username NVARCHAR(20),
	@Email NVARCHAR(50)
AS
BEGIN
	SELECT *
	FROM [Users]
	WHERE ([Username] = @Username OR [Email] = @Email) AND [UserID] <> @UserID
END
GO

CREATE PROCEDURE [Users_Read]
	@Username NVARCHAR(20),
	@Password NVARCHAR(max)
AS
BEGIN
	SELECT *
	FROM [Users]
	WHERE [Username] = @Username AND [Password] = @Password
END
GO

CREATE PROCEDURE [Users_ReadAll]
AS
BEGIN
	SELECT * FROM [Users]
END
GO

CREATE PROCEDURE [Users_ReadByUsername]
	@Username NVARCHAR(20)
AS
BEGIN
	SELECT *
	FROM [Users]
	WHERE [Username] = @Username
END
GO

CREATE PROCEDURE [Users_ReadByID]
	@UserID UNIQUEIDENTIFIER
AS
BEGIN
	SELECT *
	FROM [Users]
	WHERE [UserID] = @UserID
END
GO

CREATE PROCEDURE [Users_Update]
	@UserID uniqueidentifier,
	@Username NVARCHAR(20),
	@Email NVARCHAR(50)
AS
BEGIN
	UPDATE [Users]
	SET [Email]=@Email,[Username] = @Username
	WHERE [UserID] = @UserID
END
GO

CREATE PROCEDURE [Users_RemoveByID]
	@UserID uniqueidentifier
AS
BEGIN
	DELETE FROM [Users]
	WHERE [UserID] = @UserID
END
GO

CREATE PROCEDURE [Users_RemoveByUsername]
	@Username NVARCHAR(20)
AS
BEGIN
	DELETE FROM [Users]
	WHERE [Username] = @Username
END
GO

--Genres
CREATE PROCEDURE [Genres_Insert]
	@GenreID UNIQUEIDENTIFIER,
	@Name NVARCHAR(100)
AS
BEGIN
	INSERT INTO Genres([GenreID],[Name])
	VALUES (@GenreID,@Name)
END
GO

CREATE PROCEDURE [Genres_ReadAll]
AS
BEGIN
	SELECT * FROM [Genres]
END
GO

CREATE PROCEDURE [Genres_ReadByID]
	@GenreID UNIQUEIDENTIFIER
AS
BEGIN
	SELECT *
	FROM [Genres]
	WHERE [GenreID] = @GenreID
END
GO

CREATE PROCEDURE [Genres_ReadByName]
	@Name NVARCHAR(100)
AS
BEGIN
	SELECT *
	FROM [Genres]
	WHERE [Name] = @Name
END
GO

CREATE PROCEDURE [Genres_Update]
	@GenreID UNIQUEIDENTIFIER,
	@Name NVARCHAR(100)
AS
BEGIN
	UPDATE [Genres]
	SET [Name]=@Name
	WHERE [GenreID] = @GenreID
END
GO

CREATE PROCEDURE [Genres_Remove]
	@GenreID UNIQUEIDENTIFIER
AS
BEGIN
	DELETE FROM [Genres]
	WHERE [GenreID] = @GenreID
END
GO

CREATE PROCEDURE [Genres_RemoveByName]
	@Name NVARCHAR(100)
AS
BEGIN
	DELETE FROM [Genres]
	WHERE [Name] = @Name
END
GO

--Artists
CREATE PROCEDURE [Artists_Insert]
	@ArtistID NVARCHAR(50),
	@Name NVARCHAR(100)
AS
BEGIN
	INSERT INTO Artists([ArtistID],[Name])
	VALUES (@ArtistID,@Name)
END
GO

CREATE PROCEDURE [Artists_ReadAll]
AS
BEGIN
	SELECT * FROM [Artists]
END
GO

CREATE PROCEDURE [Artists_ReadByID]
	@ArtistID NVARCHAR(50)
AS
BEGIN
	SELECT *
	FROM [Artists]
	WHERE [ArtistID] = @ArtistID
END
GO

CREATE PROCEDURE [Artists_ReadByName]
	@Name NVARCHAR(50)
AS
BEGIN
	SELECT TOP 1 [ArtistID],[Name]
	FROM [Artists]
	WHERE [Name] = @Name 
END
GO

CREATE PROCEDURE [Artists_Update]
	@ArtistID UNIQUEIDENTIFIER,
	@Name NVARCHAR(100)
AS
BEGIN
	UPDATE [Artists]
	SET [Name]=@Name
	WHERE [ArtistID] = @ArtistID
END
GO

CREATE PROCEDURE [Artists_Remove]
	@ArtistID UNIQUEIDENTIFIER
AS
BEGIN
	DELETE FROM [Artists]
	WHERE [ArtistID] = @ArtistID
END
GO

CREATE PROCEDURE [Artists_RemoveByName]
	@Name NVARCHAR(100)
AS
BEGIN
	DELETE FROM [Artists]
	WHERE [Name] = @Name
END
GO

--UsersGenres
CREATE PROCEDURE [UsersGenres_Insert]
	@UserID UNIQUEIDENTIFIER,
	@GenreID UNIQUEIDENTIFIER
AS
BEGIN
	INSERT INTO UsersGenres([UserID],[GenreID])
	VALUES (@UserID,@GenreID)
END
GO

CREATE PROCEDURE [UsersGenres_ReadAll]
AS
BEGIN
	SELECT * FROM [UsersGenres]
END
GO

CREATE PROCEDURE [UsersGenres_ReadByUserID]
	@UserID UNIQUEIDENTIFIER
AS
BEGIN
	SELECT g.[Name]
	FROM [UsersGenres] ug
	INNER JOIN [Genres] g ON ug.GenreID = g.GenreID
	WHERE ug.[UserID] = @UserID
END
GO

CREATE PROCEDURE [UsersGenres_Remove]
	@UserID UNIQUEIDENTIFIER,
	@GenreID UNIQUEIDENTIFIER
AS
BEGIN
	DELETE FROM [UsersGenres]
	WHERE [UserID] = @UserID AND [GenreID] = @GenreID
END
GO

CREATE PROCEDURE [UsersGenres_RemoveByUserID]
	@UserID UNIQUEIDENTIFIER
AS
BEGIN
	DELETE FROM [UsersGenres]
	WHERE [UserID] = @UserID
END
GO

--UsersArtists
CREATE PROCEDURE [UsersArtists_Insert]
	@UserID UNIQUEIDENTIFIER,
	@ArtistID NVARCHAR(50)
AS
BEGIN
	INSERT INTO UsersArtists([UserID],[ArtistID])
	VALUES (@UserID,@ArtistID)
END
GO

CREATE PROCEDURE [UsersArtists_ReadAll]
AS
BEGIN
	SELECT * FROM [UsersArtists]
END
GO

CREATE PROCEDURE [UsersArtists_ReadByUserID]
	@UserID UNIQUEIDENTIFIER
AS
BEGIN
	SELECT ua.[ArtistID],a.[Name]
	FROM [UsersArtists] ua
	INNER JOIN [Artists] a ON ua.ArtistID = a.ArtistID
	WHERE ua.[UserID] = @UserID
END
GO

CREATE PROCEDURE [UsersArtists_Remove]
	@UserID UNIQUEIDENTIFIER,
	@ArtistID NVARCHAR(50)
AS
BEGIN
	DELETE FROM [UsersArtists]
	WHERE [UserID] = @UserID AND [ArtistID] = @ArtistID
END
GO

CREATE PROCEDURE [UsersArtists_RemoveByUserID]
	@UserID UNIQUEIDENTIFIER
AS
BEGIN
	DELETE FROM [UsersArtists]
	WHERE [UserID] = @UserID
END
GO

CREATE PROCEDURE [GetRandomSeeds]
	@UserID UNIQUEIDENTIFIER
AS
BEGIN
	DECLARE @genresTable as table([Name] NVARCHAR(100) NOT NULL);
	INSERT INTO @genresTable exec [UsersGenres_ReadByUserID] @UserID

	DECLARE @artistsTable as table([ArtistID] NVARCHAR(50) NOT NULL, [Name] NVARCHAR(100) NOT NULL);
	INSERT INTO @artistsTable exec [UsersArtists_ReadByUserID] @UserID

	SELECT TOP 5 *
	FROM
	(
		SELECT [ArtistID],[Name] from @artistsTable
		UNION 
		SELECT NULL as [ArtistID],[Name] from @genresTable
	) seeds
	ORDER BY NEWID()
END
GO


CREATE PROCEDURE [GetRandomGenreSeeds]
	@UserID UNIQUEIDENTIFIER
AS
BEGIN
	DECLARE @genresTable as table([Name] NVARCHAR(100) NOT NULL);
	INSERT INTO @genresTable exec [UsersGenres_ReadByUserID] @UserID

	SELECT TOP 5 *
	FROM (SELECT [Name] from @genresTable) gt
	ORDER BY NEWID()
END
GO