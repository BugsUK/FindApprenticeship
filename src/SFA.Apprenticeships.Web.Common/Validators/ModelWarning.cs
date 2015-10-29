namespace SFA.Apprenticeships.Web.Common.Validators
{
    using System;
    using System.Web.Mvc;

    public class ModelWarning : ModelError
    {
        public ModelWarning(Exception exception) : base(exception)
        {
        }

        public ModelWarning(Exception exception, string errorMessage) : base(exception, errorMessage)
        {
        }

        public ModelWarning(string errorMessage) : base(errorMessage)
        {
        }
    }
}