INSERT INTO [dbo].[Role]
           ([Name])
     VALUES
           ('admin'),
		   ('user1'),
		   ('user2')
GO

INSERT INTO [dbo].[User]
           ([Name]
           ,[Username]
           ,[Password])
     VALUES
           ('AdminName','admin','admin'),
		   ('BasicUserName 1','user1','user1'),
		   ('BasicUserName 2','user2','user2')

GO

INSERT INTO [dbo].[Device]
           ([Name]
           ,[Description]
           ,[Address]
           ,[MaxConsumption])
     VALUES
           ('Device1Name','Device 1 description', 'Device 1 address', '100'),
		   ('Device2Name','Device 2 description', 'Device 2 address', '100'),
           ('Device3Name','Device 3 description', 'Device 3 address', '100'),
           ('Device4Name','Device 4 description', 'Device 4 address', '100'),
           ('Device5Name','Device 5 description', 'Device 5 address', '100')

GO

INSERT INTO [dbo].[UserRole]
           ([UserId]
           ,[RoleId])
     VALUES
           (1,1),
		   (2,2),
		   (3,2)

GO

INSERT INTO [dbo].[DeviceUser]
           ([UserId]
           ,[DeviceId])
     VALUES
           (2,1),
		   (2,2),
		   (3,3),
		   (3,4),
		   (3,5)


