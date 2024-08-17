using DotNetCore.Domain.Models;
using DotNetCore.Domain.RepositoriesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DotNetCore.Persistance.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(string connectionstring) : base(connectionstring)
        {
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            List<Product> products = new List<Product>();
            using (var connection = await GetOpenConnectionAsync())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select * from Product";


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Product product = new()
                            {
                                Id = (int)reader[0],
                                Name = (string)reader[1],
                                Sku = (string)reader[2],

                            };
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            int id = 0;
            using (var connection = await GetOpenConnectionAsync())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "InsertIntoProduct";

                    command.Parameters.AddWithValue("Name", product.Name);
                    command.Parameters.AddWithValue("Sku", product.Sku);

                    //Get the Id of the row added to the table
                    id = await command.ExecuteNonQueryAsync();
                }
            }
            return id != 0;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            using (var connection = await GetOpenConnectionAsync())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetProductById";

                    command.Parameters.AddWithValue("Id", id);


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Product product = new()
                            {
                                Id = (int)reader[0],
                                Name = (string)reader[1],
                                Sku = (string)reader[2],

                            };
                            return product;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<bool> UpdateProductAsync(Product oldProduct)
        {
            int rawsAffected = 0;
            using (var connection = await GetOpenConnectionAsync())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "UpdateProduct";

                    command.Parameters.AddWithValue("Id", oldProduct.Id);
                    command.Parameters.AddWithValue("Name", oldProduct.Name);
                    command.Parameters.AddWithValue("Sku", oldProduct.Sku);

                    rawsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rawsAffected != 0;
        }
        public async Task<bool> RemoveProductByIdAsync(int id)
        {
            int rawsAffected = 0;

            using (var connection = await GetOpenConnectionAsync())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "RemoveProduct";

                    command.Parameters.AddWithValue("Id", id);

                    rawsAffected = await command.ExecuteNonQueryAsync();
                }
            }

            return rawsAffected != 0;
        }
    }
}
