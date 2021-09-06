using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_SQLServer_Conn
{
    public class Delete
    {


        //---------------------------------------------------------------------------------------------------//
        // DELETE RECORD METHOD                                                                              //
        //---------------------------------------------------------------------------------------------------//
        public static void DeleteRecord(Product product)
        {
            int rowsAff = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MY_CONNECTION"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE Products WHERE ProductId = @ProductId", conn))
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


    }
}
