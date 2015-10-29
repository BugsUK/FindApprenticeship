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
    }
}