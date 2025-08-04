/// <summary>
/// Service responsible for handling user authentication logic.
/// </summary>
public class AuthService
{
    private readonly UserRepository _userRepository = new UserRepository();

    /// <summary>
    /// Authenticates a user using their email and password.
    /// </summary>
    /// <param name="email">The email address provided by the user.</param>
    /// <param name="password">The password provided by the user.</param>
    /// <returns>
    /// A tuple containing:
    /// - Success: Whether the authentication was successful.
    /// - Message: A message describing the result.
    /// - UserId: The authenticated user's ID if successful.
    /// - UserRole: The role of the authenticated user if successful.
    /// </returns>
    public (bool Success, string Message, int? UserId, int? UserRole) Authenticate(string email, string password)
    {
        var contactId = _userRepository.GetContactIdByEmail(email);
        if (contactId == null) return (false, Messages.EmailNotFound, null, null);

        var userInfo = _userRepository.GetContactIdByEmailWithRole(contactId.Value);
        var userId = userInfo?.Id;
        var userRole = userInfo?.Role;
        if(userId == null || userRole == null)
            return (false, Messages.UserNotFound, null, null);

        var storedPassword = _userRepository.GetPasswordByUserId(userId.Value);
        if (storedPassword == null) return (false, Messages.AccountNotFound, null, null);

        if (storedPassword != password.Trim())
            return (false, Messages.InvalidCredentials, null, null);

        return (true, Messages.LoginSuccess, userId.Value, userRole.Value);
    }
}
