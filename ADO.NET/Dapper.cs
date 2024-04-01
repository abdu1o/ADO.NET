using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Runtime.Remoting.Contexts;
using System.Collections;

namespace ADO.NET
{
    public class Dapper
    {
        public IEnumerable<Product> GetProducts(string conn)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                var products = db.Query<Product>("GetAll", commandType: CommandType.StoredProcedure);

                foreach (var product in products)
                {
                    Console.WriteLine($"Name: {product.name} " +
                        $"Type: {product.type} " +
                        $"Color: {product.color} " +
                        $"Callories: {product.cal} \n");
                }

                return products;
            }
        }


        public Product GetProductById(int productId, string conn)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                var product = db.QueryFirstOrDefault<Product>("GetProductById", new { ProductId = productId }, commandType: CommandType.StoredProcedure);

                Console.WriteLine($"Name: {product.name} " +
                        $"Type: {product.type} " +
                        $"Color: {product.color} " +
                        $"Callories: {product.cal} \n");

                return product;
            }
        }

        public Product GetProdasductById(int productId, string conn)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                var product = db.QueryFirstOrDefault<Product>("GetProductById", new { ProductId = productId }, commandType: CommandType.StoredProcedure);

                Console.WriteLine($"Name: {product.name} " +
                        $"Type: {product.type} " +
                        $"Color: {product.color} " +
                        $"Callories: {product.cal} \n");

                return product;
            }
        }

        public void UpdateProduct(int productId, string name, string type, string color, double cal, string conn)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                var parameters = new
                {
                    ProductId = productId,
                    Name = name,
                    Type = type,
                    Color = color,
                    Cal = cal
                };

                db.Execute("UpdateProduct", parameters, commandType: CommandType.StoredProcedure);
            }

            Console.WriteLine("Product successfully updated");
        }

        public void DeleteProduct(int productId, string conn)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                var parameters = new
                {
                    ProductId = productId
                };

                db.Execute("DeleteProduct", parameters, commandType: CommandType.StoredProcedure);
            }

            Console.WriteLine("Product successfully deleted");
        }

        public double GetAverageCalories(string conn)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                Console.WriteLine("Average calories: " + db.ExecuteScalar<double>("GetAverageCalories", commandType: CommandType.StoredProcedure));

                return db.ExecuteScalar<double>("GetAverageCalories", commandType: CommandType.StoredProcedure);
            }
        }

    }
}
