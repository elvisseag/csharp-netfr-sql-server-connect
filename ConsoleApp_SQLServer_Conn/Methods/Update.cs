using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_SQLServer_Conn
{
    public class Update
    {


        //---------------------------------------------------------------------------------------------------//
        // UPDATE RECORD METHOD                                                                              //
        //---------------------------------------------------------------------------------------------------//
        public static void UpdateRecord(Product product)
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



    }
}
