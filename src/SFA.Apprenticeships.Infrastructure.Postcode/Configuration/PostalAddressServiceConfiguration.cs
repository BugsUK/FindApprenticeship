using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Infrastructure.Postcode.Configuration
{
    public class PostalAddressServiceConfiguration
    {
        public string FindByPartsEndpoint { get; set; }

        public string RetrieveByIdEndpoint { get; set; }

        public string Key { get; set; }
    }
}
