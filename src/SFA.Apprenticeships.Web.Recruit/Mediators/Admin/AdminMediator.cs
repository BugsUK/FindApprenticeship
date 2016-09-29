namespace SFA.Apprenticeships.Web.Recruit.Mediators.Admin
{
    using Common.Mediators;
    using Domain.Entities.Vacancies;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels.Admin;

    public class AdminMediator : MediatorBase, IAdminMediator
    {

        public AdminMediator()
        {

        }

        public MediatorResponse<TransferVacanciesViewModel> GetVacancyDetails(IList<string> vacancies)
        {
            IList<string> vacancyReferences = new List<string>();
            foreach (var vacancy in vacancies)
            {
                string vacancyReference;
                if (VacancyHelper.TryGetVacancyReference(vacancy, out vacancyReference))
                    vacancyReferences.Add(vacancyReference);
            }
            if (!vacancyReferences.Any())
                return GetMediatorResponse(AdminMediatorCodes.GetVacancyDetails.NoRecordsFound, new TransferVacanciesViewModel());
            return new MediatorResponse<TransferVacanciesViewModel>();
        }
    }
}