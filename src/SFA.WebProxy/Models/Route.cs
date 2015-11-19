namespace SFA.WebProxy.Models
{
    using System;

    public class Route
    {
        public Route(Uri uri, RouteIdentifier identifier)
        {
            Uri = uri;
            Identifier = identifier;
        }

        public Route(string uri, RouteIdentifier identifier) : this(new Uri(uri), identifier)
        {

        }

        public Uri Uri { get; private set; }

        public RouteIdentifier Identifier { get; private set; }
    }
}