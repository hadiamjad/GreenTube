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


        public static int Login(string _useremail,string _password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("Login",con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@Input_UE", SqlDbType.NVarChar, 30).Value = _useremail;
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

        public static int addUser(string userName, string _password, string _useremail, DateTime _date)
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
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 30).Value = _useremail;
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
           
 
        public static int addVideoToDB(VideoFile video)
        {
            if (video != null)
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd;
                int result=0;

                try
                {
                    cmd = new SqlCommand("uploadvid", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vidname", video.vidtitle);
                    cmd.Parameters.AddWithValue("@uploader", video.uploader);
                    cmd.Parameters.AddWithValue("@vidsize", video.videosize);
                    cmd.Parameters.AddWithValue("@vidpath", video.videopath);
                    cmd.ExecuteNonQuery();

                    result = 1; // upload succeeds

                }
                catch( SqlException exc)
                {
                    Console.WriteLine("SQL ERROR: " + exc);
                    result = -1;
                }
                finally
                {
                    con.Close();    
                    con = null;
                }

                return result;

            }

            else
                return -404; // file was blank
        }

        public static List<VideoFile> RetrieveVid()
        {
            List<VideoFile> videolist = new List<VideoFile>();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("getvid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    VideoFile vid = new VideoFile();
                    vid.uploader = Convert.ToInt32(rdr["uploaderID"]);
                    vid.vidtitle = rdr["vidtitle"].ToString();
                    vid.videosize = Convert.ToInt32(rdr["videosize"]);
                    vid.videopath = rdr["videopath"].ToString();
                    vid.views = Convert.ToInt32(rdr["views"]);
                    vid.likes = Convert.ToInt32(rdr["likes"]);
                    vid.dislikes = Convert.ToInt32(rdr["dislikes"]);
                    vid.uploadtime = Convert.ToDateTime(rdr["uploadtime"]);
                    videolist.Add(vid);
                }


            }
            catch(SqlException exc) {
                Console.WriteLine("SQL ERROR: " + exc);
            }
            finally {
                con.Close();
                con = null;
            }

            return videolist;

        }

        public static List<VideoFile> mostrecentvids()
        {
            List<VideoFile> videolist = new List<VideoFile>();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("mostrecent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    VideoFile vid = new VideoFile();
                    vid.uploader = Convert.ToInt32(rdr["uploaderID"]);
                    vid.vidtitle = rdr["vidtitle"].ToString();
                    vid.videosize = Convert.ToInt32(rdr["videosize"]);
                    vid.videopath = rdr["videopath"].ToString();
                    vid.views = Convert.ToInt32(rdr["views"]);
                    vid.likes = Convert.ToInt32(rdr["likes"]);
                    vid.dislikes = Convert.ToInt32(rdr["dislikes"]);
                    vid.uploadtime = Convert.ToDateTime(rdr["uploadtime"]);
                   
                    videolist.Add(vid);
                }


            }
            catch (SqlException exc)
            {
                Console.WriteLine("SQL ERROR: " + exc);
            }
            finally
            {
                con.Close();
                con = null;
            }

            return videolist;

        }

        public static List<VideoFile> mostlikedvids()
        {
            List<VideoFile> videolist = new List<VideoFile>();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("mostliked", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    VideoFile vid = new VideoFile();
                    vid.uploader = Convert.ToInt32(rdr["uploaderID"]);
                    vid.vidtitle = rdr["vidtitle"].ToString();
                    vid.videosize = Convert.ToInt32(rdr["videosize"]);
                    vid.videopath = rdr["videopath"].ToString();
                    vid.views = Convert.ToInt32(rdr["views"]);
                    vid.likes = Convert.ToInt32(rdr["likes"]);
                    vid.dislikes = Convert.ToInt32(rdr["dislikes"]);
                    vid.uploadtime = Convert.ToDateTime(rdr["uploadtime"]);
                    videolist.Add(vid);
                }


            }
            catch (SqlException exc)
            {
                Console.WriteLine("SQL ERROR: " + exc);
            }
            finally
            {
                con.Close();
                con = null;
            }

            return videolist;

        }

        public static List<VideoFile> mostwatchedvids()
        {
            List<VideoFile> videolist = new List<VideoFile>();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("mostwatched", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    VideoFile vid = new VideoFile();
                    vid.uploader = Convert.ToInt32(rdr["uploaderID"]);
                    vid.vidtitle = rdr["vidtitle"].ToString();
                    vid.videosize = Convert.ToInt32(rdr["videosize"]);
                    vid.videopath = rdr["videopath"].ToString();
                    vid.views = Convert.ToInt32(rdr["views"]);
                    vid.likes = Convert.ToInt32(rdr["likes"]);
                    vid.dislikes = Convert.ToInt32(rdr["dislikes"]);
                    vid.uploadtime = Convert.ToDateTime(rdr["uploadtime"]);

                    videolist.Add(vid);
                }


            }
            catch (SqlException exc)
            {
                Console.WriteLine("SQL ERROR: " + exc);
            }
            finally
            {
                con.Close();
                con = null;
            }

            return videolist;

        }

        public static int PageDecider(int _id)
        {
            int flag = new int();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("creatorcheck", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = _id;

                cmd.Parameters.Add("@bool", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                flag = Convert.ToInt32(cmd.Parameters["@bool"].Value);
            }
            catch (SqlException exc)
            {
                Console.WriteLine("SQL ERROR: " + exc);
            }
            finally
            {
                con.Close();
                con = null;
            }

            return flag;

        }


        public static UserData UserInformation(int _id)
        {
            UserData info = new UserData();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("BringUserInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = _id;



                SqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();

                info.uid = Convert.ToInt32(rdr["userID"]);
                info.ucreator= Convert.ToInt32(rdr["isCreator"]);
                info.uemail= rdr["userEmail"].ToString();
                info.ubod = Convert.ToDateTime(rdr["dateOfBirth"]);
                info.ujoin= Convert.ToDateTime(rdr["joinDate"]);

            }
            catch (SqlException exc)
            {
                Console.WriteLine("SQL ERROR: " + exc);
            }
            finally
            {
                con.Close();
                con = null;
            }

            return info;

        }

        public static void becomeCreator(int _id)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("becomeCreator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = _id;
                cmd.ExecuteNonQuery();

            }
            catch (SqlException exc)
            {
                Console.WriteLine("SQL ERROR: " + exc);
            }
            finally
            {
                con.Close();
                con = null;
            }

        }

        public static void DeleteId(int _id)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("DeleteAccount", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@uID", SqlDbType.Int).Value = _id;
                cmd.ExecuteNonQuery();

            }
            catch (SqlException exc)
            {
                Console.WriteLine("SQL ERROR: " + exc);
            }
            finally
            {
                con.Close();
                con = null;
            }

        }


        public static void SignOut(int _id)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("Logoff", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Input_UN", SqlDbType.Int).Value = _id;
                cmd.ExecuteNonQuery();

            }
            catch (SqlException exc)
            {
                Console.WriteLine("SQL ERROR: " + exc);
            }
            finally
            {
                con.Close();
                con = null;
            }

        }

        public static Creator CreatorView(int _id)
        {
            Creator mystats = new Creator();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            SqlCommand cmd1;
            List<VideoFile> videolist = new List<VideoFile>();
            try
            {
                cmd = new SqlCommand("allvids", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = _id;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    VideoFile vid = new VideoFile();
                    vid.uploader = Convert.ToInt32(rdr["uploaderID"]);
                    vid.vidtitle = rdr["vidtitle"].ToString();
                    vid.videosize = Convert.ToInt32(rdr["videosize"]);
                    vid.videopath = rdr["videopath"].ToString();
                    vid.views = Convert.ToInt32(rdr["views"]);
                    vid.likes = Convert.ToInt32(rdr["likes"]);
                    vid.dislikes = Convert.ToInt32(rdr["dislikes"]);
                    vid.uploadtime = Convert.ToDateTime(rdr["uploadtime"]);
                    videolist.Add(vid);
                }
                mystats.myvids = videolist;
            }
            catch (SqlException exc)
            {
                Console.WriteLine("SQL ERROR: " + exc);
            }
            finally
            {
                con.Close();
                con = null;
            }
            SqlConnection con1 = new SqlConnection(connectionString);
            con1.Open();
            try
            {
                cmd1 = new SqlCommand("totalsubscribers", con1);
                cmd1.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                cmd1.Parameters.Add("@id", SqlDbType.Int).Value = _id;
                rdr1.Read();
                mystats.totalsubscribers = Convert.ToInt32(rdr1["totalsubscribers"]);
            }
            catch (SqlException exc)
            {
                Console.WriteLine("SQL ERROR: " + exc);
            }
            finally
            {
                con1.Close();
                con1 = null;
            }
            return mystats;
        }
    }
}
