﻿namespace SFA.Apprenticeships.Web.ContactForms.Mediators
{
    using FluentValidation.Results;

    public class MediatorResponse
    {
        public string Code { get; set; }

        public MediatorResponseMessage Message { get; set; }

        public object Parameters { get; set; }

        public ValidationResult ValidationResult { get; set; }
    }


    public class MediatorResponse<T> : MediatorResponse
    {
        public T ViewModel { get; set; }
    }
}