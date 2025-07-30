using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Security;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }
    }
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string contactQuery = "SELECT id FROM contact_details WHERE email = @UserEmail";
            SqlCommand command = new SqlCommand(contactQuery, connection);
            command.Parameters.AddWithValue("@UserEmail", UserEmail.Text);

            object contactIdObj = command.ExecuteScalar();
            if (contactIdObj == null)
            {
                ErrorMessage.Text = "Email not found.";
                return;
            }
            int contactId = Convert.ToInt32(contactIdObj);

            string userQuery = "SELECT id FROM users WHERE contact_id = @ContactId";
            command = new SqlCommand(userQuery, connection);
            command.Parameters.AddWithValue("@ContactId", contactId);

            object userIdObj = command.ExecuteScalar();
            if (userIdObj == null)
            {
                ErrorMessage.Text = "User not found.";
                return;
            }
            int userId = Convert.ToInt32(userIdObj);

            string roleQuery = "SELECT role FROM users WHERE contact_id = @ContactId";
            command = new SqlCommand(roleQuery, connection);
            command.Parameters.AddWithValue("@ContactId", contactId);

            object userRoleObj = command.ExecuteScalar();
            if(userRoleObj == null)
            {
                ErrorMessage.Text = "User not found.";
                return;
            }
            string userRole = userRoleObj.ToString();

            string passwordQuery = "SELECT user_password FROM account_details WHERE user_id = @UserId";
            command = new SqlCommand(passwordQuery, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            object passwordObj = command.ExecuteScalar();
            if (passwordObj == null)
            {
                ErrorMessage.Text = "Account details not found.";
                return;
            }

            string password = passwordObj.ToString();

 

            if(password == Password.Text.Trim())
            {

                Session["UserId"] = userId;
                Session["UserRole"] = userRole;

                string script = $"<script>console.log('Authenticated. User ID: {userId}');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "LogUserId", script);

                if (Session["UserId"] != null && Session["UserRole"] != null)
                FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, false);
            }
            else
            {
                ErrorMessage.Text = "Invalid email or password.";
            }
        }


    }
}