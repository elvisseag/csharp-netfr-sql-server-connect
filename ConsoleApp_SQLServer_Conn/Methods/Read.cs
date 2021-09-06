using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_SQLServer_Conn
{
    public class Read
    {



        public static void ShowAllData()
        {

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MY_CONNECTION"].ConnectionString))
            {
                string sql = "SELECT ProductId, ProductName, Category, Price, ModifDate, Status"
                              + "  FROM Products;";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("{0} {1} {2} {3} {4} {5}",
                                           "ID".PadRight(8, ' '),
                                           "NAME".PadRight(15, ' '),
                                           "CATEGORY".PadRight(15, ' '),
                                           "PRICE".PadRight(10, ' '),
                                           "DATE".PadRight(20, ' '),
                                           "STATUS".PadRight(7, ' '));

                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1} {2} {3} {4} {5}",
                                               reader.GetInt32(0).ToString().PadRight(8, ' '),
                                               reader.GetString(1).PadRight(15, ' '),
                                               reader.GetString(2).PadRight(15, ' '),
                                               reader.GetDecimal(3).ToString().PadRight(10, ' '),
                                               reader.GetDateTime(4).ToString().PadRight(20, ' '),
                                               reader.GetInt32(5).ToString().PadRight(7, ' '));
                        }
                    }
                    conn.Close();
                }
            }
        }



        //---------------------------------------------------------------------------------------------------//
        // READ RECORD METHOD                                                                              //
        //---------------------------------------------------------------------------------------------------//
        public static void ReadRecord(Product product)
        {

            try
            {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MY_CONNECTION"].ConnectionString))
                {
                    string sql = "SELECT ProductId, ProductName, Category, Price, ModifDate, Status"
                                  + "  FROM Products WHERE ProductId = " + product.ProductId;

                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                Console.WriteLine("ID: ".PadRight(10, ' ') + "{0}", reader.GetInt32(0).ToString().PadRight(8, ' '));
                                Console.WriteLine("NAME: ".PadRight(10, ' ') + "{0}", reader.GetString(1).PadRight(15, ' '));
                                Console.WriteLine("CATEGORY: ".PadRight(10, ' ') + "{0}", reader.GetString(2).PadRight(15, ' '));
                                Console.WriteLine("PRICE: ".PadRight(10, ' ') + "{0}", reader.GetDecimal(3).ToString().PadRight(10, ' '));
                                Console.WriteLine("DATE: ".PadRight(10, ' ') + "{0}", reader.GetDateTime(4).ToString().PadRight(20, ' '));
                                Console.WriteLine("STATUS: ".PadRight(10, ' ') + "{0}", reader.GetInt32(5).ToString().PadRight(7, ' '));
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        public static Product ReadRecordTemp(Product product)
        {

            DataTable data = new DataTable();

            string query = "SELECT ProductName, Category, Price, ModifDate, Status "
                         + "  FROM Products"
                         + "  WHERE ProductId = " + product.ProductId;

            try
            {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MY_CONNECTION"].ConnectionString))
                {
                    conn.Open();
                    using (SqlDataAdapter adapt = new SqlDataAdapter(query, conn))
                    {
                        adapt.Fill(data);
                        foreach (DataRow row in data.Rows)
                        {
                            product.ProductName = row["ProductName"].ToString();
                            product.Category = row["Category"].ToString();
                            product.Price = Convert.ToDecimal(row["Price"]);
                            product.ModifDate = Convert.ToDateTime(row["ModifDate"]);
                            product.Status = Convert.ToInt32(row["Status"]);
                            break;
                        }
                    }
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return product;
        }



        //---------------------------------------------------------------------------------//
        // RUN QUERY (SELECT)                                                              //
        //---------------------------------------------------------------------------------//
        public static DataTable runQuery(string query)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MY_CONNECTION"].ConnectionString);
            SqlDataAdapter adap = new SqlDataAdapter(query, conn);
            DataTable tbResult = new DataTable();
            adap.Fill(tbResult);
            return tbResult;
        }




    }
}
