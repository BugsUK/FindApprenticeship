namespace SFA.Apprenticeships.Web.Candidate.Extensions
{
    using System;
    using Common.Models.Common;
    using Common.Providers;
    using Constants;
    using Domain.Entities.Vacancies;

    public static class UserDataProviderExtensions
    {
        public static void PushLastViewedVacancyId(this IUserDataProvider userData, int id, VacancyType type)
        {
            PushLastViewedVacancy(userData, new LastViewedVacancy {Id = id, Type = type});
        }

        public static void PushLastViewedVacancy(this IUserDataProvider userData, LastViewedVacancy lastViewedVacancy)
        {
            userData.Push(CandidateDataItemNames.LastViewedVacancy, string.Format("{0}_{1}", lastViewedVacancy.Type, lastViewedVacancy.Id));
        }

        public static LastViewedVacancy PopLastViewedVacancy(this IUserDataProvider userData)
        {
            var lastViewedVacancyId = userData.Pop(CandidateDataItemNames.LastViewedVacancy);
            if (string.IsNullOrEmpty(lastViewedVacancyId) || !lastViewedVacancyId.Contains("_"))
            {
                return null;
            }
            
            var lastViewedVacancyComponents = lastViewedVacancyId.Split('_');

            var vacancyType = (VacancyType)Enum.Parse(typeof (VacancyType), lastViewedVacancyComponents[0]);
            var vacancyId = int.Parse(lastViewedVacancyComponents[1]);

            return new LastViewedVacancy {Type = vacancyType, Id = vacancyId};
        }
    }
}