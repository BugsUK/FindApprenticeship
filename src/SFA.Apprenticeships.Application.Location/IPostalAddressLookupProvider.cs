using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Application.Location
{
    using Domain.Entities.Locations;

    public interface IPostalAddressLookupProvider
    {
        IEnumerable<PostalAddress> GetValidatedPostalAddresses(string addressLine1, string postcode);
    }
}
