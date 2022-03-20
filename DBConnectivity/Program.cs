using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DbConnectionDemo
{
    class Program
    {
        public static void Main()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from student", conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "student");
                Console.WriteLine("Using Data Set");
                foreach (DataRow row in ds.Tables["student"].Rows)
                {
                    Console.WriteLine(row["id"] + ",  " + row["name"] + ",  " + row["DOB"]);
                }


            }

            //            string connectionstr = @"Data Source=Desktop-LRM3QDC\SQLEXPRESS;Initial Catalog=Northwind;Persist Security Info=True;User ID=Guestuser;Password=vat@1234";
            //            SqlConnection conn = new SqlConnection(connectionstr);
            //            try
            //            {
            //                conn.Open();
            //                Console.WriteLine("connection established successfully");
            //            }
            //            catch (Exception e)
            //            {
            //                Console.WriteLine(e.Message);
            //            }

            //            finally
            //            {
            //                conn.Close();
            //            }

        }
    }
}