namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;

    public static class CsvPresenter
    {
        private static readonly string _separator = ",";

        public static string ToCsv<T>(IEnumerable<T> objectlist)
        {
            Type t = typeof (T);
            FieldInfo[] fields = t.GetFields();

            string header = String.Join(_separator, fields.Select(f => f.Name).ToArray());

            StringBuilder csvdata = new StringBuilder();
            csvdata.AppendLine(header);

            foreach (var o in objectlist)
                csvdata.AppendLine(ToCsvFields(fields, o));

            return csvdata.ToString();
        }

        private static string ToCsvFields(FieldInfo[] fields, object o)
        {
            StringBuilder linie = new StringBuilder();

            foreach (var f in fields)
            {
                if (linie.Length > 0)
                    linie.Append(_separator);

                var x = f.GetValue(o);

                if (x != null)
                    linie.Append("\"x.ToString()\"");
            }

            return linie.ToString();
        }
    }
}