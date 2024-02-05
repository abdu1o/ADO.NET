using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET
{
    public class Program
    {
        static void Main(string[] args)
        {
            string conn = @"Data Source=DESKTOP-GA6HPG7; Initial Catalog=Fruits and vegetables; Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Complete");

                    string query = null;

                    while (true) 
                    { 
                        Console.WriteLine("[1] - all info\n" +
                            "[2] - all names\n" +
                            "[3] - all colors\n" +
                            "[4] - max callories\n" +
                            "[5] - min callories\n" +
                            "[6] - avg callories\n" +
                            "[7] - vegetables count\n" +
                            "[8] - fruits count\n" +
                            "[9] - show by color\n" +
                            "[10] - show count by color\n" +
                            "[11] - show by cal >\n" +
                            "[12] - show by cal in range\n" +
                            "[13] - show where color red or yellow\n");

                        int choose;
                        choose = Convert.ToInt32(Console.ReadLine());

                        switch (choose) 
                        {
                            case 1:
                                query = "SELECT * FROM Product";
                                break;

                            case 2:
                                query = "SELECT name FROM Product";
                                break;

                            case 3:
                                query = "SELECT color FROM Product";
                                break;

                            case 4:
                                query = "SELECT MAX(cal) FROM Product";
                                break;

                            case 5:
                                query = "SELECT MIN(cal) FROM Product";
                                break;

                            case 6:
                                query = "SELECT AVG(cal) FROM Product";
                                break;

                            case 7:
                                query = "SELECT COUNT(*) FROM Product\r\nWHERE Product.type LIKE 'vegetable'";
                                Console.WriteLine("Amount of vegetables: ");
                                break;

                            case 8:
                                query = "SELECT COUNT(*) FROM Product\r\nWHERE Product.type LIKE 'fruit'";
                                Console.WriteLine("Amount of fruits: ");
                                break;

                            case 9:
                                Console.WriteLine("Enter ur color: ");
                                string color = Console.ReadLine();
                                query = "SELECT COUNT(*) FROM Product\r\nWHERE color LIKE '" + color + "'";
                                Console.WriteLine("Amount of color " + color + ": ");
                                break;

                            case 10:
                                query = "SELECT color, COUNT(*) FROM Product\r\nGROUP BY color";
                                break;

                            case 11:
                                Console.WriteLine("Enter end of range: ");
                                string cal = Console.ReadLine();
                                query = "SELECT * FROM Product WHERE cal > " + cal;
                                break;

                            case 12:
                                Console.WriteLine("Enter begin of range: ");
                                string range_start = Console.ReadLine();

                                Console.WriteLine("Enter end of range: ");
                                string range_end = Console.ReadLine();
                                query = "SELECT * FROM Product\r\nWHERE cal > " + range_start + " AND cal < " + range_end;
                                break;

                            case 13:
                                query = "SELECT * FROM Product\r\nWHERE color LIKE 'yellow' OR color LIKE 'red'";
                                break;
                        }

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        Console.Write(reader[i] + " ");
                                    }
                                    Console.WriteLine();
                                }
                            }
                        }
                    }  
                }
                catch(Exception ex)
                {

                    Console.WriteLine("Error" + ex.Message);
                }
            }
        }
    }
}
