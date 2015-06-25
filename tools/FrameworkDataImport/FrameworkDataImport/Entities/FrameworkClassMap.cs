namespace FrameworkDataImport.Entities
{
    using System;
    using System.Linq;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;

    public sealed class FrameworkClassMap : CsvClassMap<Framework>
    {
        public FrameworkClassMap()
        {
            Map(m => m.Name).Index(0);
            Map(m => m.Number).Index(1);
            Map(m => m.IssuingAuthority).Index(2);
            Map(m => m.Sector).Index(3).TypeConverter<SectorConverter>();
            Map(m => m.Levels).Index(7).TypeConverter<LevelsConverter>();
        }
    }

    public class LevelsConverter : ITypeConverter
    {
        public string ConvertToString(TypeConverterOptions options, object value)
        {
            throw new NotImplementedException();
        }

        public object ConvertFromString(TypeConverterOptions options, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new int[0];
            }

            return
                text.Split(',')
                    .Where(i => !string.IsNullOrEmpty(i))
                    .Select(i => Convert.ToInt32(i.Replace("L", "")))
                    .ToArray();
        }

        public bool CanConvertFrom(Type type)
        {
            return true;
        }

        public bool CanConvertTo(Type type)
        {
            throw new NotImplementedException();
        }
    }

    public class SectorConverter : ITypeConverter
    {
        public string ConvertToString(TypeConverterOptions options, object value)
        {
            throw new NotImplementedException();
        }

        public object ConvertFromString(TypeConverterOptions options, string text)
        {
            return text.Replace("&", "and").Replace(", and", " and").Replace("Business Administration", "Business, Administration");
        }

        public bool CanConvertFrom(Type type)
        {
            return true;
        }

        public bool CanConvertTo(Type type)
        {
            throw new NotImplementedException();
        }
    }
}