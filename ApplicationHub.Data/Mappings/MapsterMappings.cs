using ApplicationHub.Modules.Dtos.Responses;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Models;
using Mapster;

namespace ApplicationHub.Data.Mappings;

public class MapsterMappings
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<ApplicationFormResponse, ApplicationForm>.NewConfig();

        TypeAdapterConfig<PaginatedResponse<ApplicationFormResponse>, PaginatedResponse<ApplicationForm>>.NewConfig()
            .Map(dest => dest.Items, src => src.Items.Adapt<IEnumerable<ApplicationForm>>());
    }
}