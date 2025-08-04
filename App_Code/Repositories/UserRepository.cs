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
    /// <param name="email">The user's email address.</param>git s
    /// <returns>The contact ID if found; otherwise, null.</returns>
    public int? GetContactIdByEmail(string email)
    {
        var result = DBUtil.ExecuteScalar(UserQueries.GetContactIdByEmail, new[] {
            new SqlParameter("@UserEmail", email)
        });

        return result != null ? (int?)Convert.ToInt32(result) : null;
    }

    /// <summary>
    /// Retrieves the user ID associated with the provided contact ID and Retrieves the role of the user associated with the provided contact ID.
    /// </summary>
    /// <param name="contactId">The contact ID of the user.</param>
    /// <returns>The user ID if found; otherwise, null. and The user's role as a string, or null if not found.</returns>
    public UserInfo GetContactIdByEmailWithRole(int contactId)
    {
        using (var reader = DBUtil.ExecuteReader(UserQueries.GetUserIdByContactIdWithRole, new[] {
        new SqlParameter("@ContactId", contactId)
    }))
        {
            if (reader.Read())
            {
                return new UserInfo
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Role = Convert.ToInt32(reader["role"])
                };
            }
        }
        return null; // Return null if no user is found
    }
    
    /// <summary>
    /// Retrieves the password of the user associated with the provided user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The user's password as a string, or null if not found.</returns>
    public string GetPasswordByUserId(int userId)
    {
        var result = DBUtil.ExecuteScalar(UserQueries.GetPasswordByUserId, new[] {
            new SqlParameter("@UserId", userId)
        });

        return result?.ToString();
    }
}
