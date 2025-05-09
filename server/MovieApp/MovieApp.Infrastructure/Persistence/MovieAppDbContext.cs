using Microsoft.EntityFrameworkCore;
using MovieApp.Domain.Models;

namespace MovieApp.Infrastructure.Persistence;

public class MovieAppDbContext : DbContext
{
    public MovieAppDbContext(DbContextOptions<MovieAppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieAppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}