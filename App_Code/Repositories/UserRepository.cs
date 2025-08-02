using System;
using System.Data.SqlClient;

/// <summary>
/// Repository class responsible for retrieving user-related data from the database.
/// </summary>
public class UserRepository
{
    /// <summary>
    /// Retrieves the contact ID associated with the provided email.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <returns>The contact ID if found; otherwise, null.</returns>
    public int? GetContactIdByEmail(string email)
    {
        string query = "SELECT id FROM contact_details WHERE email = @UserEmail";
        var result = DBUtil.ExecuteScalar(query, new[] {
            new SqlParameter("@UserEmail", email)
        });

        return result != null ? (int?)Convert.ToInt32(result) : null;
    }

    /// <summary>
    /// Retrieves the user ID associated with the provided contact ID.
    /// </summary>
    /// <param name="contactId">The contact ID of the user.</param>
    /// <returns>The user ID if found; otherwise, null.</returns>
    public int? GetUserIdByContactId(int contactId)
    {
        string query = "SELECT id FROM users WHERE contact_id = @ContactId";
        var result = DBUtil.ExecuteScalar(query, new[] {
            new SqlParameter("@ContactId", contactId)
        });

        return result != null ? (int?)Convert.ToInt32(result) : null;
    }

    /// <summary>
    /// Retrieves the role of the user associated with the provided contact ID.
    /// </summary>
    /// <param name="contactId">The contact ID of the user.</param>
    /// <returns>The user's role as a string, or null if not found.</returns>
    public string GetUserRoleByContactId(int contactId)
    {
        string query = "SELECT role FROM users WHERE contact_id = @ContactId";
        var result = DBUtil.ExecuteScalar(query, new[] {
            new SqlParameter("@ContactId", contactId)
        });

        return result?.ToString();
    }

    /// <summary>
    /// Retrieves the password of the user associated with the provided user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The user's password as a string, or null if not found.</returns>
    public string GetPasswordByUserId(int userId)
    {
        string query = "SELECT user_password FROM account_details WHERE user_id = @UserId";
        var result = DBUtil.ExecuteScalar(query, new[] {
            new SqlParameter("@UserId", userId)
        });

        return result?.ToString();
    }
}
