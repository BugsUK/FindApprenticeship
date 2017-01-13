namespace SFA.DAS.RAA.Api.AcceptanceTests.Models
{
    using System.Collections.Generic;
    using System.Web.Http.ModelBinding;

    public class ResponseMessage
    {
        public string Message { get; set; }

        public IDictionary<string, IList<string>> ModelState { get; set; }
    }
}