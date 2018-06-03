USE HobbyDatabase

DROP TABLE dbo.UserProfiles
GO

DROP TABLE dbo.Photos
GO


DROP TABLE dbo.Tokens
GO


DROP TABLE dbo.Users
GO


CREATE TABLE dbo.Users
(
	ID INT IDENTITY PRIMARY KEY,
	email VARCHAR(256) NOT NULL,
	[password] VARBINARY(1024) NOT NULL,
	CONSTRAINT uniqueEmail UNIQUE(email)   
)
GO

CREATE INDEX usersEmail ON dbo.Users(email)
GO


CREATE TABLE dbo.Tokens
(
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	userID INT NOT NULL,
	expiration DateTime NOT NULL,
    FOREIGN KEY (userID) REFERENCES Users(ID)
)
GO

CREATE TABLE dbo.Photos
(
    [guid] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL UNIQUE,
	photo VARBINARY(MAX) NOT NULL
)
GO


CREATE TABLE dbo.UserProfiles
(
	ID INT IDENTITY PRIMARY KEY,
	userID INT NOT NULL,
	nick VARCHAR(256) NOT NULL,
	name VARCHAR(512) NULL,
	nameVisible BIT,
	surname VARCHAR(512) NULL,
	surnameVisible BIT,
	description VARCHAR(4096) NULL,
	descriptionVisible BIT,
	gender VARCHAR(512) NULL,
	genderVisible BIT,
	interests VARCHAR(512) NULL,
	interestsVisible BIT,
	born DATE NULL,
	ageVisible BIT,
	photoGuid UNIQUEIDENTIFIER NULL,
	CONSTRAINT uniqueUserID UNIQUE(userID),
    FOREIGN KEY (photoGuid) REFERENCES Photos(guid)
)

CREATE INDEX profileUserID ON dbo.UserProfiles(userID)
GO