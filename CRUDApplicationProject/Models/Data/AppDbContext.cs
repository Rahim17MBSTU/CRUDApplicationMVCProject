using Microsoft.EntityFrameworkCore;

namespace CRUDApplicationProject.Models.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options):base(options)
        {}
        public DbSet<Person> Persons { get; set; }
    }
}
