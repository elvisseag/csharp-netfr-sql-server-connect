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
    class Program
    {
        static void Main(string[] args)
        {

            //Console.WriteLine("|=========================================================|");
            //Console.WriteLine("|====== WELCOME TO SQL SERVER CONNECTION FROM C# =========|");
            //Console.WriteLine("|=========================================================|");

            // Test with String Builder (optional)
            getDataWithStringBuilder();

            // Run custom queries
            getData();
        }

        private static void getDataWithStringBuilder()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource     = ConfigurationManager.AppSettings["SERVER"];
                builder.InitialCatalog = ConfigurationManager.AppSettings["DATABASE"];
                builder.UserID         = ConfigurationManager.AppSettings["USER"];
                builder.Password       = ConfigurationManager.AppSettings["PASSWORD"];
                
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {

                    string sql = "SELECT ProductId, ProductName, Category, Price, ModifDate, Status"
                               + "  FROM Products;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1}", reader.GetInt32(0).ToString(), reader.GetString(1) );
                            }
                            Console.ReadKey();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
        }

        private static void getData()
        {

            string productId, productName;

            DataTable data= new DataTable();
            data = runQuery("SELECT ProductId, ProductName, Category, Price, ModifDate, Status"
                          + "  FROM Products;");

            foreach (DataRow row in data.Rows)
            {
                productId = row["ProductId"].ToString();
                productName = row["ProductName"].ToString();
                Console.WriteLine("{0} {1}", productId, productName);
            }
            Console.ReadKey();
        }

        private static DataTable runQuery(string query)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MY_CONNECTION"].ConnectionString);
            conn.Open();
            SqlDataAdapter adap = new SqlDataAdapter(query, conn);
            DataTable tbResult = new DataTable();
            adap.Fill(tbResult);
            conn.Close();
            return tbResult;
        }

    }
}
