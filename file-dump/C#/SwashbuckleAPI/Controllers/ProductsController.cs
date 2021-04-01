using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using Swashbuckle.AspNetCore.Filters;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <remarks>
        /// Get all products from the store catalogue
        /// </remarks>
        /// <returns>List of all the products</returns>
        [HttpGet]
        [SwaggerResponseExample(200, typeof(Product.Create.ExampleList))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Get product information
        /// </summary>
        /// <remarks>
        /// Get informations about a Product from a store catalogue
        /// </remarks>
        /// <param name="id">Id of the product</param>
        /// <returns name="product">Product with a specified id or error status code</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [SwaggerResponseExample(200, typeof(Product.Example))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        /// Update existing Product
        /// </summary>
        /// <remarks>
        /// Updates an existing Product in a store catalogue.
        /// </remarks>
        /// <param name="id">Id of the Product to be updated</param>
        /// <param name="product">Product object with new values</param>
        /// <returns>Status code</returns> 
        [SwaggerRequestExample(typeof(Product.Create), typeof(Product.Create.Example))]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Create new Product
        /// </summary>
        /// <remarks>
        /// Creates a new product in a store catalogue.
        /// </remarks>
        /// <param name="ProductCreateSettings">Create settings for new Product</param>
        /// <returns name="newProduct">Returns created Product</returns>
        [SwaggerResponseExample(201, typeof(Product.Example))]
        [SwaggerRequestExample(typeof(Product.Create), typeof(Product.Create.Example))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product.Create ProductCreateSettings)
        {
            Product newProduct = new Product();
            newProduct.EanCode = ProductCreateSettings.EanCode;
            newProduct.Name = ProductCreateSettings.Name;
            newProduct.Price = ProductCreateSettings.Price;
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
        }

        /// <summary>
        /// Deletes product
        /// </summary>
        /// <remarks>
        /// Deletes specific product from the stores catalogue
        /// </remarks>
        /// <param name="id">Id of a product to be deleted</param>
        /// <returns name="product">Returns deleted product or error status code</returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(200, typeof(Product.Example))]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
