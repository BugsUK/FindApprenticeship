namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System;

    public class SmsNumberFormatter
    {
        private const string CountryCode = "44";

        public string Format(string smsNumber)
        {
            if (string.IsNullOrWhiteSpace(smsNumber))
            {
                throw new ArgumentNullException("smsNumber");
            }

            var formattedSmsNumber = smsNumber.Trim().TrimStart(new[] { '0', '+' });

            if (!formattedSmsNumber.StartsWith(CountryCode))
            {
                formattedSmsNumber = CountryCode + formattedSmsNumber;
            }

            return formattedSmsNumber;
        }
    }
}
