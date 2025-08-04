/// <summary>
/// Contains SQL queries related to user data.
/// </summary>
public static class UserQueries
{
    /// <summary>
    /// SQL query to retrieve the contact ID from the contact_details table by user email.
    /// </summary>
    public const string GetContactIdByEmail = @"
        SELECT id 
        FROM contact_details 
        WHERE email = @UserEmail";

    /// <summary>
    /// SQL query to retrieve the user ID from the users table by contact ID.
    /// </summary>
    public const string GetUserIdByContactId = @"
        SELECT id 
        FROM users 
        WHERE contact_id = @ContactId";

    public const string GetUserIdByContactIdWithRole = @"
        SELECT id, role 
        FROM users 
        WHERE contact_id = @ContactId";
    /// <summary>
    /// SQL query to retrieve the user role from the users table by contact ID.
    /// </summary>
    public const string GetUserRoleByContactId = @"
        SELECT role 
        FROM users 
        WHERE contact_id = @ContactId";

    /// <summary>
    /// SQL query to retrieve the user password from the account_details table by user ID.
    /// </summary>
    public const string GetPasswordByUserId = @"
        SELECT user_password 
        FROM account_details 
        WHERE user_id = @UserId";
}
