namespace SFA.WebProxy.Models
{
    using System.Collections.Generic;

    public class Routing
    {
        public IList<Route> Routes;
        public int PrimaryUriIndex { get; private set; }

        public Routing()
        {
            PrimaryUriIndex = 0;
        }

        public Routing(int primaryUriIndex)
        {
            PrimaryUriIndex = primaryUriIndex;
        }
    }
}