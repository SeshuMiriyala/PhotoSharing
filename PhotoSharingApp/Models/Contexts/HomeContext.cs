using System;
using System.Data;
using System.Data.SqlClient;

namespace PhotoSharingApp.Models.Contexts
{
    internal class HomeContext : Context
    {
        public bool RegisterUser(string userName, string password, string firstName, string lastName, string middleName, string emailId)
        {
            using (var sqlcon = new SqlConnection(ConString))
            {
                sqlcon.Open();
                var transaction = sqlcon.BeginTransaction();
                try
                {
                    var command =
                        new SqlCommand(
                            "INSERT INTO Users(UserName, Password, FirstName, LastName, MiddleName, FolderPath, LastLoggedIn, EmailId) VALUES" +
                            "(@username, @password, @firstname, @lastname, @middlename, @folderPath, @loggedin, @emailId)",
                            sqlcon, transaction);
                    command.Parameters.AddWithValue("@username", userName);
                    command.Parameters.AddWithValue("@firstname", firstName);
                    command.Parameters.AddWithValue("@lastname", lastName);
                    command.Parameters.AddWithValue("@middlename", string.IsNullOrEmpty(middleName)? (object) DBNull.Value : middleName);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@emailId", emailId);
                    command.Parameters.AddWithValue("@folderPath", userName);
                    command.Parameters.AddWithValue("@loggedin", DateTime.Now);

                    var result = command.ExecuteNonQuery();

                    if (0 < result)
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
            }
            return false;
        }

        public bool IsValidUser(string userName, string password)
        {
            using (var sqlcon = new SqlConnection(ConString))
            {
                sqlcon.Open();
                var transaction = sqlcon.BeginTransaction();
                try
                {
                    var command =
                        new SqlCommand(
                            "SELECT COUNT(*) FROM Users WHERE UserName = @username AND Password = @password", sqlcon, transaction);
                    command.Parameters.AddWithValue("@username", userName);
                    command.Parameters.AddWithValue("@password", password);

                    var result = Convert.ToInt32(command.ExecuteScalar());

                    if (1 == result)
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