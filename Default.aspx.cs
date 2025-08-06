using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Code-behind for the default landing page of the Restaurant Management Application.
/// </summary>
public partial class _Default : Page
{
    /// <summary>
    /// Handles the Page Load event for the default page.
    /// This method is executed every time the page is loaded or refreshed.
    /// </summary>
    /// <param name="sender">The source of the event (typically the page itself).</param>
    /// <param name="e">An EventArgs object that contains no event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Redirect unauthenticated users
        if (Session["UserId"] == null || Session["UserRole"] == null || Convert.ToInt32(Session["UserRole"]) != 0)
        {
            Response.Redirect("Account/Login.aspx");
            return;
        }

        // Only run on first page load (not postbacks)
        if (!IsPostBack)
        {
            int ownerId = Convert.ToInt32(Session["UserId"]);
            var ownerRepo = new OwnerRepository();
            List<Restaurant> restaurants = ownerRepo.GetRestaurantsByOwnerId(ownerId);

            foreach (Restaurant restaurant in restaurants)
            {
                var link = new HyperLink
                {
                    Text = "Restaurant ID: " + restaurant.Id + "  Restaurant Name: " + restaurant.Name,
                    NavigateUrl = $"/Pages/RestaurantPage.aspx?id={restaurant.Id}"
                };

                RestaurantPanel.Controls.Add(link);
                RestaurantPanel.Controls.Add(new LiteralControl("<br/>"));
            }
        }
    }
}
