using System.Text.Json;
using ApplicationHub.Api.Middlewares;
using ApplicationHub.Modules.Dtos.Responses;
using ApplicationHub.Modules.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApplicationHub.Tests.Middlewares
{
    public class ErrorHandlerMiddlewareTests
    {
        private DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            return context;
        }

        private async Task<ErrorResponse> GetResponseAsync(DefaultHttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Response.Body);
            var responseText = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<ErrorResponse>(responseText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new ErrorResponse();
        }

        [Fact]
        public async Task Invoke_ShouldHandleDaoDbException()
        {
            var loggerMock = new Mock<ILogger<ErrorHandlerMiddleware>>();
            var middleware = new ErrorHandlerMiddleware(loggerMock.Object, _ => throw new DaoDbException("Database failure"));

            var context = CreateHttpContext();

            await middleware.Invoke(context);

            var errorResponse = await GetResponseAsync(context);

            Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
            Assert.Contains("Database Error.", errorResponse.Errors);
        }

        [Fact]
        public async Task Invoke_ShouldHandleApiException()
        {
            var loggerMock = new Mock<ILogger<ErrorHandlerMiddleware>>();
            var middleware = new ErrorHandlerMiddleware(loggerMock.Object, _ => throw new ApiException("API failure"));

            var context = CreateHttpContext();

            await middleware.Invoke(context);

            var errorResponse = await GetResponseAsync(context);

            Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
            Assert.Contains("Api Error.", errorResponse.Errors);
        }

        [Fact]
        public async Task Invoke_ShouldHandleGenericException()
        {
            var loggerMock = new Mock<ILogger<ErrorHandlerMiddleware>>();
            var middleware = new ErrorHandlerMiddleware(loggerMock.Object, _ => throw new Exception("Generic failure"));

            var context = CreateHttpContext();

            await middleware.Invoke(context);

            var errorResponse = await GetResponseAsync(context);

            Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
            Assert.Contains("Internal Server Error.", errorResponse.Errors);
        }
    }
}
