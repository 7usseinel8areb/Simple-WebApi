using DotNetCore.Domain.Models;
using DotNetCore.Domain.RepositoriesInterface;
using DotNetCore_WebApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace DotNetCore_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("GetSingle/{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            Product? product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
                return StatusCode(StatusCodes.Status404NotFound, "This Product was not found");

            return StatusCode(StatusCodes.Status200OK, product);
        }
        
        [HttpGet]
        [Route("GetAll")]
        //Add Filter on one or many controllers or action
        //Sensetive Action
        [LogSensetiveActionAttributeFilter]
        public async Task<IActionResult> GeAlltProducts()
        {
            List<Product>? products = (await _productRepository.GetProductsAsync()).ToList();

            return StatusCode(StatusCodes.Status200OK, products);
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                bool added = await _productRepository.CreateProductAsync(product);

                if (added)
                {
                    return StatusCode(StatusCodes.Status201Created, "Product created successfully");
                }
                return StatusCode(StatusCodes.Status400BadRequest,"Product can't be created");
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id,Product product)
        {
            if(ModelState.IsValid)
            {
                Product? oldProduct = await _productRepository.GetProductByIdAsync(id);

                if (oldProduct == null)
                    return StatusCode(StatusCodes.Status404NotFound, "This Product was not found");

                bool Updated = await _productRepository.UpdateProductAsync(product);
                if (Updated)
                    return StatusCode(StatusCodes.Status204NoContent, "Product updated successfully");
                return StatusCode(StatusCodes.Status400BadRequest,"Can't update this product");
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            Product? product = await _productRepository.GetProductByIdAsync(id);

            if(product == null)
                return StatusCode(StatusCodes.Status404NotFound, "This Product was not found");
           
            bool deleted = await _productRepository.RemoveProductByIdAsync(id);

            if (deleted)
                return StatusCode(StatusCodes.Status204NoContent, "This Product was deleted successfully");

            return StatusCode(StatusCodes.Status400BadRequest, "Can't delete this product");

        }
    }
}
