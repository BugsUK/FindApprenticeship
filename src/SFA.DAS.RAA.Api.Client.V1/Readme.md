Open a command window targetting this project's directory and enter:

"..\packages\AutoRest.0.17.3\tools\AutoRest.exe" -Input "http://local-restapi.findapprenticeship.service.gov.uk/swagger/docs/v1" -Namespace SFA.DAS.RAA.Api.Client.V1 -OutputDirectory . -ClientName ApiClient -AddCredentials true

This should be rerun every time the API is updated