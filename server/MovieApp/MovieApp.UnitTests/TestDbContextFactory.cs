using Microsoft.EntityFrameworkCore;
using MovieApp.Infrastructure.Persistence;

namespace MovieApp.UnitTests;

public static class TestDbContextFactory
{
    public static MovieAppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<MovieAppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new MovieAppDbContext(options);
    }
}