namespace SFA.WebProxy.Models
{
    using System.Collections.Generic;

    public class Routing
    {
        public IList<Route> Routes;

        public string RewriteFrom;
        public string RewriteTo;
    }
}