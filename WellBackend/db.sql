CREATE DATABASE [Well];
GO

USE [Well];
GO

CREATE TABLE [Users] (
	[Id] bigint NOT NULL,
    [Email] nvarchar(50) NOT NULL,
    [Password] nvarchar(50) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Surname] nvarchar(50) NOT NULL,
    [Birthday] date NOT NULL,
    [Country] nvarchar(50),
    [City] nvarchar(50),
    [Gender] nvarchar(10) NOT NULL,
    [ProfilePicture] nvarchar(50),
    [JoinDate] date NOT NULL,

	CONSTRAINT PkUserId PRIMARY KEY (Id)
);

INSERT INTO [Users] VALUES (1, 'asd@a.com', 'pass', 'Carlos', 'Collado Caballero', '1996/02/19', null, null, 'Masculino', null, '2000/02/10');
INSERT INTO [Users] VALUES (2, 'zxc@z.com', 'word', 'Jose Manuel', 'Rufete Gomez', '1996/11/15', null, null, 'Masculino', null, '2000/02/11');

CREATE TABLE [Photos] (
	[Id] bigint NOT NULL,
    [FileName] nvarchar(50) NOT NULL,
    [UploadDate] date NOT NULL,
	[Likes] bigint NOT NULL,
    [UserId] bigint NOT NULL,

	CONSTRAINT PkPhotoId PRIMARY KEY (Id),
	CONSTRAINT FkUserPhotoId FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE [Comments] (
	[Id] bigint NOT NULL,
    [Text] text NOT NULL,
    [PublishDate] date NOT NULL,
    [Likes] bigint NOT NULL,
    [UserId] bigint,
    [PhotoId] bigint,
    [CommentId] bigint,

	CONSTRAINT PkCommentId PRIMARY KEY (Id),
	CONSTRAINT FkUserCommentId FOREIGN KEY (UserId) REFERENCES Users(Id),
	CONSTRAINT FkPhotoCommentId FOREIGN KEY (PhotoId) REFERENCES Photos(Id),
	CONSTRAINT FkCommentCommentId FOREIGN KEY (CommentId) REFERENCES Comments(Id)
);

CREATE TABLE Friendships (
	[Id] bigint NOT NULL,
	[UserId1] bigint NOT NULL,
	[Userid2] bigint NOT NULL,
	[FriendsSince] date NOT NULL,

	CONSTRAINT PkFriendshipId PRIMARY KEY (Id),
	CONSTRAINT FkUser1 FOREIGN KEY (UserId1) REFERENCES Users(Id),
	CONSTRAINT FkUser2 FOREIGN KEY (UserId2) REFERENCES Users(Id)
);
