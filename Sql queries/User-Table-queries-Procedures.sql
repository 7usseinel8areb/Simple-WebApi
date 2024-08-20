Create table [User]
(
	Id int not null primary key identity,
	[Name] nvarchar(100)  not null,
	[Password]  nvarchar(100) not null
);
Go

Create TAble UserPermissions(

	UserId int not null references [User](Id) on delete Cascade,
	PermissionId int not null

	Primary Key(UserId,PermissionId)

);
Go

--Insert into [User] values('Hussein','abc123')
--Insert into UserPermissions values(1,1),(1,2)

Create procedure GetUserByUserNameAndPassword(
	@UserName nvarchar(100),
	@Password nvarchar(100)
)
as 
Begin
	Select top 1 * from [User]
	where [Name] = @UserName and [Password] = @Password
End;
Go



Create procedure CheckUserPermission(
	@UserId int,
	@PermissionId int
)
as 
Begin
	 select CASE 
        WHEN EXISTS (
            SELECT 1
            FROM UserPermissions
            WHERE UserId = @UserId
              AND PermissionId = @PermissionId
        ) THEN 1
        ELSE 0
		End;
End;
Go

--CheckUserPermission 1,3