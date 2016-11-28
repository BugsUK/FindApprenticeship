namespace SFA.DAS.RAA.Api.Handlers
{
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensions;

    public class ApiKeyHandler : DelegatingHandler
    {
        //set a default API key 
        private const string ApiKeyQueryStringParam = "api_key";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string apiKeyValue;
            
            //Validate that the api key exists
            if (request.TryGetQueryStringValue(ApiKeyQueryStringParam, out apiKeyValue))
            {
                var userIdClaim = new Claim(ClaimTypes.SerialNumber, apiKeyValue);
                var identity = new ClaimsIdentity(new[] { userIdClaim }, "ApiKey");
                var principal = new ClaimsPrincipal(identity);

                Thread.CurrentPrincipal = principal;
            }

            //Allow the request to process further down the pipeline
            var response = await base.SendAsync(request, cancellationToken);

            //Return the response back up the chain
            return response;

            //If the key is missing, return an http status code.
            //return request.CreateResponse(HttpStatusCode.Forbidden, "Missing API Key");
        }
    }
}