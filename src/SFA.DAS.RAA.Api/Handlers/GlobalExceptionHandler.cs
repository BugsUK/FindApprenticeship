namespace SFA.DAS.RAA.Api.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Security;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;

    public class GlobalExceptionHandler : ExceptionHandler
    {
        /*public override void Handle(ExceptionHandlerContext context)
        {
            var argumentException = context.Exception as ArgumentException;
            if (argumentException != null)
            {
                context.Result = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(argumentException.Message));
            }

            var httpResponseException = context.Exception as HttpResponseException;
            if (httpResponseException != null)
            {
                context.Request.CreateErrorResponse(httpResponseException.Response.StatusCode, new HttpError(httpResponseException.Message));
            }

            base.Handle(context);
        }*/

        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            if (context.Exception is ArgumentException)
            {
                context.Result = new ErrorResponseActionResult(context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(context.Exception.Message)));
            }

            if (context.Exception is KeyNotFoundException)
            {
                context.Result = new ErrorResponseActionResult(context.Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError(context.Exception.Message)));
            }

            if (context.Exception is SecurityException)
            {
                context.Result = new ErrorResponseActionResult(context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpError(context.Exception.Message)));
            }

            return base.HandleAsync(context, cancellationToken);
        }

        private class ErrorResponseActionResult : IHttpActionResult
        {
            private readonly HttpResponseMessage _responseMessage;

            public ErrorResponseActionResult(HttpResponseMessage responseMessage)
            {
                _responseMessage = responseMessage;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_responseMessage);
            }
        }
    }
}