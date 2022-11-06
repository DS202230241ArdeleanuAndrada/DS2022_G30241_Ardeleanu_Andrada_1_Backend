CREATE PROCEDURE [dbo].[SP_Device_Create] 
	@Name		NVARCHAR(100),
	@Description	NVARCHAR(100),
	@Address	NVARCHAR(100),
	@MaxConsumption int
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
	SET NOCOUNT ON;
	
	INSERT INTO [dbo].[Device]
           ([Name]
           ,[Description]
           ,[Address]
		   ,[MaxConsumption])
     VALUES
           (@Name, @Description, @Address, @MaxConsumption)

	SELECT ISNULL(CAST(SCOPE_IDENTITY() as int), -1)
COMMIT TRAN

GO

CREATE PROCEDURE [dbo].[SP_Device_Delete] 
	@Id		int
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
	SET NOCOUNT ON;
	
	DELETE dbo.DeviceUser where DeviceId = @Id	
	DELETE dbo.[device] where Id = @Id


COMMIT TRAN

GO

CREATE PROCEDURE [dbo].[SP_Device_GetAllDevices]
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
	SET NOCOUNT ON;
	
	SELECT 
		*
	FROM dbo.[Device]

COMMIT TRAN

GO

CREATE PROCEDURE [dbo].[SP_Device_Update] 
	@Id int,
	@Name nvarchar(100),
	@Description nvarchar(100),
	@Address nvarchar(100),
	@MaxConsumption nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @deviceId int;
	SET @deviceId=(SELECT Id from [Device] where Id=@Id)

	IF(@deviceId IS NOT NULL)
	BEGIN
	UPDATE [dbo].[Device]
		SET Name=@Name, Description=@Description, Address=@Address, MaxConsumption=@MaxConsumption
	WHERE Id=@Id
	END
	ELSE
		SELECT -1;
	SELECT @Id;
END

GO

