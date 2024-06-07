using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataIndexingService.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Logging;
using DataIndexingService.Interfaces;

namespace DataIndexingService.Services
{
    public class SqlDataService : IDataRetriever<Products>
    {
        private readonly string _connectionString;
      

        public SqlDataService(string connectionString)
        {
            _connectionString = connectionString;
         
        }


        public async Task<List<Products>> RetrieveDataAsync()
        {
            var data = new List<Products>();
            try
            {
                
               

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT Id, Name, Description, Price, Category FROM Products";
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data.Add(new Products
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                Category = reader.GetString(4)
                            });
                        }
                    }
                }

               
            
            }
            catch (Exception ex)
            {
               // throw ex.Message;
            }
            return data;
        }

        

        //public async Task<int> InsertProductAsync(Product product)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var command = new SqlCommand("INSERT INTO Products (Name, Description, Price, Category) OUTPUT INSERTED.Id VALUES (@Name, @Description, @Price, @Category)", connection);
        //        command.Parameters.AddWithValue("@Name", product.Name);
        //        command.Parameters.AddWithValue("@Description", product.Description);
        //        command.Parameters.AddWithValue("@Price", product.Price);
        //        command.Parameters.AddWithValue("@Category", product.Category);

        //        return (int)await command.ExecuteScalarAsync();
        //    }
        //}

        //public async Task UpdateProductAsync(Product product)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var command = new SqlCommand("UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, Category = @Category WHERE Id = @Id", connection);
        //        command.Parameters.AddWithValue("@Id", product.Id);
        //        command.Parameters.AddWithValue("@Name", product.Name);
        //        command.Parameters.AddWithValue("@Description", product.Description);
        //        command.Parameters.AddWithValue("@Price", product.Price);
        //        command.Parameters.AddWithValue("@Category", product.Category);

        //        await command.ExecuteNonQueryAsync();
        //    }
        //}
        //public async Task DeleteProductAsync(int id)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var command = new SqlCommand("DELETE FROM Products WHERE Id = @Id", connection);
        //        command.Parameters.AddWithValue("@Id", id);

        //        await command.ExecuteNonQueryAsync();
        //    }
        //}
    }
}
