using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET
{
    public class Program
    {
        private static string providerName = ConfigurationManager.AppSettings["provider"];
        private static string connString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        private static Dapper dapper = new Dapper();

        static async Task Main(string[] args)
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

            using (DbConnection connection = factory.CreateConnection())
            {
                if (connection == null)
                {
                    Console.WriteLine("Failed to create connection");
                    return;
                }

                connection.ConnectionString = connString;

                try
                {
                    await connection.OpenAsync();
                    Console.WriteLine("Connection successful");

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
                                          "[13] - show where color red or yellow\n" +
                                          "[14] - search by id\n" +
                                          "[15] - show all data\n" +
                                          "[16] - update element\n" +
                                          "[17] - delet element\n" +
                                          "[18] - cal average\n");

                        int choose;
                        string inputVar = null;
                        string range_start = null;
                        string range_end = null;

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
                                query = "SELECT COUNT(*) FROM Product WHERE Product.type LIKE 'vegetable'";
                                Console.WriteLine("Amount of vegetables: ");
                                break;

                            case 8:
                                query = "SELECT COUNT(*) FROM Product WHERE Product.type LIKE 'fruit'";
                                Console.WriteLine("Amount of fruits: ");
                                break;

                            case 9:
                                Console.WriteLine("Enter ur color: ");
                                inputVar = Console.ReadLine();
                                query = "SELECT COUNT(*) FROM Product WHERE color LIKE @color";
                                Console.WriteLine("Amount of color " + inputVar + ": ");
                                break;

                            case 10:
                                query = "SELECT color, COUNT(*) FROM Product GROUP BY color";
                                break;

                            case 11:
                                Console.WriteLine("Enter end of range: ");
                                inputVar = Console.ReadLine();
                                query = "SELECT * FROM Product WHERE cal > @inputVar";
                                break;

                            case 12:
                                Console.WriteLine("Enter begin of range: ");
                                range_start = Console.ReadLine();

                                Console.WriteLine("Enter end of range: ");
                                range_end = Console.ReadLine();
                                query = "SELECT * FROM Product WHERE cal > @range_start AND cal < @range_end";
                                break;

                            case 13:
                                query = "SELECT * FROM Product WHERE color LIKE 'yellow' OR color LIKE 'red'";
                                break;

                            case 14:
                                dapper.GetProductById(1, connString);
                                break;

                            case 15:
                                dapper.GetProducts(connString);
                                break;

                            case 16:
                                dapper.UpdateProduct(1, "pineapple", "fruit", "green", 12, connString);
                                break;

                            case 17:
                                dapper.DeleteProduct(1, connString);
                                break;

                            case 18:
                                dapper.GetAverageCalories(connString);
                                break;
                        }

                        using (DbCommand command = factory.CreateCommand())
                        {
                            if (command == null)
                            {
                                Console.WriteLine("Failed to create command.");
                                return;
                            }

                            command.Connection = connection;
                            command.CommandText = query;

                            if (choose == 9 || choose == 11 || choose == 12)
                            {
                                if (choose == 9)
                                {
                                    command.Parameters.Add(CreateParameter("color", DbType.String, inputVar));
                                }

                                else if (choose == 11)
                                {
                                    command.Parameters.Add(CreateParameter("cal", DbType.Int32, inputVar));
                                }

                                else if (choose == 12)
                                {
                                    command.Parameters.Add(CreateParameter("range_start", DbType.Int32, range_start));
                                    command.Parameters.Add(CreateParameter("range_end", DbType.Int32, range_end));
                                }
                            }

                            Stopwatch stopwatch = new Stopwatch();
                            stopwatch.Start();

                            using (DbDataReader reader = await command.ExecuteReaderAsync())
                            {
                                stopwatch.Stop();
                                TimeSpan ts = stopwatch.Elapsed;

                                if (reader.HasRows)
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            Console.Write(reader[i] + " ");
                                        }
                                        Console.WriteLine();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No data found");
                                }

                                Console.WriteLine($"Execution Time: {ts.TotalMilliseconds} milliseconds");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        private static DbParameter CreateParameter(string name, DbType type, object value)
        {
            DbParameter parameter = DbProviderFactories.GetFactory(providerName).CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Value = value;
            return parameter;
        }
    }
}
