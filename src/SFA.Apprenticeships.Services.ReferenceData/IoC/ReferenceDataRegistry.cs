﻿namespace SFA.Apprenticeships.Services.ReferenceData.IoC
{
    using SFA.Apprenticeships.Common.Interfaces.ReferenceData;
    using SFA.Apprenticeships.Common.Interfaces.Services;
    using SFA.Apprenticeships.Services.Common.Wcf;
    using SFA.Apprenticeships.Services.ReferenceData.Proxy;
    using SFA.Apprenticeships.Services.ReferenceData.Service;
    using StructureMap.Configuration.DSL;

    using SFA.Apprenticeships.Common.Caching;

    public class ReferenceDataRegistry : Registry
    {
        public ReferenceDataRegistry()
        {
            For<IWcfService<IReferenceData>>().Use<WcfService<IReferenceData>>();
            For<IReferenceDataService>().Use<ReferenceDataService>()
                .DecorateWith((ctx, provider) => new CachedReferenceDataService(ctx.GetInstance<ICacheClient>(), provider));
        }
    }
}