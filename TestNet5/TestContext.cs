using Microsoft.EntityFrameworkCore;

namespace TestNet5
{
    public class TestContext:DbContext
    {
        public DbSet<Book> Books { get; set; }
        public TestContext(DbContextOptions<TestContext> options):base(options)
        {
            
        }
    }
}