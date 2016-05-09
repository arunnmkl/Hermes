using AmadeusConsulting.Simplex.Base.Serialization;
using AmadeusConsulting.Simplex.Security;

namespace Hermes.Authentication
{
	public static class UserGroupController
	{
		public static int CreateUser(string username = "unittest", string password = "unittestpassword", bool addGroup = false, bool createResource = false)
		{
			var userId = AuthenticationCommands.CreateSecurityUser(username, "unittest@test.com");
			AuthenticationCommands.SetPassword(userId, password);

			if (addGroup)
				AddUserToGroup(userId);

			if (createResource)
				CreateLogicalResource();

			return userId;
		}

		public static void AddUserToGroup(int userId, string groupName = "UnitTest Group")
		{
			var groupId = AuthenticationCommands.CreateSecurityGroup(groupName);
			AuthenticationCommands.AddUserToGroup(userId, groupId);
		}

		public static void CreateLogicalResource(string resourceName = "Access This", string groupName = "UnitTest Group")
		{
			string sql = string.Format(@"				

				DECLARE @RID uniqueidentifier,
				@Name varchar(50), -- name of the resource
				@GroupName varchar(50)

				SET @RID = NEWID()
				SET @Name = '{0}'
				SET @GroupName = '{1}'

				-- Create logical resource
				IF NOT EXISTS(SELECT RID FROM acgsec.LogicalResource WHERE Name = @Name)
				BEGIN
					INSERT INTO [acgsec].[LogicalResource]
					([RID]
					,[Name]
					,[Created])
					VALUES
					(@RID
					,@Name
					,GETDATE())
				END

				DECLARE @ResourceID uniqueidentifier
				DECLARE @SID uniqueidentifier
				DECLARE @PermissionLevel int
				SET @PermissionLevel = -2 -- Read

				IF NOT EXISTS(SELECT 1 FROM [acgsec].[Permission] P WHERE P.PermissionId = @PermissionLevel)
				BEGIN
					INSERT [acgsec].[Permission] ([PermissionId], [Name]) VALUES (@PermissionLevel, N'Read')
				END

				SET @ResourceID = (SELECT [RID] FROM [acgsec].[LogicalResource] WHERE Name = @Name)

				IF NOT EXISTS(Select GroupID from acgsec.[Group] where Name like @GroupName)
				BEGIN
					INSERT INTO [acgsec].[Group]
					([Name])
					VALUES
					(@GroupName)
				END

				SET @SID = (SELECT [SID] from acgsec.[Group] WHERE Name = @GroupName)

				IF  NOT EXISTS(SELECT RID FROM acgsec.ACL WHERE RID=@ResourceID AND [SID]=@SID)
				BEGIN
					INSERT acgsec.ACL ([RID], [SID]) VALUES
					(@ResourceID, @SID)

					INSERT acgsec.AccessRule (ACLID, PermissionId, [Deny])
					SELECT ACL.ACLID
					, @PermissionLevel
					, 0
					FROM acgsec.ACL ACL
					WHERE ACL.RID = @ResourceID AND
					ACL.[SID] = @SID
				END
			", resourceName, groupName);

			try
			{
				SqlSerializer.Default.Execute(sql);
			}
			catch (System.Exception)
			{
			}
		}

		public static void ClearTestUserData(string username = "unittest", string groupName = "UnitTest Group")
		{
			SqlSerializer.Default.Execute(
			 string.Format(@"
				DELETE acgsec.[UserGroup] FROM acgsec.[UserGroup] UG INNER JOIN acgsec.[User] U ON UG.UserId = U.UserId WHERE U.[Username] = '{0}'
				DELETE acgsec.[UserPassword] FROM acgsec.[UserPassword] P INNER JOIN acgsec.[User] U ON P.UserId = U.UserId WHERE U.[Username] = '{0}'
				DELETE acgsec.[UserWindowsAccount] FROM acgsec.[UserWindowsAccount] W INNER JOIN acgsec.[User] U ON W.UserId = U.UserId WHERE U.[Username] = '{0}'
				DELETE acgsec.[User] WHERE [Username] = '{0}'

				DELETE acgsec.[Group] WHERE [Name] = '{1}'
			", username, groupName));
		}
	}
}