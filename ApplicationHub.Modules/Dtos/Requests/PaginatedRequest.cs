using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApplicationHub.Modules.Dtos.Requests;

public class PaginatedRequest<T,TModel> where T : class where TModel : class
{
    [BindNever]
    [JsonIgnore] public IQueryable<T>? Query { get; set; }
    private int? _pageNumber;

    [FromQuery]
    public int PageNumber
    {
        get => _pageNumber ?? 1;
        set => _pageNumber = value;
    }

    private int? _pageSize;

    [FromQuery]
    public int PageSize
    {
        get => _pageSize ?? 10;
        set => _pageSize = value;
    }
    
    [FromQuery] public TModel? Model { get; set; }
}