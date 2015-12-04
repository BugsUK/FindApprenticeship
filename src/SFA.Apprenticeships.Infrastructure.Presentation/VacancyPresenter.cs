namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    public static class VacancyPresenter
    {
        public static string GetVacancyReference(this int vacancyReferenceNumber)
        {
            return "VAC" + vacancyReferenceNumber.ToString("D9");
        }

        public static string GetVacancyReference(this long vacancyReferenceNumber)
        {
            return "VAC" + vacancyReferenceNumber.ToString("D9");
        }
    }
}