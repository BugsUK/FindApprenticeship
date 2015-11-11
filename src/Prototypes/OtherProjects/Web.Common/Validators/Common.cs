namespace SFA.Apprenticeships.Web.Common.Validators
{
    using System;
    using ViewModels;

    public class Common
    {
        public static bool BeValidDate(DateViewModel instance)
        {
            return BeValidDate(instance, null);
        }

        public static bool BeValidDate(DateViewModel instance, int? day)
        {
            if (instance == null) return false;
            try
            {
                var date = instance.Date;
                return date != DateTime.MinValue;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        public static bool BeTwoWeeksInTheFuture(DateViewModel instance)
        {
            return BeInTheFuture(instance, 14);
        }

        public static bool BeInTheFuture(DateViewModel instance, int daysInFuture)
        {
            //We don't have a value for date yet so assume it's in the future
            if(instance == null || !instance.HasValue) return true;

            return instance.Date > DateTime.Today.AddDays(daysInFuture);
        }

        public static bool BeAfter(DateViewModel instance, DateViewModel comparison)
        {
            throw new NotImplementedException();
        }
    }
}