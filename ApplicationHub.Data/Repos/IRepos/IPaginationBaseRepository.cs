using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Models;

namespace ApplicationHub.Data.Repos.IRepos;

public interface IPaginationBaseRepository<T, TModel> where T : class where TModel : class
{
    Task<PaginatedResponse<T>> GetPagedAsync(PaginatedRequest<T, TModel> paginationRequest);
}