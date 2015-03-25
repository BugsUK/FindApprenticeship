﻿namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Domain.Interfaces.Configuration;
    using Constants;
    using Infrastructure.Web.Configuration;
    using Providers;
    using ViewModels.VacancySearch;
    using Common.Attributes;

    public class LocationController : Controller
    {
        private readonly int _locationResultLimit;
        private readonly ISearchProvider _searchProvider;

        public LocationController(IConfigurationService configService, ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
            _locationResultLimit = configService.Get<WebConfiguration>(WebConfiguration.WebConfigurationName).LocationResultLimit;
        }

        [HttpGet]
        [AllowCrossSiteJson]
        [OutputCache(CacheProfile = CacheProfiles.Data, VaryByParam = "term")]
        public async Task<ActionResult> Location(string term)
        {
            return await Task.Run<ActionResult>(() =>
            {
                if (Request.IsAjaxRequest())
                {
                    var result = string.IsNullOrWhiteSpace(term)
                        ? new LocationsViewModel(new List<LocationViewModel>())
                        : _searchProvider.FindLocation(term);

                    return Json(result.Locations.Take(_locationResultLimit), JsonRequestBehavior.AllowGet);
                }

                throw new NotSupportedException("Non-JS not yet implemented!");
            });
        }

        [HttpGet]
        [AllowCrossSiteJson]
        [OutputCache(CacheProfile = CacheProfiles.Data, VaryByParam = "postcode")]
        public async Task<ActionResult> Addresses(string postcode)
        {
            return await Task.Run<ActionResult>(() =>
            {
                if (Request.IsAjaxRequest())
                {
                    var addresses = _searchProvider.FindAddresses(postcode);
                    return Json(addresses, JsonRequestBehavior.AllowGet);
                }

                throw new NotSupportedException("Non-JS not yet implemented!");
            });
        }
    }
}
