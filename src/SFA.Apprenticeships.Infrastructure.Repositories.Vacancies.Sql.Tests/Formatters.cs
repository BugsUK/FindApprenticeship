namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Tests
{
    using System;

    public interface IObjectFormatter
    {
        string Format(object value);
    }

    public class StringFormatter : IObjectFormatter
    {
        public string Format(object value)
        {
            return $"'{value}'";
        }
    }

    public class DateTimeFormatter : IObjectFormatter
    {
        public string Format(object value)
        {
            return $"'{((DateTime) value).ToString("O")}'";
        }
    }

    public class DefaultFormatter : IObjectFormatter
    {
        public string Format(object value)
        {
            return value.ToString();
        }
    }

    public class NullableFormatter : IObjectFormatter
    {
        public string Format(object value)
        {
            return InstanceFormatter.FormatTypeInstance(value, value.GetType());
        }
    }

    public static class InstanceFormatter
    {
        private static readonly Type DefaultFormatterType = typeof (DefaultFormatter);

        public static string FormatTypeInstance(object value, Type type)
        {
            var typeName = type.Name.Contains("Nullable") ? "Nullable" : type.Name;

            var formatterName = "SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Tests." + typeName +
                                "Formatter";

            var formatterType = Type.GetType(formatterName) ?? DefaultFormatterType;
            
            var instance = Activator.CreateInstance(formatterType);
            var returnValue = formatterType.GetMethod("Format").Invoke(instance, new[] {value});
            return returnValue.ToString();
        }
    }
}