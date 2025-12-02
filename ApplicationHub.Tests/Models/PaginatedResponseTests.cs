using ApplicationHub.Modules.Models;

namespace ApplicationHub.Tests.Models;

public class PaginatedResponseTests
{
    [Fact]
    public void PaginatedResponse_Properties_ShouldBeSetCorrectly()
    {
        var items = new List<string> { "Item1", "Item2" };
        var response = new PaginatedResponse<string>
        {
            Items = items,
            PageNumber = 2,
            PageSize = 10,
            TotalRecords = 50,
            TotalPages = 5
        };

        Assert.Equal(items, response.Items);
        Assert.Equal(2, response.PageNumber);
        Assert.Equal(10, response.PageSize);
        Assert.Equal(50, response.TotalRecords);
        Assert.Equal(5, response.TotalPages);
    }
}