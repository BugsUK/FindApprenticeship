using System.Collections.Generic;
using SFA.Apprenticeships.Infrastructure.Postcode.Entities;

namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    public interface IFindPostcodeService
    {
        IEnumerable<PostcodeSearchInfo> FindPostcodes(string postcode);
    }
}