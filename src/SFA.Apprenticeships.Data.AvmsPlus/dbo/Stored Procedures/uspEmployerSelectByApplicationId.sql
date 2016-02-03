CREATE PROCEDURE [dbo].[uspEmployerSelectByApplicationId]
	@applicationId int = 0
	
AS
 BEGIN      
 
   SET NOCOUNT ON      
   
		BEGIN TRY    
 
			SELECT
				EMP.EMPLOYERID,
				EMP.FULLNAME,
				EMP.TRADINGNAME,
        VC.TITLE
			FROM
				EMPLOYER EMP
			INNER JOIN [VacancyOwnerRelationship] VPR ON
				VPR.EMPLOYERID = EMP.EMPLOYERID		
			INNER JOIN 	VACANCY VC ON
				VC.[VacancyOwnerRelationshipId] = VPR.[VacancyOwnerRelationshipId]
			 INNER JOIN APPLICATION APP ON
				APP.VACANCYID = VC.VACANCYID AND APP.APPLICATIONID =  @applicationId



	    END TRY      
      
	BEGIN CATCH      
		EXEC RethrowError;      
	END CATCH       
      
   SET NOCOUNT OFF      
END