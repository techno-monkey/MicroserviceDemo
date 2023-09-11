using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogService.Database;
using AutoMapper;
using CatalogService.Services;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogAPIController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ICommunicationDataClient _dataClient;

        public CatalogAPIController(DatabaseContext context, ICommunicationDataClient dataClient)
        {
            _context = context;
            _dataClient = dataClient;
        }

        // GET: api/CatalogAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            return await _context.Category.ToListAsync();
        }

        // GET: api/CatalogAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpGet("/api/CatalogAPI/name/{name}")]
        public async Task<ActionResult<Category>> GetCategoryByName(string name)
        {
            if (_context.Category == null)
            {
                return NotFound();
            }

            var category = _context.Category.FromSqlInterpolated($"SELECT * FROM dbo.Category WHERE Name = {name}").SingleOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/CatalogAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/CatalogAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
           

            if (_context.Category == null)
            {
                return Problem("Entity set 'DatabaseContext.Category'  is null.");
            }
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            try
            {
                await _dataClient.SendCategoryToCommunication(new CategoryDto { CategoryId = category.CategoryId, Name = category.Name });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed: {ex.Message}");
            }
            Console.WriteLine($"Success");

            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        }

        // DELETE: api/CatalogAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
