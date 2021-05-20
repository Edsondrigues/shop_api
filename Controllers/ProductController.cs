using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get()
        {
            var products = await _context
                .Products
                .Include(x => x.Category)
                .AsNoTracking().ToListAsync();

            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _context
                .Products
                .Include(x => x.Category)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return Ok(product);
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<ActionResult<List<Product>>> GetByCategory(int id)
        {
            var products = await _context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoryId == id).ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post([FromBody] Product model)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(model);
                await _context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}