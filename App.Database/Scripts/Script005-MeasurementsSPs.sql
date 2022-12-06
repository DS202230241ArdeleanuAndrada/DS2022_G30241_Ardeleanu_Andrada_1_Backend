ALTER PROCEDURE [dbo].[SP_User_GetRole] 
	@Username		NVARCHAR(100),
	@Password		NVARCHAR(100)
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
	SET NOCOUNT ON;
	
	SELECT 
		u.Id,
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceMeasurements](
	[Id] int IDENTITY(1,1) NOT NULL,
	[DeviceId] int NOT NULL,
	[Timestamp] datetime2(7) NOT NULL,
	[MeasurementValue] float NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Device_GetMeasurements]
	@DeviceId int
AS

	SET NOCOUNT ON;
	
	SELECT 
		*
	FROM dbo.[DeviceMeasurements] where DeviceId = @DeviceId

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Device_AddMeasurement] 
	@DeviceId	INT,
	@Timestamp	Datetime2(7),
	@MeasurementValue	FLOAT
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
	SET NOCOUNT ON;
	
	INSERT INTO [dbo].[DeviceMeasurements]
           ([DeviceId]
           ,[Timestamp]
           ,[MeasurementValue])
     VALUES
           (@DeviceId, @Timestamp, @MeasurementValue)

COMMIT TRAN

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Device_GetById]
	@DeviceId int
AS

	SET NOCOUNT ON;
	
	SELECT 
		d.*,
		u.Name as 'NameOfUser',
		u.Username
	FROM dbo.[Device] d
		INNER JOIN dbo.DeviceUser du on du.DeviceId = d.Id
		INNER JOIN dbo.[User] u on u.Id = du.UserId
	where d.Id = @DeviceId

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Device_GetHourlyMeasurements]
	@DeviceId int
AS

	SET NOCOUNT ON;
	
	SELECT 
		*
	FROM dbo.[DeviceMeasurements] 
	where DeviceId = @DeviceId
	and Timestamp > DATEADD(HOUR, -1, GETDATE())