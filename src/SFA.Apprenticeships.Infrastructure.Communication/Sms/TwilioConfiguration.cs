namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System.Collections.Generic;
    using System.Configuration;
    using Common.Configuration;
    using Configuration;

    public class TwilioConfiguration : SecureConfigurationSection<TwilioConfiguration>, ITwillioConfiguration
    {
        private const string AccountSidConstant = "AccountSid";
        private const string AuthTokenConstant = "AuthToken";
        private const string MobileNumberFromConstant = "MobileNumberFrom";

        public TwilioConfiguration()
            : base("TwilioConfiguration")
        {
        }

        [ConfigurationProperty(AccountSidConstant, IsRequired = true)]
        public string AccountSid
        {
            get { return (string)this[AccountSidConstant]; }
            set { this[AccountSidConstant] = value; }
        }

        [ConfigurationProperty(AuthTokenConstant, IsRequired = true)]
        public string AuthToken
        {
            get { return (string)this[AuthTokenConstant]; }
            set { this[AuthTokenConstant] = value; }
        }

        [ConfigurationProperty(MobileNumberFromConstant, IsRequired = true)]
        public string MobileNumberFrom
        {
            get { return (string)this[MobileNumberFromConstant]; }
            set { this[MobileNumberFromConstant] = value; }
        }

        public IEnumerable<SmsTemplate> Templates
        {
            get { return TemplateCollection; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(SmsTemplateConfigurationCollection), AddItemName = "Template")]
        public SmsTemplateConfigurationCollection TemplateCollection
        {
            get { return (SmsTemplateConfigurationCollection)this[""]; }
        }
    }
}
