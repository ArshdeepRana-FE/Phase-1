/// <summary>
///    This class contains string data members that represent login operations.
/// </summary>
public class Messages
{
    /// <summary>
    /// Displayed when the userId or userRole is null
    /// </summary>
    public const string UserNotFound = "User not found.";

    /// <summary>
    /// Displayed when the email does not exist in the database
    /// </summary>
    public const string EmailNotFound = "Email not found.";

    /// <summary>
    /// Displayed when the password does not match the stored password
    /// </summary>
    public const string InvalidCredentials = "Invalid email or password.";

    /// <summary>
    /// Displayed when the credentials are valid and the user is successfully logged in
    /// </summary>
    public const string LoginSuccess = "Login successful.";

    /// <summary>
    /// Displayed when the account is not found in the database
    /// </summary>
    public const string AccountNotFound = "Account not found.";
}