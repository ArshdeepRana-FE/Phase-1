using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Providers.Entities;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Code-behind for the default landing page of the Restaurant Management Application.
/// </summary>
public partial class _Default : System.Web.UI.Page
{
    /// <summary>
    /// Handles the Page Load event for the default page.
    /// This method is executed every time the page is loaded or refreshed.
    /// </summary>
    /// <param name="sender">The source of the event (typically the page itself).</param>
    /// <param name="e">An EventArgs object that contains no event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null || Session["UserRole"] == null)
        {
            Response.Redirect("Account/Login.aspx");
        }


        if (!IsPostBack)
        {
             string connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string restaurantQuery = "SELECT id FROM owner_restaurant WHERE owner_id = @OwnerId";
                SqlCommand command = new SqlCommand(restaurantQuery, connection);
                command.Parameters.AddWithValue("@OwnerId", Session["UserId"].ToString());
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    HyperLink linkRestaurant = new HyperLink();
                    linkRestaurant.Text = "Restaurant ID: " + reader["id"].ToString();
                    linkRestaurant.NavigateUrl = "RestaurantPage.aspx?id=" + reader["id"].ToString();


                    RestaurantPanel.Controls.Add(linkRestaurant);
                    RestaurantPanel.Controls.Add(new LiteralControl("<br/>"));
                }

                reader.Close();
                connection.Close();
            }
        }
    }   


}

