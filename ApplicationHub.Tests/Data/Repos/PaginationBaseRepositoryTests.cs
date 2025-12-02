using ApplicationHub.Data.Repos;
using ApplicationHub.Modules.Dtos.Requests;
using Microsoft.EntityFrameworkCore;

namespace ApplicationHub.Tests.Data.Repos;

public class PaginationBaseRepositoryTests
{
    private class TestEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
        public DbSet<TestEntity> TestEntities { get; set; }
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPagination()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        await using var context = new TestDbContext(options);
        context.TestEntities.AddRange(Enumerable.Range(1, 10).Select(i => new TestEntity { Id = i, Name = $"Name{i}" }));
        await context.SaveChangesAsync();

        var repository = new PaginationBaseRepository<TestEntity, TestEntity>();

        var request = new PaginatedRequest<TestEntity, TestEntity>
        {
            PageNumber = 2,
            PageSize = 3,
            Query = context.TestEntities.AsQueryable()
        };

        var result = await repository.GetPagedAsync(request);

        Assert.Equal(2, result.PageNumber);
        Assert.Equal(3, result.PageSize);
        Assert.Equal(10, result.TotalRecords);
        Assert.Equal(4, result.TotalPages);
        Assert.Equal(new[] { 4, 5, 6 }, result.Items.Select(x => x.Id));
    }
}