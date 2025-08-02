using System;
using System.Data.SqlClient;

    public class UserRepository
    {
        public int? GetContactIdByEmail(string email)
        {
            string query = "SELECT id FROM contact_details WHERE email = @UserEmail";
            var result = DBUtil.ExecuteScalar(query, new[] {
            new SqlParameter("@UserEmail", email)
        });

            return result != null ? (int?)Convert.ToInt32(result) : null;
        }

        public int? GetUserIdByContactId(int contactId)
        {
            string query = "SELECT id FROM users WHERE contact_id = @ContactId";
            var result = DBUtil.ExecuteScalar(query, new[] {
            new SqlParameter("@ContactId", contactId)
        });

            return result != null ? (int?)Convert.ToInt32(result) : null;
        }

        public string GetUserRoleByContactId(int contactId)
        {
            string query = "SELECT role FROM users WHERE contact_id = @ContactId";
            var result = DBUtil.ExecuteScalar(query, new[] {
            new SqlParameter("@ContactId", contactId)
        });

            return result?.ToString();
        }

        public string GetPasswordByUserId(int userId)
        {
            string query = "SELECT user_password FROM account_details WHERE user_id = @UserId";
            var result = DBUtil.ExecuteScalar(query, new[] {
            new SqlParameter("@UserId", userId)
        });

            return result?.ToString();
        }
    }
