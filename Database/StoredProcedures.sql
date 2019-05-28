USE EmotionBasedMusicPlayer
GO

--Users
CREATE PROCEDURE [Users_Insert]
	@Username NVARCHAR(20),
	@Email NVARCHAR(50),
	@Password NVARCHAR(max)
AS
BEGIN
	INSERT INTO Users([Username],[Email],[Password])
	VALUES (@Username,@Email,@Password)
END
GO

CREATE PROCEDURE [Users_ReadByUsernameAndEmail]
	@Username NVARCHAR(20),
	@Email NVARCHAR(50)
AS
BEGIN
	SELECT *
	FROM [Users]
	WHERE [Username] = @Username OR [Email] = @Email
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

CREATE PROCEDURE [Users_ReadByID]
	@Username NVARCHAR(20)
AS
BEGIN
	SELECT *
	FROM [Users]
	WHERE [Username] = @Username
END
GO

CREATE PROCEDURE [Users_Update]
	@Username NVARCHAR(20),
	@Email NVARCHAR(50),
	@Password NVARCHAR(max)
AS
BEGIN
	UPDATE [Users]
	SET [Email]=@Email,[Password] = @Password
	WHERE [Username] = @Username
END
GO

CREATE PROCEDURE [Users_Remove]
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
	@Username NVARCHAR(20),
	@GenreID UNIQUEIDENTIFIER
AS
BEGIN
	INSERT INTO UsersGenres([Username],[GenreID])
	VALUES (@Username,@GenreID)
END
GO

CREATE PROCEDURE [UsersGenres_ReadAll]
AS
BEGIN
	SELECT * FROM [UsersGenres]
END
GO

CREATE PROCEDURE [UsersGenres_ReadByUsername]
	@Username NVARCHAR(50)
AS
BEGIN
	SELECT *
	FROM [UsersGenres]
	WHERE [Username] = @Username
END
GO

CREATE PROCEDURE [UsersGenres_Remove]
	@Username NVARCHAR(20),
	@GenreID UNIQUEIDENTIFIER
AS
BEGIN
	DELETE FROM [UsersGenres]
	WHERE [Username] = @Username AND [GenreID] = @GenreID
END
GO

CREATE PROCEDURE [UsersGenres_RemoveByUsername]
	@Username NVARCHAR(100)
AS
BEGIN
	DELETE FROM [UsersGenres]
	WHERE [Username] = @Username
END
GO

--UsersArtists
CREATE PROCEDURE [UsersArtists_Insert]
	@Username NVARCHAR(20),
	@ArtistID NVARCHAR(50)
AS
BEGIN
	INSERT INTO UsersArtists([Username],[ArtistID])
	VALUES (@Username,@ArtistID)
END
GO

CREATE PROCEDURE [UsersArtists_ReadAll]
AS
BEGIN
	SELECT * FROM [UsersArtists]
END
GO

CREATE PROCEDURE [UsersArtists_ReadByUsername]
	@Username NVARCHAR(50)
AS
BEGIN
	SELECT *
	FROM [UsersArtists]
	WHERE [Username] = @Username
END
GO

CREATE PROCEDURE [UsersArtists_Remove]
	@Username NVARCHAR(20),
	@ArtistID NVARCHAR(50)
AS
BEGIN
	DELETE FROM [UsersArtists]
	WHERE [Username] = @Username AND [ArtistID] = @ArtistID
END
GO

CREATE PROCEDURE [UsersArtists_RemoveByUsername]
	@Username NVARCHAR(100)
AS
BEGIN
	DELETE FROM [UsersArtists]
	WHERE [Username] = @Username
END
GO