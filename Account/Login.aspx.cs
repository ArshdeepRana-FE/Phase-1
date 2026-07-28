using System;
using System.Web.Security;

/// <summary>
/// Code-behind for the Login page.
/// Handles user authentication and session management.
/// </summary>
public partial class Login : System.Web.UI.Page
{
    /// <summary>
    /// Handles the Page Load event.
    /// Ensures logic is only run on the initial page load.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }
    }

    /// <summary>
    /// Handles the Login button click event.
    /// Authenticates the user and sets session data on successful login.
    /// </summary>
    /// <param name="sender">The source of the event (Login button).</param>
    /// <summary>
    /// Authenticates the submitted login credentials and handles the resulting login state.
    /// </summary>
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        if(UserEmail.Text == string.Empty || Password.Text == string.Empty)
        {
            ErrorMessage.Text = "Email and password cannot be empty.";
            return;
        }
        var authService = new AuthService();
        var result = authService.Authenticate(UserEmail.Text, Password.Text);

        if (result.Success)
        {
            Session["UserId"] = result.UserId;
            Session["UserRole"] = result.UserRole;

            FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, false);
        }
        else
        {
            ErrorMessage.Text = result.Message;
        }
    }
}
