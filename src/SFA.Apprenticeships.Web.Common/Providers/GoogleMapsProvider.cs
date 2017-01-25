namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Application.Interfaces;
    using Configuration;
    using ViewModels.Locations;

    public class GoogleMapsProvider : IGoogleMapsProvider
    {
        private readonly string _privateKey;
        private readonly string _usablePrivateKey;

        public GoogleMapsProvider(IConfigurationService configurationService)
        {
            _privateKey = configurationService.Get<CommonWebConfiguration>().GoogleMapsPrivateKey;
            if (!string.IsNullOrEmpty(_privateKey))
            {
                _usablePrivateKey = _privateKey.Replace("-", "+").Replace("_", "/");
            }
        }

        public string GetStaticMapsUrl(GeoPointViewModel geoPointViewModel)
        {
            if (geoPointViewModel == null) return "";

            var staticMapsUrl = $"https://maps.googleapis.com/maps/api/staticmap?markers={geoPointViewModel.Latitude},{geoPointViewModel.Longitude}&size=190x125&zoom=12";

            if (string.IsNullOrEmpty(_usablePrivateKey))
            {
                return staticMapsUrl;
            }

            var encoding = new ASCIIEncoding();

            // converting key to bytes will throw an exception, need to replace '-' and '_' characters first.
            var privateKeyBytes = Convert.FromBase64String(_usablePrivateKey);

            staticMapsUrl += "&client=gme-skillsfundingagency";
            var uri = new Uri(staticMapsUrl);
            var encodedPathAndQueryBytes = encoding.GetBytes(uri.LocalPath + uri.Query);

            // compute the hash
            var algorithm = new HMACSHA1(privateKeyBytes);
            var hash = algorithm.ComputeHash(encodedPathAndQueryBytes);

            // convert the bytes to string and make url-safe by replacing '+' and '/' characters
            var signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");

            // Add the signature to the existing URI.
            return uri.Scheme + "://" + uri.Host + uri.LocalPath + uri.Query + "&signature=" + signature;
        }
    }
}