namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System.Collections.Generic;
    using System.Configuration;

    public class SmsTemplateConfigurationCollection : ConfigurationElementCollection, IEnumerable<SmsTemplateConfiguration>
    {
        private readonly List<SmsTemplateConfiguration> _elements;
        private readonly Dictionary<string, SmsTemplateConfiguration> _elementDictionary;

        public SmsTemplateConfigurationCollection()
        {
            _elements = new List<SmsTemplateConfiguration>();
            _elementDictionary = new Dictionary<string, SmsTemplateConfiguration>();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var element = new SmsTemplateConfiguration();

            _elements.Add(element);

            return element;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var key = ((SmsTemplateConfiguration)element).Name;

            if (!_elementDictionary.ContainsKey(key))
            {
                _elementDictionary.Add(key, ((SmsTemplateConfiguration)element));
            }

            return _elementDictionary[key];
        }

        public new IEnumerator<SmsTemplateConfiguration> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
}
