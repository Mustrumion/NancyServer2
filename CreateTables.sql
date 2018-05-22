USE HobbyDatabase

CREATE TABLE dbo.Users
(
	ID INT IDENTITY PRIMARY KEY,
	email VARCHAR(256) NOT NULL,
	[password] VARBINARY(1024) NOT NULL
)
GO

CREATE INDEX usersEmail ON dbo.Users(email)
GO

CREATE TABLE dbo.Tokens
(
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	userID INT NOT NULL,
	expiration DateTime NOT NULL
)
GO
