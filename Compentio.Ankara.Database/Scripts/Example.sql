CREATE OR ALTER PROCEDURE SP_Example
(
    @UserName NVARCHAR(200) = NULL
)
AS
BEGIN
  SELECT * FROM [$(ScriptParameter)].[dbo].[VW_USERS]	
END
