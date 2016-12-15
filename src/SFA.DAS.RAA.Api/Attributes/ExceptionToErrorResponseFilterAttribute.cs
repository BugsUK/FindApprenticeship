namespace SFA.DAS.RAA.Api.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Security;
    using System.Web.Http;
    using System.Web.Http.Filters;
    using FluentValidation;
    using FluentValidation.Results;
    using FluentValidation.WebApi;

    public class ExceptionToErrorResponseFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ArgumentException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(context.Exception.Message));
            }

            if (context.Exception is KeyNotFoundException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError(context.Exception.Message));
            }

            if (context.Exception is SecurityException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpError(context.Exception.Message));
            }

            var validationException = context.Exception as ValidationException;
            if (validationException != null)
            {
                new ValidationResult(validationException.Errors).AddToModelState(context.ActionContext.ModelState, null);

                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, context.ActionContext.ModelState);
            }

            base.OnException(context);
        }
    }
}