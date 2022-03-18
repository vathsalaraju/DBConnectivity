//using System;
//using System.Data.SqlClient;

//namespace DbConnectionDemo
//{
//    class Program
//    {
//        public static void Main()
//        {
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
            
//        }
//    }
//}