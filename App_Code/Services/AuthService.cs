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
    public (bool Success, string Message, int? UserId, string UserRole) Authenticate(string email, string password)
    {
        var contactId = _userRepository.GetContactIdByEmail(email);
        if (contactId == null) return (false, "Email not found.", null, null);

        var userId = _userRepository.GetUserIdByContactId(contactId.Value);
        if (userId == null) return (false, "User not found.", null, null);

        var userRole = _userRepository.GetUserRoleByContactId(contactId.Value);
        if (userRole == null) return (false, "User role not found.", null, null);

        var storedPassword = _userRepository.GetPasswordByUserId(userId.Value);
        if (storedPassword == null) return (false, "Account details not found.", null, null);

        if (storedPassword != password.Trim())
            return (false, "Invalid email or password.", null, null);

        return (true, "Login successful.", userId.Value, userRole);
    }
}
