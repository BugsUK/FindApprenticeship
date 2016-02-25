/*----------------------------------------------------------------------                  
Name  : ReportGetSchoolsByName                  
Description :  returns ordered unique school name 
                
History:                  
--------                  
Date                                      Version                                Author                                  Comment
======================================================================
16-FEB-2012                       1.0                                         Sanjeev Kumar

---------------------------------------------------------------------- */                 

CREATE procedure [dbo].[ReportGetSchoolsByName]
@schoolName varchar(500)
as

                SET NOCOUNT ON  
                SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

set @schoolName=ltrim(rtrim(@schoolName))


    BEGIN TRY 
                                if  len(@schoolName)=0 
                                begin
                                                select 'All' as SchoolName, -1 as SchoolId
                                end
                                else
                                begin
                                                                SELECT  SchoolName+'  (' + Address + ')' SchoolName, 
                                                                                                SchoolId
                                                                FROM 
                                                                                                dbo.School
                                                                WHERE                                                 
                                                                                                SchoolName like @schoolName+'%'

                                                              
                                                ORDER BY
                                                                SchoolName
                                end
--                             if  len(@schoolName)=0 
--                             begin
--                             select 'All' as SchoolName, -1 as SchoolId
--                             end
--
--                             else
--                             begin
--                             SELECT  SchoolName, 
--                                                             SchoolId
--                                                             --,Address
--                             FROM 
--                                                             dbo.School
--                             WHERE
--                                                             
--                                                             SchoolName like @schoolName+'%'
--                             ORDER BY
--                                             SchoolName
--                             end
END TRY  
                BEGIN CATCH  
                                EXEC RethrowError
                END CATCH  
      
    SET NOCOUNT OFF