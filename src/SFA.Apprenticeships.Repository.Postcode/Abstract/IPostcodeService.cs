﻿using System.Collections.Generic;
using SFA.Apprenticeships.Repository.Postcode.Entities;

namespace SFA.Apprenticeships.Repository.Postcode.Abstract
{
    public interface IPostcodeService
    {
        /// <summary>
        /// Gets the postcode information for the postcode or partial postcode
        /// </summary>
        PostcodeInfo GetPostcodeInfo(string postcode);

        /// <summary>
        /// Gets the postcode given lat/lon geo point.
        /// </summary>
        string GetPostcodeFromLatLong(string latitudeLongitude);

        /// <summary>
        /// Verifies if the specified postcode is valid. False otherwise.
        /// </summary>
        bool ValidatePostcode(string postcode);

        /// <summary>
        /// Gets a collection of postcodes related to the partial match.
        /// </summary>
        IList<PostcodeInfo> GetPartialMatches(string postcode);

        /// <summary>
        /// Returns a random postcode location.
        /// </summary>
        PostcodeInfo GetRandomPostcode();
    }
}
