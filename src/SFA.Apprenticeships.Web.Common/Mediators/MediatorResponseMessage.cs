using SFA.Apprenticeships.Web.Common.Constants;

namespace SFA.Apprenticeships.Web.Common.Mediators
{
    public class MediatorResponseMessage
    {
        public string Text { get; set; }

        public UserMessageLevel Level { get; set; }
    }
}