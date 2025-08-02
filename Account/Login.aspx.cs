using System;
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
