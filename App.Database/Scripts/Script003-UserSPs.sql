CREATE PROCEDURE [dbo].[SP_User_GetRole] 
	@Username		NVARCHAR(100),
	@Password		NVARCHAR(100)
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
	SET NOCOUNT ON;
	
	SELECT 
		u.Username, 
		u.Name as 'User',
		r.Name as 'Role'
	FROM dbo.[User] u 
		INNER JOIN dbo.UserRole ur 
			ON ur.UserId = u.Id
		INNER JOIN dbo.[Role] r 
			ON ur.RoleId = r.Id
	WHERE u.Username = @Username 
		AND u.Password = @Password

COMMIT TRAN

GO

CREATE PROCEDURE [dbo].[SP_User_GetDevices] 
	@UserId		int
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
	SET NOCOUNT ON;
	
	SELECT 
		d.*
	FROM dbo.[User] u 
		INNER JOIN dbo.DeviceUser du 
			ON du.UserId = u.Id
		INNER JOIN dbo.[Device] d 
			ON du.DeviceId = d.Id
	WHERE u.Id = @UserId

COMMIT TRAN

GO

CREATE PROCEDURE [dbo].[SP_User_AssignDevice] 
	@UserId		int,
	@DeviceId		int

AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
	SET NOCOUNT ON;
	
	INSERT INTO [dbo].[DeviceUser]
    VALUES (@UserId,@DeviceId)

	SELECT ISNULL(CAST(SCOPE_IDENTITY() as int), -1)
COMMIT TRAN

GO

CREATE PROCEDURE [dbo].[SP_User_RemoveDevice] 
	@UserId		int,
	@DeviceId		int

AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
	SET NOCOUNT ON;
	
	DELETE [dbo].[DeviceUser]
    WHERE UserId = @UserId and DeviceId = @DeviceId

COMMIT TRAN

GO

CREATE PROCEDURE [dbo].[SP_User_Create] 
	@Name		NVARCHAR(100),
	@Username	NVARCHAR(100),
	@Password	NVARCHAR(100)
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
	SET NOCOUNT ON;
	
	INSERT INTO [dbo].[User]
           ([Name]
           ,[Username]
           ,[Password])
     VALUES
           (@Name, @Username, @Password)

	SELECT ISNULL(CAST(SCOPE_IDENTITY() as int), -1)
COMMIT TRAN

GO

CREATE PROCEDURE [dbo].[SP_User_Delete] 
	@Id		int
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
	SET NOCOUNT ON;
	
	DELETE dbo.UserRole where UserId = @Id
	DELETE dbo.DeviceUser where UserId = @Id
	DELETE dbo.[User] where Id = @Id


COMMIT TRAN