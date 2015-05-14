namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System;

    public class ReachSmsNumberFormatter : ISmsNumberFormatter
    {
        private const string CountryCode = "44";

        public string Format(string smsNumber)
        {
            if (string.IsNullOrWhiteSpace(smsNumber))
            {
                throw new ArgumentNullException("smsNumber");
            }

            var formattedSmsNumber = smsNumber
                .Replace(" ", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace("+", string.Empty);

            formattedSmsNumber = formattedSmsNumber.TrimStart(new[] { '0' });

            if (formattedSmsNumber.StartsWith(CountryCode))
            {
                formattedSmsNumber = formattedSmsNumber.Substring(CountryCode.Length);
            }

            formattedSmsNumber = formattedSmsNumber.TrimStart(new[] { '0' });

            if (!formattedSmsNumber.StartsWith(CountryCode))
            {
                formattedSmsNumber = CountryCode + formattedSmsNumber;
            }

            return formattedSmsNumber;
        }
    }
}