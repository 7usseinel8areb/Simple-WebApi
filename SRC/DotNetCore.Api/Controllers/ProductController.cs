using DotNetCore.Domain.Models;
using DotNetCore.Domain.RepositoriesInterface;
using DotNetCore_WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace DotNetCore_WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("GetSingle/{key:int}")]
        public async Task<IActionResult> GetProduct([FromRoute(Name = "Key")] int id)
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
        [Authorize]
        //[AllowAnonymous] => This if the whole controller have [Authorize] attribute and we want to remove the validation from this action
        public async Task<IActionResult> GetAllProducts()
        {
            var userName = User.Identity.Name;


            /*//To get the claims you have added
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Debug.WriteLine($"{userId}:{userName}");*/

            List<Product>? products = (await _productRepository.GetProductsAsync()).ToList();

            return StatusCode(StatusCodes.Status200OK, products);
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateProduct(/*[FromQuery] */Product product/*, [FromQuery] Product product2*/)
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
