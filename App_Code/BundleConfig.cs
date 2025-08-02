using System.Web.Optimization;

namespace RestaurantManagementApplication
{
    /// <summary>
    /// Configures JavaScript and CSS bundles for optimization.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers all bundles used in the application.
        /// </summary>
        /// <param name="bundles">A collection of bundles to which script and style bundles are added.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Register script bundle for WebForms JavaScript files
            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs"));

            // Register Modernizr script bundle
            // Use the development version during development. 
            // For production, use the build tool at https://modernizr.com to include only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));
        }
    }
}
