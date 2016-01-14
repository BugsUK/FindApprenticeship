using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Application.Location
{
    using Domain.Entities.Locations;
    using Interfaces.Locations;
    public class PostalAddressSearchService : IPostalAddressSearchService
    {
        public PostalAddress GetAddress(string fullPostcode, string addressLine1)
        {
            throw new NotImplementedException();
        }
    }
}
