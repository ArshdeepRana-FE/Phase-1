public class AuthService
{
    private readonly UserRepository _userRepository = new UserRepository();

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
