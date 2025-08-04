using System;
using System.Data.SqlClient;

/// <summary>
/// Repository class responsible for retrieving user-related data from the database.
/// </summary>
public class UserRepository
{
    /// <summary>
    /// Executes the SQL query to get user information like id, role, password from the users relation
    /// </summary>
    /// <param name="email"></param>
    /// <returns>UserInfo object</returns>
    public UserInfo GetUserByEmail(string email)
    {
        using (var reader = DBUtil.ExecuteReader(UserQueries.GetUserByEmail, new[] {
            new SqlParameter("@UserEmail", email)
        }))
        {
            if (reader.Read())
            {
                   return new UserInfo
                   {
                       Id = Convert.ToInt32(reader["UserId"]),
                       Role = Convert.ToInt32(reader["role"]),
                       Password = reader["password"].ToString()
                   };
            }
        }
        return null; // Return null if no user is found
    }
}
