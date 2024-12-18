using Microsoft.EntityFrameworkCore;
using tp2tuller.Models;

namespace tp2tuller.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
    }
}
