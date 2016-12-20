namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.Constants
{
    public class WageUpdateMessages
    {
        public const string CannotEditTraineeshipWage = "You can only edit the wage of an Apprenticeship vacancy.";
        public const string CannotEditNonLiveWage = "You can only edit the wage of a vacancy that is live or closed.";
        public const string MissingHoursPerWeek = "You can only edit the wage of a vacancy that has a valid value for hours per week.";

        public class Type
        {
            public const string CannotChangeLegacyTextType = "You cannot change the type of a LegacyText wage.";
            public const string InvalidLegacyWeeklyTypeChange = "You can only change the type of a LegacyWeekly wage to Custom (fixed) or CustomRange (wage range).";
            public const string InvalidApprenticeshipMinimumTypeChange = "You can only change the type of an ApprenticeshipMinimum wage to NationalMinimum, Custom (fixed) or CustomRange (wage range).";
            public const string InvalidNationalMinimumTypeChange = "You can only change the type of a NationalMinimum wage to Custom (fixed) or CustomRange (wage range).";
            public const string InvalidCustomTypeChange = "You can only change the type of a Custom (fixed) wage to CustomRange (wage range).";
            public const string InvalidCustomRangeTypeChange = "You can only change the type of a CustomRange (wage range) wage to Custom (fixed).";
            public const string InvalidCompetitiveSalaryTypeChange = "You cannot change the type of a CompetitiveSalary wage.";
            public const string InvalidToBeAgreedUponAppointmentTypeChange = "You cannot change the type of a ToBeAgreedUponAppointment wage.";
            public const string InvalidUnwagedTypeChange = "You can only change the type of an Unwaged wage to ApprenticeshipMinimum, NationalMinimum, Custom (fixed) or CustomRange (wage range).";
        }

        public class Amount
        {
            public const string InvalidCustomAmount = "The new fixed wage must be higher than the original figure.";
            public const string InvalidCustomRangeAmount = "The new fixed wage must be higher than the orignal wage range minimum.";
            public const string MissingCustomAmount = "You must specify a valid amount.";
            public const string InvalidCustomAmountNationalMinimumWage = "The new fixed wage must be higher than £{0:N2}.";
            public const string InvalidCustomAmountApprenticeMinimumWage = "The new fixed wage must be higher than £{0:N2}.";
        }

        public class AmountLowerBound
        {
            public const string InvalidCustomAmountLowerBound = "The minimum amount must be higher than the original amount.";
            public const string InvalidCustomRangeAmountLowerBound = "The minimum amount must be higher than the original fixed wage.";
            public const string MissingCustomAmountLowerBound = "You must specify a valid minimum amount for the wage range.";
            public const string InvalidCustomAmountLowerBoundNationalMinimumWage = "The minimum amount must be higher than £{0:N2}.";
            public const string InvalidCustomAmountLowerBoundApprenticeMinimumWage = "The minimum amount must be higher than £{0:N2}.";
        }

        public class AmountUpperBound
        {
            public const string MissingCustomAmountUpperBound = "You must specify a valid maximum amount for the wage range.";
            public const string AmountUpperBoundShouldBeGreaterThanAmountLowerBound = "The maximum amount for the wage range must be higher than the minimum amount.";
        }

        public class Unit
        {
            public const string MissingUnit = "You must specify a valid wage unit.";
        }
    }
}