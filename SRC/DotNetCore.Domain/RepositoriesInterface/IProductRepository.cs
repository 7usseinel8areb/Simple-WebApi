using DotNetCore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Domain.RepositoriesInterface
{
    public interface IProductRepository
    {
        Task<bool> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product oldProduct);

        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync();


        Task<bool> RemoveProductByIdAsync(int id);
    }
}
