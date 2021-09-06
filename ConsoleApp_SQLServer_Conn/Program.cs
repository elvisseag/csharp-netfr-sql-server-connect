using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace ConsoleApp_SQLServer_Conn
{
    class Program
    {
        

        static void Main(string[] args)
        {

            Console.WriteLine("Probando conexión ...");
            // test connection
            int test = TestConnection(1);
            System.Threading.Thread.Sleep(1000);
            if (test == 1)
            {
                Console.Clear();
            }

            Product product = new Product();

            Console.WriteLine("|=========================================================|");
            Console.WriteLine("|====== WELCOME TO SQL SERVER CONNECTION FROM C# =========|");
            Console.WriteLine("|=========================================================|");
            Console.WriteLine();

            // Get all data (default)
            Read.ShowAllData();

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
                        Read.ShowAllData();
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
                        Create.CreateRecord(product);
                        break;
                    case "R":
                        Console.WriteLine("Ingrese código del producto:");
                        product.ProductId = Convert.ToInt32(Console.ReadLine());
                        Read.ReadRecord(product);
                        break;
                    case "U":
                        Console.WriteLine("Ingrese código del producto a actualizar:");
                        product.ProductId = Convert.ToInt32(Console.ReadLine());
                        Product p = Read.ReadRecordTemp(product);

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
                        Update.UpdateRecord(product);
                        break;
                    case "D":
                        Console.WriteLine("Ingrese código del producto a eliminar:");
                        product.ProductId = Convert.ToInt32(Console.ReadLine());
                        Delete.DeleteRecord(product);
                        break;
                    case "EXIT":
                        Console.WriteLine("BYE");
                        break;
                    default:
                        break;
                }

            }

            // EXTRAS --------------------------------------- { //
                // TestRunQuery();
                // TestUploadDataToDB();
            // } ----------------------------------------------//

        }

        private static void TestUploadDataToDB()
        {
            DataTable data = new DataTable();
            data.Columns.Add("PRODUCT_NAME");
            data.Columns.Add("CATEGORY");

            DataRow row = null;
            
            row = data.NewRow();
            row["PRODUCT_NAME"] = "Fanta";
            row["CATEGORY"]    = "Gaseosa";
            data.Rows.Add(row);

            Create.UploadDataToDB(data);
        }


        private static void TestRunQuery()
        {
            DataTable result = new DataTable();

            result = Read.runQuery("SELECT ProductName, Category FROM Products;");

            foreach (DataRow row in result.Rows)
            {
                string nombre = row["ProductName"].ToString();
                string categoria = row["Category"].ToString();
                Console.WriteLine("{0} {1}", nombre, categoria);
            }
            Console.ReadKey();
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
                Console.WriteLine("Connection error");
            }
            else {
                ok = 1;
                Console.WriteLine("Successful connection");
            }
            return ok;
        }

    }
}
