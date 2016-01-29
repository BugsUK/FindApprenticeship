namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.WebService
{
    using System;
    using AutoMapper;

    public class WebServiceConsumerTypeResolver : ValueResolver<string, Domain.Entities.WebServices.WebServiceConsumerType>
    {
        protected override Domain.Entities.WebServices.WebServiceConsumerType ResolveCore(string webServiceConsumerTypeCode)
        {
            switch (webServiceConsumerTypeCode)
            {
                case "P":
                    return Domain.Entities.WebServices.WebServiceConsumerType.Provider;
                case "E":
                    return Domain.Entities.WebServices.WebServiceConsumerType.Employer;
                case "T":
                    return Domain.Entities.WebServices.WebServiceConsumerType.ThirdParty;
            }

            throw new ArgumentOutOfRangeException(
                $"Unknown web service consumer type code: \"{webServiceConsumerTypeCode}\"");
        }
    }
}
