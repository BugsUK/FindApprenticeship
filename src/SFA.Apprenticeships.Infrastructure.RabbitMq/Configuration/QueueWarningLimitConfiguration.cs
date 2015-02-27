namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Configuration
{
    using System.Configuration;

    public interface IQueueWarningLimit
    {
        string NameEndsWith { get; set; }

        int Limit { get; set; }
    }

    public class QueueWarningLimitConfiguration : ConfigurationElement, IQueueWarningLimit
    {
        private const string NameConst = "NameEndsWith";
        private const string LimitConst = "Limit";

        [ConfigurationProperty(NameConst, IsRequired = true)]
        public string NameEndsWith
        {
            get { return (string)this[NameConst]; }
            set { this[NameConst] = value; }
        }

        [ConfigurationProperty(LimitConst, IsRequired = true)]
        public int Limit
        {
            get { return (int)this[LimitConst]; }
            set { this[LimitConst] = value; }
        }
    }
}