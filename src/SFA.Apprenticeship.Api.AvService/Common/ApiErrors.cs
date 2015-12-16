namespace SFA.Apprenticeship.Api.AvService.Common
{
    using DataContracts.Version51;

    public static class ApiErrors
    {
        public static ErrorCodesData Error10001 => new ErrorCodesData
        {
            InterfaceErrorCode = -10001,
            InterfaceErrorDescription = "\"WorkingWeek\" must be 100 characters or less"
        };

        public static ErrorCodesData Error10002 => new ErrorCodesData
        {
            InterfaceErrorCode = -10002,
            InterfaceErrorDescription = "\"WorkingWeek\" is mandatory"
        };

        public static ErrorCodesData Error10003 => new ErrorCodesData
        {
            InterfaceErrorCode = -10003,
            InterfaceErrorDescription = "\"WeeklyWage\" should be atleast £40"
        };

        public static ErrorCodesData Error10004 => new ErrorCodesData
        {
            InterfaceErrorCode = -10004,
            InterfaceErrorDescription = "\"WeeklyWage\" is mandatory"
        };

        public static ErrorCodesData Error10005 => new ErrorCodesData
        {
            InterfaceErrorCode = -10005,
            InterfaceErrorDescription = "\"VacancyType\" is mandatory"
        };

        public static ErrorCodesData VacancyTitleIsTooLong => new ErrorCodesData
        {
            InterfaceErrorCode = -10006,
            InterfaceErrorDescription = "\"Title\" must be 200 characters or less"
        };

        public static ErrorCodesData VacancyTitleIsMandatory => new ErrorCodesData
        {
            InterfaceErrorCode = -10007,
            InterfaceErrorDescription = "\"Title\" is mandatory"
        };

        public static ErrorCodesData Error10008 => new ErrorCodesData
        {
            InterfaceErrorCode = -10008,
            InterfaceErrorDescription = "\"PossibleStartDate\" is mandatory"
        };

        public static ErrorCodesData VacancyShortDescriptionIsTooLong => new ErrorCodesData
        {
            InterfaceErrorCode = -10009,
            InterfaceErrorDescription = "\"ShortDescription\" must be 512 characters or less"
        };

        public static ErrorCodesData VacancyShortDescriptionIsMandatory => new ErrorCodesData
        {
            InterfaceErrorCode = -10010,
            InterfaceErrorDescription = "\"ShortDescription\" is mandatory"
        };

        public static ErrorCodesData Error10011 => new ErrorCodesData
        {
            InterfaceErrorCode = -10011,
            InterfaceErrorDescription = "\"NumberOfPositions\" is mandatory for standard vacancies"
        };

        public static ErrorCodesData Error10012 => new ErrorCodesData
        {
            InterfaceErrorCode = -10012,
            InterfaceErrorDescription = "\"LearningProviderEdsUrn\" is mandatory"
        };

        public static ErrorCodesData Error10013 => new ErrorCodesData
        {
            InterfaceErrorCode = -10013,
            InterfaceErrorDescription = "\"InterviewStartDate\" is mandatory"
        };

        public static ErrorCodesData VacancyLongDescriptionIsMandatory => new ErrorCodesData
        {
            InterfaceErrorCode = -10014,
            InterfaceErrorDescription = "\"LongDescription\" is mandatory"
        };

        public static ErrorCodesData Error10015 => new ErrorCodesData
        {
            InterfaceErrorCode = -10015,
            InterfaceErrorDescription = "\"ApprenticeshipFramework\" is mandatory"
        };

        public static ErrorCodesData Error10016 => new ErrorCodesData
        {
            InterfaceErrorCode = -10016,
            InterfaceErrorDescription = "\"ApprenticeshipFramework\" must be 3 characters"
        };

        public static ErrorCodesData Error10017 => new ErrorCodesData
        {
            InterfaceErrorCode = -10017,
            InterfaceErrorDescription = "\"EmployerWebsite\" must be 512 characters or less"
        };

        public static ErrorCodesData Error10018 => new ErrorCodesData
        {
            InterfaceErrorCode = -10018,
            InterfaceErrorDescription = "\"EmployerExternalApplicationWebsite\" must be 512 characters or less"
        };

        public static ErrorCodesData Error10019 => new ErrorCodesData
        {
            InterfaceErrorCode = -10019,
            InterfaceErrorDescription = "\"EmployerEdsUrn\" is mandatory"
        };

        public static ErrorCodesData Error10020 => new ErrorCodesData
        {
            InterfaceErrorCode = -10020,
            InterfaceErrorDescription = "\"EmployerDescription\" must be 8000 characters or less"
        };

        public static ErrorCodesData Error10021 => new ErrorCodesData
        {
            InterfaceErrorCode = -10021,
            InterfaceErrorDescription = "\"EmployerAnonymousName\" must be 510 characters or less"
        };

        public static ErrorCodesData Error10022 => new ErrorCodesData
        {
            InterfaceErrorCode = -10022,
            InterfaceErrorDescription = "\"ContactName\" must be 200 characters or less"
        };

        public static ErrorCodesData Error10023 => new ErrorCodesData
        {
            InterfaceErrorCode = -10023,
            InterfaceErrorDescription = "\"ClosingDate\" is mandatory"
        };

        public static ErrorCodesData Error10024 => new ErrorCodesData
        {
            InterfaceErrorCode = -10024,
            InterfaceErrorDescription = "\"ApplicationInstructions\" must be 8000 characters or less"
        };

        public static ErrorCodesData Error10025 => new ErrorCodesData
        {
            InterfaceErrorCode = -10025,
            InterfaceErrorDescription = "\"NumberOfVacancies\" is mandatory for multi - site vacancies"
        };

        public static ErrorCodesData Error10026 => new ErrorCodesData
        {
            InterfaceErrorCode = -10026,
            InterfaceErrorDescription = "\"ClosingDate\" is Invalid"
        };

        public static ErrorCodesData Error10027 => new ErrorCodesData
        {
            InterfaceErrorCode = -10027,
            InterfaceErrorDescription = "\"EmployerImage\" size must be less than 10K"
        };

        public static ErrorCodesData VacancyLongDescriptionIsTooLong => new ErrorCodesData
        {
            InterfaceErrorCode = -10028,
            InterfaceErrorDescription = "\"LongDescription\" must be 2147483648 characters or less"
        };

        public static ErrorCodesData Error10029 => new ErrorCodesData
        {
            InterfaceErrorCode = -10029,
            InterfaceErrorDescription = "\"InterviewStartDate\" is Invalid"
        };

        public static ErrorCodesData Error10030 => new ErrorCodesData
        {
            InterfaceErrorCode = -10030,
            InterfaceErrorDescription = "\"PossibleStartDate\" is Invalid"
        };

        public static ErrorCodesData Error10031 => new ErrorCodesData
        {
            InterfaceErrorCode = -10031,
            InterfaceErrorDescription = "\"EmployerExternalApplicationWebsite \" in mandatory for offline vacancies"
        };

        public static ErrorCodesData Error10032 => new ErrorCodesData
        {
            InterfaceErrorCode = -10032,
            InterfaceErrorDescription = "\"EmployerDescription\" is mandatory for anonymous employer"
        };

        public static ErrorCodesData Error10033 => new ErrorCodesData
        {
            InterfaceErrorCode = -10033,
            InterfaceErrorDescription = "Learning Provider is not authorised for this vacancy"
        };

        public static ErrorCodesData Error10034 => new ErrorCodesData
        {
            InterfaceErrorCode = -10034,
            InterfaceErrorDescription = "Vacancy reference number already exists"
        };

        public static ErrorCodesData Error10035 => new ErrorCodesData
        {
            InterfaceErrorCode = -10035,
            InterfaceErrorDescription = "Invalid relationship for training provider and employer"
        };
        public static ErrorCodesData Error10036 => new ErrorCodesData
        {
            InterfaceErrorCode = -10036,
            InterfaceErrorDescription = "\"ApprenticeshipFramework\" is invalid"
        };

        public static ErrorCodesData Error10037 => new ErrorCodesData
        {
            InterfaceErrorCode = -10037,
            InterfaceErrorDescription = "\"County\" for standard location is invalid"
        };

        public static ErrorCodesData Error10038 => new ErrorCodesData
        {
            InterfaceErrorCode = -10038,
            InterfaceErrorDescription = "\"County\" for multiple location is invalid"
        };

        public static ErrorCodesData Error10039 => new ErrorCodesData
        {
            InterfaceErrorCode = -10039,
            InterfaceErrorDescription = "\"County\" for standard location is mandatory"
        };

        public static ErrorCodesData Error10040 => new ErrorCodesData
        {
            InterfaceErrorCode = -10040,
            InterfaceErrorDescription = "\"County\" for multiple location is mandatory"
        };

        public static ErrorCodesData Error10041 => new ErrorCodesData
        {
            InterfaceErrorCode = -10041,
            InterfaceErrorDescription = "\"AddressLine1\" for standard location is mandatory"
        };

        public static ErrorCodesData Error10042 => new ErrorCodesData
        {
            InterfaceErrorCode = -10042,
            InterfaceErrorDescription = "\"AddressLine1\" for multiple location is mandatory"
        };

        public static ErrorCodesData Error10043 => new ErrorCodesData
        {
            InterfaceErrorCode = -10043,
            InterfaceErrorDescription = "\"EmployerImage\" is not valid."
        };

        public static ErrorCodesData Error10044 => new ErrorCodesData
        {
            InterfaceErrorCode = -10044,
            InterfaceErrorDescription = "Entered Training Provider and Employer cannot have national vacancy"
        };

        public static ErrorCodesData Error10045 => new ErrorCodesData
        {
            InterfaceErrorCode = -10045,
            InterfaceErrorDescription = "\"EmployerEdsUrn\" is invalid"
        };

        public static ErrorCodesData Error10046 => new ErrorCodesData
        {
            InterfaceErrorCode = -10046,
            InterfaceErrorDescription = "\"LearningProviderEdsUrn\" is invalid"
        };

        public static ErrorCodesData Error10047 => new ErrorCodesData
        {
            InterfaceErrorCode = -10047,
            InterfaceErrorDescription = "\"PostCode\" is mandatory for standard vacancies"
        };

        public static ErrorCodesData Error10048 => new ErrorCodesData
        {
            InterfaceErrorCode = -10048,
            InterfaceErrorDescription = "\"PostCode\" is mandatory for multisite vacancies"
        };

        public static ErrorCodesData Error10049 => new ErrorCodesData
        {
            InterfaceErrorCode = -10049,
            InterfaceErrorDescription = "\"PostCode\" is invalid for standard vacancy"
        };

        public static ErrorCodesData Error10050 => new ErrorCodesData
        {
            InterfaceErrorCode = -10050,
            InterfaceErrorDescription = "\"PostCode\" is invalid for multisite vacancy"
        };

        public static ErrorCodesData Error1 => new ErrorCodesData
        {
            InterfaceErrorCode = -1,
            InterfaceErrorDescription = "Unknown System Error"
        };

        public static ErrorCodesData Error20001 => new ErrorCodesData
        {
            InterfaceErrorCode = -20001,
            InterfaceErrorDescription = "Unknown Vacancy Reference"
        };

        public static ErrorCodesData Error20002 => new ErrorCodesData
        {
            InterfaceErrorCode = -20002,
            InterfaceErrorDescription = "InvalidVacancyReference"
        };

        public static ErrorCodesData Error20003 => new ErrorCodesData
        {
            InterfaceErrorCode = -20003,
            InterfaceErrorDescription = "Invalid Update Value"
        };

        public static ErrorCodesData Error20004 => new ErrorCodesData
        {
            InterfaceErrorCode = -20004,
            InterfaceErrorDescription = "You cannot record this number of candidates as successful as the total number of successes is greater than the number of vacancies available for this advert.Either the number of successful candidates reported is incorrect or the number of vacancies for this advert needs to be increased."
        };

        public static ErrorCodesData Error20005 => new ErrorCodesData
        {
            InterfaceErrorCode = -20005,
            InterfaceErrorDescription = "Updates Not Allowed"
        };

        public static ErrorCodesData Error10051 => new ErrorCodesData
        {
            InterfaceErrorCode = -10051,
            InterfaceErrorDescription = "Unsupported HTML Tags"
        };

        public static ErrorCodesData Error10052 => new ErrorCodesData
        {
            InterfaceErrorCode = -10052,
            InterfaceErrorDescription = "\"DisplayRecruitmentAgency\" is mandatory"
        };

        public static ErrorCodesData Error10053 => new ErrorCodesData
        {
            InterfaceErrorCode = -10053,
            InterfaceErrorDescription = "\"SmallEmployerWageIncentive\" is mandatory"
        };

        public static ErrorCodesData Error10054 => new ErrorCodesData
        {
            InterfaceErrorCode = -10054,
            InterfaceErrorDescription = "\"DeliveryOrganisation\" does not exist"
        };

        public static ErrorCodesData Error10055 => new ErrorCodesData
        {
            InterfaceErrorCode = -10055,
            InterfaceErrorDescription = "\"VacancyManager\" does not exist"
        };

        public static ErrorCodesData Error10056 => new ErrorCodesData
        {
            InterfaceErrorCode = -10056,
            InterfaceErrorDescription = "\"ContractOwner\" is not authorised for this vacancy"
        };

        public static ErrorCodesData Error10057 => new ErrorCodesData
        {
            InterfaceErrorCode = -10057,
            InterfaceErrorDescription = "\"VacancyManager\" is not authorised for this vacancy"
        };

        public static ErrorCodesData Error10058 => new ErrorCodesData
        {
            InterfaceErrorCode = -10058,
            InterfaceErrorDescription = "\"DeliveryOrganisation\" is not authorised for this vacancy"
        };

        public static ErrorCodesData Error10059 => new ErrorCodesData
        {
            InterfaceErrorCode = -10059,
            InterfaceErrorDescription = "\"ContractOwnerUKPRN\" is mandatory"
        };

        public static ErrorCodesData DeliveryProviderEdsUrnIsMandatory => new ErrorCodesData
        {
            InterfaceErrorCode = -10060,
            InterfaceErrorDescription = "\"DeliveryProviderEdsUrn\" is mandatory"
        };

        public static ErrorCodesData VacancyManagerEdsUrnIsMandatory => new ErrorCodesData
        {
            InterfaceErrorCode = -10061,
            InterfaceErrorDescription = "\"VacancyManagerEdsUrn\" is mandatory"
        };

        public static ErrorCodesData VacancyOwnerEdsUrnIsMandatory => new ErrorCodesData
        {
            InterfaceErrorCode = -10062,
            InterfaceErrorDescription = "\"VacancyOwnerEdsUrn\" is mandatory"
        };

        public static ErrorCodesData Error10063 => new ErrorCodesData
        {
            InterfaceErrorCode = -10063,
            InterfaceErrorDescription = "\"LocalAuthority\" does not exist"
        };

        public static ErrorCodesData Error10064 => new ErrorCodesData
        {
            InterfaceErrorCode = -10064,
            InterfaceErrorDescription = "\"Address\" is mandatory"
        };

        public static ErrorCodesData Error10065 => new ErrorCodesData
        {
            InterfaceErrorCode = -10065,
            InterfaceErrorDescription = "\"SiteVacancyDetails\" is mandatory"
        };

        public static ErrorCodesData Error10066 => new ErrorCodesData
        {
            InterfaceErrorCode = -10066,
            InterfaceErrorDescription = "\"WageType\" is invalid"
        };

        public static ErrorCodesData Error10067 => new ErrorCodesData
        {
            InterfaceErrorCode = -10067,
            InterfaceErrorDescription = "\"PostCode\" does not exist"
        };

        public static ErrorCodesData Error10068 => new ErrorCodesData
        {
            InterfaceErrorCode = -10068,
            InterfaceErrorDescription = "\"WeeklyWage\" must be £0 for a traineeship"
        };
    }
}
