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
        // Retrieve user information from the repository
        UserInfo user = _userRepository.GetUserByEmail(email);

        // If the user does not exist, return failure
        if (user == null || user?.Id == null || user?.Role == null || user?.Password == null)
        {
            return (false, Messages.UserNotFound, null, null);
        }

        // Verify the provided password against the stored hash
        bool passwordIsValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
        if (!passwordIsValid)
        {
            return (false, Messages.InvalidCredentials, null, null);
        }

        // If validation is successful, return user information
        return (true, Messages.LoginSuccess, user.Id, user.Role);
    }

}
