namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.WebService
{
    using System.Collections.Generic;
    using Entities;
    using Vacancy;

    public class WebServiceMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<WebServiceConsumer, Domain.Entities.WebServices.WebServiceConsumer>()
                .ForMember(d => d.AllowedWebServiceCategories, opt => opt.ResolveUsing(AllowedWebServiceCategoriesResolver));
        }

        private static List<Domain.Entities.WebServices.WebServiceCategory> AllowedWebServiceCategoriesResolver(WebServiceConsumer webServiceConsumer)
        {
            var allowedWebServiceCategories = new List<Domain.Entities.WebServices.WebServiceCategory>();

            if (webServiceConsumer.AllowReferenceDataService)
            {
                allowedWebServiceCategories.Add(Domain.Entities.WebServices.WebServiceCategory.Reference);
            }

            if (webServiceConsumer.AllowVacancyUploadService)
            {
                allowedWebServiceCategories.Add(Domain.Entities.WebServices.WebServiceCategory.VacancyUpload);
            }

            if (webServiceConsumer.AllowVacancySummaryService)
            {
                allowedWebServiceCategories.Add(Domain.Entities.WebServices.WebServiceCategory.VacancySummary);
            }

            if (webServiceConsumer.AllowVacancyDetailService)
            {
                allowedWebServiceCategories.Add(Domain.Entities.WebServices.WebServiceCategory.VacancyDetail);
            }

            return allowedWebServiceCategories;
        }
    }
}
