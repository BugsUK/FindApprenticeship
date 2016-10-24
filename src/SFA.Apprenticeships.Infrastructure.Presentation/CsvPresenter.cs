namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using CsvHelper;
    using CsvHelper.Configuration;

    public static class CsvPresenter
    {
        public static string ToCsv<T, TClassMap>(IEnumerable<T> objectlist) where TClassMap : CsvClassMap<T>
        {
            var writer = new StringWriter();
            var csv = new CsvWriter(writer);
            csv.Configuration.CultureInfo = CultureInfo.GetCultureInfo("en-GB");
            csv.Configuration.UseExcelLeadingZerosFormatForNumerics = true;
            csv.Configuration.RegisterClassMap<TClassMap>();
            csv.WriteRecords(objectlist);
            return writer.ToString();
        }
    }
}