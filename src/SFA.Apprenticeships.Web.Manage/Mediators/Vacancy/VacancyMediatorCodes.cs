﻿namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    public class VacancyMediatorCodes
    {
        public class ApproveVacancy
        {
            public const string NoAvailableVacancies = "VacancyMediatorCodes.ApproveVacancy.NoAvailableVacancies";
            public const string Ok = "VacancyMediatorCodes.ApproveVacancy.Ok";
        }

        public class RejectVacancy
        {
            public const string NoAvailableVacancies = "VacancyMediatorCodes.RejectVacancy.NoAvailableVacancies";
            public const string Ok = "VacancyMediatorCodes.RejectVacancy.Ok";
        }

        public class GetVacancy
        {
            public const string NotAvailable = "VacancyMediatorCodes.GetVacancy.NotAvailable";
            public const string Ok = "VacancyMediatorCodes.GetVacancy.Ok";
            public const string FailedValidation = "VacancyMediatorCodes.GetVacancy.FailedValidation";
        }

        public class GetVacancySummaryViewModel
        {
            public const string FailedValidation = "VacancyMediatorCodes.GetVacancySummaryViewModel.FailedValidation";
            public const string Ok = "VacancyMediatorCodes.GetVacancySummaryViewModel.Ok";
        }
        
        public class UpdateVacancy
        {
            public const string FailedValidation = "VacancyMediatorCodes.UpdateVacancy.FailedValidation";
            public const string Ok = "VacancyMediatorCodes.UpdateVacancy.Ok";
        }
        
        public class GetBasicVacancyDetails
        {
            public const string Ok = "VacancyMediatorCodes.GetBasicVacancyDetails.Ok";
            public const string FailedValidation = "VacancyMediatorCodes.GetBasicVacancyDetails.FailedValidation";
        }

        public class GetVacancyRequirementsProspectsViewModel
        {
            public const string FailedValidation = "VacancyMediatorCodes.GetVacancyRequirementsProspectsViewModel.FailedValidation";
            public const string Ok = "VacancyMediatorCodes.GetVacancyRequirementsProspectsViewModel.Ok";
        }

        public class GetVacancyQuestionsViewModel
        {
            public const string FailedValidation = "VacancyMediatorCodes.GetVacancyQuestionsViewModel.FailedValidation";
            public const string Ok = "VacancyMediatorCodes.GetVacancyQuestionsViewModel.Ok";
        }

        public class GetEmployerInformation
        {
            public const string FailedValidation = "VacancyMediatorCodes.GetEmployerInformation.FailedValidation";
            public const string Ok = "VacancyMediatorCodes.GetEmployerInformation.Ok";
        }

        public class UpdateEmployerInformation
        {
            public const string FailedValidation = "VacancyMediatorCodes.UpdateEmployerInformation.FailedValidation";
            public const string Ok = "VacancyMediatorCodes.UpdateEmployerInformation.Ok";
        }

        public class GetLocationAddressesViewModel
        {
            public const string FailedValidation = "VacancyMediatorCodes.GetLocationAddressesViewModel.FailedValidation";
            public const string Ok = "VacancyMediatorCodes.GetLocationAddressesViewModel.Ok";
        }

        public class AddLocations
        {
            public const string FailedValidation = "VacancyMediatorCodes.AddLocations.FailedValidation";
            public const string Ok = "VacancyMediatorCodes.AddLocations.Ok";
        }

        public class SearchLocations
        {
            public const string NotFullPostcode = "VacancyMediatorCodes.SearchLocations.NoFullPostcode";
            public const string Ok = "VacancyMediatorCodes.SearchLocations.Ok";
        }

        public class UseLocation
        {
            public const string Ok = "VacancyMediatorCodes.SearchLocations.Ok";
        }

        public class RemoveLocation
        {
            public const string Ok = "VacancyMediatorCodes.SearchLocations.Ok";
        }
    }
}