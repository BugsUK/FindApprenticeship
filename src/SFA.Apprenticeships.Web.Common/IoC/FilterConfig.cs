namespace SFA.Apprenticeships.Web.Common.IoC
{
    using System.Linq;
    using System.Web.Mvc;
    using Attributes;
    using StructureMap;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters, IContainer container)
        {
            var oldProvider = FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider);
            FilterProviders.Providers.Remove(oldProvider);
            FilterProviders.Providers.Add(new DependencyResolverFilterProvider(container));

            filters.Add(new AiHandleErrorAttribute());
        }
    }
}
