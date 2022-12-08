﻿CREATE PROCEDURE [eliasm_db].[UserRolDelete]
	@UserId nvarchar(450),
	@RoleId nvarchar(450)
AS
BEGIN TRY
	SET NOCOUNT ON;
	
	BEGIN TRAN
			DELETE AspNetUserRoles
			WHERE UserId = @UserId
			AND RoleId = @RoleId

	COMMIT;

	SELECT 1 Id

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 
		ROLLBACK;

	DECLARE @ErrorMessage NVARCHAR(4000);  
	DECLARE @ErrorSeverity INT;  
	DECLARE @ErrorState INT;  

	SELECT @ErrorMessage = ERROR_MESSAGE(),@ErrorSeverity = ERROR_SEVERITY(),@ErrorState = ERROR_STATE();  

	RAISERROR (@ErrorMessage, -- Message text.  
				@ErrorSeverity, -- Severity.  
				@ErrorState -- State.  
			);  
END CATCH