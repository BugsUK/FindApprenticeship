namespace SFA.Apprenticeships.Application.Location
{
    using System.Collections.Generic;
    using Domain.Entities.Locations;

    public interface IPostcodeLookupProvider
    {
        Location GetLocation(string postcode);
    }
}
