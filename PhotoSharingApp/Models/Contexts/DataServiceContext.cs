
using System;
using System.Data;
using System.Data.SqlClient;

namespace PhotoSharingApp.Models.Contexts
{
    internal class DataServiceContext : Context
    {
        public bool IsValidEmail(string email)
        {
            using (var sqlcon = new SqlConnection(ConString))
            {
                sqlcon.Open();
                var transaction = sqlcon.BeginTransaction();
                try
                {
                    var command =
                        new SqlCommand(
                            "SELECT COUNT(*) FROM Users WHERE EmailId = @email", sqlcon, transaction);
                    command.Parameters.AddWithValue("@email", email);

                    var result = Convert.ToInt32(command.ExecuteScalar());

                    if (0 == result)
                    {
                        transaction.Commit();
                        sqlcon.Close();
                        return true;
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (sqlcon.State == ConnectionState.Open)
                    {
                        transaction.Rollback();
                        sqlcon.Close();
                    }
                }
                return false;
            }
        }

        public bool IsValidUserName(string username)
        {
            using (var sqlcon = new SqlConnection(ConString))
            {
                sqlcon.Open();
                var transaction = sqlcon.BeginTransaction();
                try
                {
                    var command =
                        new SqlCommand(
                            "SELECT COUNT(*) FROM Users WHERE UserName = @username", sqlcon, transaction);
                    command.Parameters.AddWithValue("@username", username);

                    var result = Convert.ToInt32(command.ExecuteScalar());

                    if (0 == result)
                    {
                        transaction.Commit();
                        sqlcon.Close();
                        return true;
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (sqlcon.State == ConnectionState.Open)
                    {
                        transaction.Rollback();
                        sqlcon.Close();
                    }
                }
                return false;
            }
        }
    }
}