using ApplicationHub.Data.Mappings;
using ApplicationHub.Modules.Dtos.Responses;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Enums;
using ApplicationHub.Modules.Models;
using Mapster;

namespace ApplicationHub.Tests.Data.Mappings
{
    public class MapsterMappingsTests
    {
        public MapsterMappingsTests()
        {
            MapsterMappings.RegisterMappings();
        }

        [Fact]
        public void ApplicationFormResponse_Should_Map_To_ApplicationForm()
        {
            var response = new ApplicationFormResponse
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Company = "test",
                Position = "test",
                FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
            };

            var result = response.Adapt<ApplicationForm>();

            Assert.Equal(response.Id, result.Id);
        }

        [Fact]
        public void PaginatedResponse_Should_Map_Correctly()
        {
            var paginatedResponse = new PaginatedResponse<ApplicationFormResponse>
            {
                Items = new List<ApplicationFormResponse>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        UserId = Guid.NewGuid(),
                        Company = "test",
                        Position = "test",
                        FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        UserId = Guid.NewGuid(),
                        Company = "test",
                        Position = "test",
                        FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
                    }
                },
                TotalRecords = 2,
                PageSize = 10,
                PageNumber = 1
            };

            var result = paginatedResponse.Adapt<PaginatedResponse<ApplicationForm>>();

            Assert.Equal(paginatedResponse.TotalRecords, result.TotalRecords);
            Assert.Equal(paginatedResponse.PageSize, result.PageSize);
            Assert.Equal(paginatedResponse.PageNumber, result.PageNumber);
        }
    }
}