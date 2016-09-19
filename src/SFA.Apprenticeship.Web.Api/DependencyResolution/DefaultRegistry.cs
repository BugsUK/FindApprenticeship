// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SFA.Apprenticeship.Web.Api.DependencyResolution {
    using System.Web;
    using Apprenticeships.Application.Communication;
    using Apprenticeships.Application.Communication.Strategies;
    using Apprenticeships.Application.Employer;
    using Apprenticeships.Application.Employer.Strategies;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Interfaces.Communications;
    using Apprenticeships.Application.Interfaces.Employers;
    using Apprenticeships.Application.Interfaces.Organisations;
    using Apprenticeships.Application.Organisation;
    using Apprenticeships.Application.ReferenceData;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Apprenticeships.Infrastructure.Raa;
    using Apprenticeships.Infrastructure.Raa.Mappers;
    using Apprenticeships.Infrastructure.Raa.Strategies;
    using Apprenticeships.Web.Raa.Common.Mappers;
    using Apprenticeships.Web.Raa.Common.Strategies;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
	
    public class DefaultRegistry : Registry {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));
            For<IMapper>().Singleton().Use<RaaCommonWebMappers>().Name = "RaaCommonWebMappers";
            //For<IMapper>().Singleton().Use<RecruitMappers>().Name = "RecruitMappers";

            For<IGetByIdStrategy>().Use<GetByIdStrategy>();
            For<IGetByIdsStrategy>().Use<GetByIdsStrategy>();
            For<IGetByEdsUrnStrategy>().Use<GetByEdsUrnStrategy>().Ctor<IMapper>().Named("EmployerMappers");

            For<IVacancySummaryStrategy>().Use<VacancySummaryStrategy>().Ctor<IMapper>().Named("RaaCommonWebMappers");

            For<IEmployerService>().Use<EmployerService>();
            For<IOrganisationService>().Use<OrganisationService>();
            For<IEmployerCommunicationService>().Use<EmployerCommunicationService>();

            For<IGetPagedEmployerSearchResultsStrategy>().Use<GetPagedEmployerSearchResultsStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<ISaveEmployerStrategy>().Use<SaveEmployerStrategy>();
            For<ISendEmployerLinksStrategy>().Use<SendEmployerLinksStrategy>();
            For<ISendEmployerCommunicationStrategy>().Use<QueueEmployerCommunicationStrategy>();

            For<IPublishVacancySummaryUpdateStrategy>().Use<PublishVacancySummaryUpdateStrategy>().Ctor<IMapper>().Is<VacancySummaryUpdateMapper>();

            For<IReferenceDataProvider>().Use<ReferenceDataProvider>();

            For<IGetReleaseNotesStrategy>().Use<GetReleaseNotesStrategy>();
        }

        #endregion
    }
}