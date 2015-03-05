namespace SFA.Apprenticeships.Web.ContactForms.Mediators
{
    using ContactForms.Constants;

    public class MediatorResponseMessage
    {
        public string Text { get; set; }

        public UserMessageLevel Level { get; set; }
    }
}