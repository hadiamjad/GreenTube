using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using GreenTube.Models;

namespace GreenTube.Models
{
    public class crud
    {
        public static string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=project;Integrated Security=True";

        public static int Login(string userId,string _password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("Login",con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@Input_UN", SqlDbType.NVarChar, 30).Value = userId;
                cmd.Parameters.Add("@Input_Pass", SqlDbType.NVarChar, 16).Value = _password;

                cmd.Parameters.Add("@Status", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@Status"].Value);
            }

            catch(SqlException exc)
            {
                Console.WriteLine("SQL ERROR: " + exc.Message.ToString());
                result = -1;
            }

            finally
            {
                con.Close();
                con = null;
            }

            return result;
        }

        public static int addUser(string userName, string _password, string userId, DateTime _date)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("SignUp", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@UN", SqlDbType.NVarChar, 30).Value = userName;
                cmd.Parameters.Add("@pass", SqlDbType.NVarChar, 16).Value = _password;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 30).Value = userId;
                cmd.Parameters.Add("@DOB", SqlDbType.DateTime).Value = _date;

                cmd.Parameters.Add("@Status", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@Status"].Value);

            }
            catch (SqlException exc)
            {
                Console.WriteLine("SQL ERROR: " + exc.Message.ToString());
                result = -1;
            }
            finally
            {
                con.Close();
                con = null;
            }

            return result;
        }
            

    }
}