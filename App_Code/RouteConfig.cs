using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace RestaurantManagementApplication
{
    /// <summary>
    /// Configures URL routing for the application.
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// Registers friendly URL routes for the application.
        /// </summary>
        /// <param name="routes">The route collection to configure.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings
            {
                AutoRedirectMode = RedirectMode.Permanent
            };

            // Enable friendly URLs (removes ".aspx" from URLs)
            routes.EnableFriendlyUrls(settings);
        }
    }
}
