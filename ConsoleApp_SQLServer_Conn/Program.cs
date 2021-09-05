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

            Console.WriteLine("Probando conexión ...");
            // test connection
            int test = TestConnection(1);
            if (test != 0)
            {
                Console.Clear();
            }

            Product product = new Product();

            Console.WriteLine("|=========================================================|");
            Console.WriteLine("|====== WELCOME TO SQL SERVER CONNECTION FROM C# =========|");
            Console.WriteLine("|=========================================================|");
            Console.WriteLine();

            // Get all data (default)
            ShowAllData();

            string sel = "";

            while (sel != "EXIT")
            {

                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Indique acción a realizar:");
                Console.WriteLine("A: Mostrar todos los registros"); 
                Console.WriteLine("C: Create a row");
                Console.WriteLine("R: Read a row");
                Console.WriteLine("U: Update a row");
                Console.WriteLine("D: Delete a row");
                Console.WriteLine("EXIT: Salir");
                Console.ResetColor();


                sel = Console.ReadLine();

                switch (sel)
                {
                    case "A":
                        ShowAllData();
                        break;
                    case "C":
                        Console.WriteLine("Ingrese nombre del producto:");
                        product.ProductName = Console.ReadLine();
                        Console.WriteLine("Ingrese categoria del producto:");
                        product.Category = Console.ReadLine();
                        Console.WriteLine("Ingrese precio del producto:");
                        product.Price = Convert.ToDecimal(Console.ReadLine());
                        product.ModifDate = DateTime.Now.Date;
                        product.Status = 1;
                        CreateRecord(product);
                        break;
                    case "R":
                        Console.WriteLine("Ingrese código del producto:");
                        product.ProductId = Convert.ToInt32(Console.ReadLine());
                        ReadRecord(product);
                        break;
                    case "U":
                        Console.WriteLine("Ingrese código del producto a actualizar:");
                        product.ProductId = Convert.ToInt32(Console.ReadLine());
                        Product p = ReadRecordTemp(product);

                        Console.WriteLine("Ingrese nombre del producto:");
                        Console.WriteLine("CURRENT VALUE: " + p.ProductName);
                        product.ProductName = Console.ReadLine();
                        Console.WriteLine("Ingrese categoria del producto:");
                        Console.WriteLine("CURRENT VALUE: " + p.Category);
                        product.Category = Console.ReadLine();
                        Console.WriteLine("Ingrese precio del producto:");
                        Console.WriteLine("CURRENT VALUE: " + p.Price);
                        product.Price = Convert.ToDecimal(Console.ReadLine());
                        product.ModifDate = DateTime.Now.Date;
                        product.Status = 1;
                        UpdateRecord(product);
                        break;
                    case "D":
                        Console.WriteLine("Ingrese código del producto a eliminar:");
                        product.ProductId = Convert.ToInt32(Console.ReadLine());
                        DeleteRecord(product);
                        break;
                    case "EXIT":
                        Console.WriteLine("BYE");
                        break;
                    default:
                        break;
                }

            }

        }

        private static Product ReadRecordTemp(Product product)
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

        //---------------------------------------------------------------------------------------------------//
        // DELETE RECORD METHOD                                                                              //
        //---------------------------------------------------------------------------------------------------//
        private static void DeleteRecord(Product product)
        {
            int rowsAff = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MY_CONNECTION"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE Products WHERE ProductId = @ProductId", conn) )
                    {
                        cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                        rowsAff = cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    Console.WriteLine(rowsAff.ToString() + " rows affected.");
                }

                
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        //---------------------------------------------------------------------------------------------------//
        // UPDATE RECORD METHOD                                                                              //
        //---------------------------------------------------------------------------------------------------//
        private static void UpdateRecord(Product product)
        {
            int rowsAff = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MY_CONNECTION"].ConnectionString))
                {
                    SqlCommand cmd;

                    conn.Open();
                    cmd = new SqlCommand("UPDATE Products "
                                       + "  SET ProductName = @ProductName, "
                                       + "      Category    = @Category, "
                                       + "      Price       = @Price, "
                                       + "      ModifDate   = @ModifDate, "
                                       + "      Status      = @Status"
                                       + "  WHERE ProductId = @ProductId", conn);
                    cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@Category", product.Category);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@ModifDate", product.ModifDate);
                    cmd.Parameters.AddWithValue("@Status", product.Status);
                    rowsAff = cmd.ExecuteNonQuery();
                    conn.Close();

                    Console.WriteLine(rowsAff.ToString() + " rows affected.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        //---------------------------------------------------------------------------------------------------//
        // READ RECORD METHOD                                                                              //
        //---------------------------------------------------------------------------------------------------//
        private static void ReadRecord(Product product)
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

        //---------------------------------------------------------------------------------------------------//
        // CREATE RECORD METHOD                                                                              //
        //---------------------------------------------------------------------------------------------------//
        private static void CreateRecord(Product product)
        {
            int rowsAff = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MY_CONNECTION"].ConnectionString))
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("INSERT INTO Products(ProductName, Category, Price, ModifDate, Status) "
                                       + "  VALUES(@ProductName, @Category, @Price, @ModifDate, @Status)", conn);
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@Category", product.Category);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@ModifDate", product.ModifDate);
                    cmd.Parameters.AddWithValue("@Status", product.Status);
                    rowsAff = cmd.ExecuteNonQuery();
                    conn.Close();

                    Console.WriteLine(rowsAff.ToString() + " rows affected.");

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }



        private static void ShowAllData()
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
                                               reader.GetInt32(5).ToString().PadRight(7, ' ') );
                        }
                    }
                    conn.Close();
                }
            }
        }


        //---------------------------------------------------------------------------------------------------//
        // TEST CONNECTION TO SQL SEVER                                                                      //
        //---------------------------------------------------------------------------------------------------//
        private static int TestConnection(int iType)
        {
            SqlConnection connection = null;
            int ok;

            if (iType == 1)
            {
                // Classic Connection String
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MY_CONNECTION"].ConnectionString);

            } else if (iType == 2)
            {
                // String Builder
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = ConfigurationManager.AppSettings["SERVER"];
                builder.InitialCatalog = ConfigurationManager.AppSettings["DATABASE"];
                builder.UserID = ConfigurationManager.AppSettings["USER"];
                builder.Password = ConfigurationManager.AppSettings["PASSWORD"];

                connection = new SqlConnection(builder.ConnectionString);
            }

            if (connection == null)
            {
                ok = 0;
                Console.WriteLine("Successful connection");
            }
            else {
                ok = 1;
                Console.WriteLine("Connection error");
            }
            return ok;
        }


        //private static void getDataTest()
        //{

        //    string productId, productName;

        //    DataTable data= new DataTable();
        //    data = runQuery("SELECT ProductId, ProductName, Category, Price, ModifDate, Status"
        //                  + "  FROM Products;");

        //    foreach (DataRow row in data.Rows)
        //    {
        //        productId = row["ProductId"].ToString();
        //        productName = row["ProductName"].ToString();
        //        Console.WriteLine("{0} {1}", productId, productName);
        //    }
        //    Console.ReadKey();
        //}

        //private static DataTable runQuery(string query)
        //{
        //    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MY_CONNECTION"].ConnectionString);
        //    conn.Open();
        //    SqlDataAdapter adap = new SqlDataAdapter(query, conn);
        //    DataTable tbResult = new DataTable();
        //    adap.Fill(tbResult);
        //    conn.Close();
        //    return tbResult;
        //}

    }
}
