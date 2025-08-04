/// <summary>
/// Contains SQL queries related to user data.
/// </summary>
public static class UserQueries
{
    /// <summary>
    /// SQL query to get user id, user role, user password from users relation using the email entered by the user.
    /// </summary>
    public const string GetUserByEmail = @"
        SELECT u.id AS UserId, u.role, u.password
        FROM contact_details c
        JOIN users u ON c.id = u.contact_id
        WHERE c.email = @UserEmail;
    ";
}
