// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
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
    using Apprenticeships.Application.Application.IoC;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Provider;
    using Apprenticeships.Application.VacancyPosting.IoC;
    using Apprenticeships.Infrastructure.Azure.ServiceBus.Configuration;
    using Apprenticeships.Infrastructure.Azure.ServiceBus.IoC;
    using Apprenticeships.Infrastructure.Caching.Memory.IoC;
    using Apprenticeships.Infrastructure.Common.Configuration;
    using Apprenticeships.Infrastructure.Common.IoC;
    using Apprenticeships.Infrastructure.EmployerDataService.IoC;
    using Apprenticeships.Infrastructure.Logging.IoC;
    using Apprenticeships.Infrastructure.Postcode.IoC;
    using Apprenticeships.Infrastructure.Repositories.Mongo.Applications.IoC;
    using Apprenticeships.Infrastructure.Repositories.Mongo.Employers.IoC;
    using Apprenticeships.Infrastructure.Repositories.Mongo.Providers.IoC;
    using Apprenticeships.Infrastructure.Repositories.Sql.Configuration;
    using Apprenticeships.Infrastructure.Repositories.Sql.IoC;
    using Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.IoC;
    using StructureMap;
	
    public static class IoC {
        public static IContainer Initialize() {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationService = container.GetInstance<IConfigurationService>();
            var cacheConfig = configurationService.Get<CacheConfiguration>();
            var sqlConfiguration = configurationService.Get<SqlConfiguration>();
            var azureServiceBusConfiguration = configurationService.Get<AzureServiceBusConfiguration>();

            return new Container(c =>
            {
                c.AddRegistry<DefaultRegistry>();
                c.AddRegistry<MemoryCacheRegistry>();
                c.AddRegistry<EmployerRepositoryRegistry>();
                c.AddRegistry<EmployerDataServicesRegistry>();
                c.AddRegistry<PostcodeRegistry>();
                c.AddRegistry(new AzureServiceBusRegistry(azureServiceBusConfiguration));
                c.AddRegistry<VacancyPostingServiceRegistry>();
                c.AddRegistry<VacancyRepositoryRegistry>();
                c.AddRegistry<ApplicationServicesRegistry>();
                c.AddRegistry<ApplicationRepositoryRegistry>();

                c.For<IProviderService>().Use<ProviderService>();
                c.For<IProviderVacancyAuthorisationService>().Use<ProviderVacancyAuthorisationService>();

                c.AddRegistry(new CommonRegistry(cacheConfig));
                c.AddRegistry<LoggingRegistry>();
                
                // service layer
                c.AddRegistry(new RepositoriesRegistry(sqlConfiguration));

                c.AddRegistry<ProviderRepositoryRegistry>();
            });
        }
    }
}
