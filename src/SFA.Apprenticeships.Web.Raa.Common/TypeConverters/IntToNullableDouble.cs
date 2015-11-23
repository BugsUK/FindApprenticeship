namespace SFA.Apprenticeships.Web.Raa.Common.TypeConverters
{
    using System;
    using System.ComponentModel;

    public static class IntToNullableDouble
    {
        public static double? ToNullableDouble(this int? i)
        {
            double? result = null;

            try
            {
                TypeConverter conv = TypeDescriptor.GetConverter(typeof(double?));
                result = (double?)conv.ConvertFrom(i);
            }
            catch { }

            return result;
        }
    }
}
