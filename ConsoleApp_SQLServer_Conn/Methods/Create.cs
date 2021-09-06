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
    public class Create
    {


        //---------------------------------------------------------------------------------------------------//
        // CREATE RECORD METHOD                                                                              //
        //---------------------------------------------------------------------------------------------------//
        public static void CreateRecord(Product product)
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




        //---------------------------------------------------------------------------------//
        // BULK COPY (INSERT)                                                              //
        //---------------------------------------------------------------------------------//
        public static void UploadDataToDB(DataTable dt)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["MY_CONNECTION"].ToString();

            try
            {
                using (SqlBulkCopy sbc = new SqlBulkCopy(strConnString))
                {
                    sbc.BatchSize = 10000;
                    sbc.BulkCopyTimeout = 10000;
                    //Columnas desde SAP a DB
                    sbc.ColumnMappings.Add("PRODUCT_NAME", "ProductName"); // (campo_dt, campo_db)
                    sbc.ColumnMappings.Add("CATEGORY", "Category");

                    sbc.DestinationTableName = "PRODUCTS";// Tabla de BD
                    sbc.WriteToServer(dt);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

    }
}
