
using Microsoft.EntityFrameworkCore;
using VcsSystem_assignment.Models;

namespace VcsSystem_assignment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employee { get; set; }
    }
}
