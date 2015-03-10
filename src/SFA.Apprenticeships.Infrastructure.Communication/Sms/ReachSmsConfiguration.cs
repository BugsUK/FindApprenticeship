namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System.Collections.Generic;
    using System.Configuration;
    using Common.Configuration;

    public class ReachSmsConfiguration : SecureConfigurationSection<ReachSmsConfiguration>, IReachSmsConfiguration
    {
        private const string UsernameConstant = "Username";
        private const string PasswordConstant = "Password";
        private const string OriginatorConstant = "Originator";
        private const string UrlConstant = "Url";
        private const string CallbackUrlConstant = "CallbackUrl";

        public ReachSmsConfiguration()
            : base("ReachSmsConfiguration")
        {
        }

        [ConfigurationProperty(UsernameConstant, IsRequired = true)]
        public string Username
        {
            get { return (string)this[UsernameConstant]; }
            set { this[UsernameConstant] = value; }
        }

        [ConfigurationProperty(PasswordConstant, IsRequired = true)]
        public string Password
        {
            get { return (string)this[PasswordConstant]; }
            set { this[PasswordConstant] = value; }
        }

        [ConfigurationProperty(OriginatorConstant, IsRequired = true)]
        public string Originator
        {
            get { return (string)this[OriginatorConstant]; }
            set { this[OriginatorConstant] = value; }
        }

        [ConfigurationProperty(UrlConstant, IsRequired = true)]
        public string Url
        {
            get { return (string)this[UrlConstant]; }
            set { this[UrlConstant] = value; }
        }

        [ConfigurationProperty(CallbackUrlConstant, IsRequired = true)]
        public string CallbackUrl
        {
            get { return (string)this[CallbackUrlConstant]; }
            set { this[UrlConstant] = value; }
        }

        public IEnumerable<SmsTemplateConfiguration> Templates
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
