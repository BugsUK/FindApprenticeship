namespace SFA.Apprenticeship.Api.AvService.ErrorHandlers
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

    public class ErrorHandlerBase
    {
        protected ErrorHandlerBase(string policyName)
        {
            if (policyName == null)
            {
                throw new ArgumentNullException(nameof(policyName));
            }

            PolicyName = policyName;
        }
        public string PolicyName { get; }

        protected bool HandleException(Exception e)
        {
            try
            {
                return ExceptionPolicy.HandleException(e, PolicyName);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
