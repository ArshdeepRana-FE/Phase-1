<%@ Application Language="C#" %>
<%@ Import Namespace="RestaurantManagementApplication" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>

<script runat="server">
    /// <summary>
    /// Handles application start events.
    /// This method is called once when the application starts.
    /// It registers route and bundle configurations.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments.</param>
    void Application_Start(object sender, EventArgs e)
    {
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
</script>
