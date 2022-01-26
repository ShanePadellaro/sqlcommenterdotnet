using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestNetCore31
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly TestContext _context;

        public BookController(TestContext context)
        {
            _context = context;
        }
        // GET: api/Book
        [HttpGet]
        public async Task<List<Book>> Get()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Book/5
        [HttpGet("create")]
        public async Task Create(int id)
        {
            var book = new Book() {Name = "NewBook"};
            _context.Add(book);
            await _context.SaveChangesAsync();
        }

        
    }
}