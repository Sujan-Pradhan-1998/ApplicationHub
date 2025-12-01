using System.Linq.Expressions;
using ApplicationHub.Data.Repos.IRepos;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationHub.Data.Repos;

public class PaginationBaseRepository<T, TModel> : IPaginationBaseRepository<T, TModel>
    where T : class where TModel : class
{
    public async Task<PaginatedResponse<T>> GetPagedAsync(PaginatedRequest<T, TModel> paginationRequest)
    {
        paginationRequest.PageNumber = Math.Max(1, paginationRequest.PageNumber);
        paginationRequest.PageSize = Math.Max(1, paginationRequest.PageSize);

        var totalRecords = await paginationRequest.Query!.CountAsync();

        var items = await paginationRequest.Query!
            .AsNoTracking()
            .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
            .Take(paginationRequest.PageSize)
            .ToListAsync();

        return new PaginatedResponse<T>
        {
            Items = items,
            PageNumber = paginationRequest.PageNumber,
            PageSize = paginationRequest.PageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)paginationRequest.PageSize)
        };
    }
}