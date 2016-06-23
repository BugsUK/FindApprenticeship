namespace SFA.WebProxy.Models
{
    using System;

    public class Route
    {
        public Route(Uri uri, RouteIdentifier identifier, bool isPrimary)
        {
            Uri = uri;
            Identifier = identifier;
            IsPrimary = isPrimary;
        }

        public Route(string uri, RouteIdentifier identifier, bool isPrimary) : this(new Uri(uri), identifier, isPrimary)
        {

        }

        public Uri Uri { get; private set; }

        public RouteIdentifier Identifier { get; private set; }

        public bool IsPrimary { get; private set; }
    }
}