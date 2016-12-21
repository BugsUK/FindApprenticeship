namespace SFA.DAS.RAA.Api.AcceptanceTests.Constants
{
    public class UriFormats
    {
        public const string VacancyIdUriFormat = "/vacancy/?vacancyId={0}";
        public const string VacancyReferenceNumberUriFormat = "/vacancy/?vacancyReferenceNumber={0}";
        public const string VacancyGuidUriFormat = "/vacancy/?vacancyGuid={0}";

        public const string EditWageVacancyIdUriFormat = "/vacancy/wage/?vacancyId={0}";

        public const string LinkEmployerUri = "/employer/link";
        public const string LinkEmployerEdsUrnUriFormat = LinkEmployerUri + "?edsUrn={0}";
    }
}