namespace SFA.Apprenticeships.Infrastructure.Raa.IoC
{
    using Application.ReferenceData;
    using Application.Candidate.Configuration;
    using StructureMap.Configuration.DSL;
    
    public class RaaRegistry : Registry
    {
        public RaaRegistry(ServicesConfiguration servicesConfiguration)
        {
            if (servicesConfiguration.VacanciesSource == ServicesConfiguration.Raa)
            {
                For<IReferenceDataProvider>()
                    .Use<ReferenceDataProvider>();
            }
        }
    }
}