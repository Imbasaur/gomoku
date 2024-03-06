using Gomoku.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gomoku.DAL;
public class GomokuDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("cs");
}
