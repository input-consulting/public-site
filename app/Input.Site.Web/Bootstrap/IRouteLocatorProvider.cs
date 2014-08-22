using System.Collections.Generic;

namespace InputSite.Bootstrap
{
    public interface IRouteLocatorProvider
    {
        IEnumerable<string> StaticRoutes();

        IEnumerable<string> DateRoutes();
    }
}