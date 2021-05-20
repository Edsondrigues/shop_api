using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;
        public CategoryController(DataContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();

            if (categories == null) return BadRequest("Categoria nao encontrada!");
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (category == null) return BadRequest("Categoria nao encontrada!");
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Post([FromBody] Category model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                _context.Categories.Add(model);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Nao foi possivel criar uma categoria!" });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] Category model)
        {
            if (id != model.Id)
                return NotFound(new { message = "Categoria não encontrada" });

            // Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _context.Entry<Category>(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Não foi possível atualizar a categoria" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Nao foi possivel atualizar a categoria!" });
            }
        }
    }
}