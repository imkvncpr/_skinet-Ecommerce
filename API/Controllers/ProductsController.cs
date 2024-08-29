using API.RequestHelpers;
using Core.Entites;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class ProductsController(IGenericRepository<Product> repo) : BaseAPIController
    {

        [HttpGet]

        public async Task<ActionResult<Product>> GetProducts(
            [FromQuery]ProductSpecParams specParams)
        {
            var spec = new ProductSortPagSpecifcation(specParams);
        
            return await CreatePageResult(repo, spec, specParams.PageIndex, specParams.PageSize);
        }
        
        
        [HttpGet("{id:int}")] // api/products/1
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);

            if(product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.Add(product);
            if (await repo.SaveAllAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            return BadRequest("Failed to add product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
                if (product.Id != id || !ProductExists(id))
                return BadRequest("Invalid product");

            repo.Update(product);
            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Failed to update product");
                        
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            repo.Remove(product);
            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Failed to delete product");
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await repo.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await repo.ListAsync(spec));
        
        }
          
        private bool ProductExists(int id)
        {
            return repo.EntityExists(id);
        }
        
    }
}