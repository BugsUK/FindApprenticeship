namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using System.Configuration;
    using Common.Configuration;
    using SFA.Apprenticeships.Common.AppSettings;

    public class SendGridConfiguration : SecureConfigurationSection<SendGridConfiguration>
    {
        private const string UserNameConstant = "NetworkUsername";
        private const string PasswordConstant = "NetworkPassword";

        public SendGridConfiguration()
            : base("SendGridConfiguration")
        {
        }
                
        public string UserName
        {
            get { return BaseAppSettingValues.NetworkUsername; }
        }
                
        public string Password
        {
            get { return BaseAppSettingValues.NetworkPassword; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(SendGridTemplateConfigurationCollection), AddItemName = "Template")]
        public SendGridTemplateConfigurationCollection Templates
        {
            get { return (SendGridTemplateConfigurationCollection)this[""]; }
        }        
    }
}
