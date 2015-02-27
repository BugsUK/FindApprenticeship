namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;

    public interface IQueueWarningLimits
    {
        int DefaultLimit { get; }

        IList<IQueueWarningLimit> ToList();
    }

    public class QueueWarningLimitCollection : ConfigurationElementCollection, IQueueWarningLimits
    {
        private const string DefaultLimitConst = "DefaultLimit";

        private readonly IList<IQueueWarningLimit> _elements = new List<IQueueWarningLimit>();
            
        [ConfigurationProperty(DefaultLimitConst, IsRequired = true)]
        public int DefaultLimit
        {
            get { return (int)base[DefaultLimitConst]; }
            set { base[DefaultLimitConst] = value; }
        }

        public IList<IQueueWarningLimit> ToList()
        {
            return _elements;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var element = new QueueWarningLimitConfiguration();
            _elements.Add(element);
            return element;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var key = ((IQueueWarningLimit)element).NameEndsWith;
            return key;
        }
    }
}