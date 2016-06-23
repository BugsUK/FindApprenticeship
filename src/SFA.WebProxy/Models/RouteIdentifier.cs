namespace SFA.WebProxy.Models
{
    using System;

    public class RouteIdentifier
    {
        public RouteIdentifier(DateTime dateTime, Guid id, string name)
        {
            DateTime = dateTime;
            Id = id;
            Name = name;
        }

        public RouteIdentifier() : this(DateTime.UtcNow, Guid.NewGuid(), string.Empty)
        {
            
        }

        public RouteIdentifier(string name) : this(DateTime.UtcNow, Guid.NewGuid(), name)
        {
            
        }

        public RouteIdentifier(RouteIdentifier routeIdentifier, string name) : this(routeIdentifier.DateTime, routeIdentifier.Id, name)
        {
            
        }

        public DateTime DateTime { get; private set; }

        public Guid Id { get; private set; }

        public string Name { get; private set; }
    }
}