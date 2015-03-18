namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using System.Configuration;
    using Common.Configuration;
    using SFA.Apprenticeships.Common.AppSettings;

    public class SendGridConfiguration : SecureConfigurationSection<SendGridConfiguration>
    {
        private const string UserNameConstant = "UserName";
        private const string PasswordConstant = "Password";

        public SendGridConfiguration()
            : base("SendGridConfiguration")
        {
        }

        [ConfigurationProperty(UserNameConstant, IsRequired = true)]
        public string UserName
        {
            get { return (string)this[UserNameConstant]; }
            set { this[UserNameConstant] = value; }
        }

        [ConfigurationProperty(PasswordConstant, IsRequired = true)]
        public string Password
        {
            get { return (string)this[PasswordConstant]; }
            set { this[PasswordConstant] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(SendGridTemplateConfigurationCollection), AddItemName = "Template")]
        public SendGridTemplateConfigurationCollection Templates
        {
            get { return (SendGridTemplateConfigurationCollection)this[""]; }
        }
        //private const string UserNameConstant = "NetworkUserName";
        //private const string PasswordConstant = "NetworkPassword";

        //public SendGridConfiguration()
        //    : base("SendGridConfiguration")
        //{
        //}
                
        //public string UserName
        //{
        //    get { return BaseAppSettingValues.NetworkUsername; }
        //}
                
        //public string Password
        //{
        //    get { return BaseAppSettingValues.NetworkPassword;}            
        //}       

        //[ConfigurationProperty("", IsDefaultCollection = true)]
        //[ConfigurationCollection(typeof(SendGridTemplateConfigurationCollection), AddItemName = "Template")]
        //public SendGridTemplateConfigurationCollection Templates
        //{
        //    get { return (SendGridTemplateConfigurationCollection)this[""]; }
        //}
    }
}
