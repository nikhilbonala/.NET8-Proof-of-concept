


using Microsoft.EntityFrameworkCore;
using Test1LearnNewVersion.Models.Entities;

namespace Test1LearnNewVersion.Data
{
    public class LearnDbContext : DbContext
    {
        public LearnDbContext(DbContextOptions dbContextOptions): base(dbContextOptions) 
        {
                
        }

        public DbSet<user> Users { get; set; }

    }
}
