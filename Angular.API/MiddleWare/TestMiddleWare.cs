using Angular.API.Errors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Angular.API.MiddleWare
{

    public class TestMiddleWare
    {
        private readonly RequestDelegate _request;
        private readonly ILogger<TestMiddleWare> _logger;
        private readonly IHostEnvironment _env;

        public TestMiddleWare(RequestDelegate request, ILogger<TestMiddleWare> logger, IHostEnvironment env)
        {
            _request = request;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ApiException apiException = _env.IsDevelopment()
                    ? new ApiException((int)HttpStatusCode.InternalServerError, e.Message, e.StackTrace)
                    : new ApiException((int)HttpStatusCode.InternalServerError, "Internal Server Error");
                var jsonFormat = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonResult = JsonSerializer.Serialize(apiException, jsonFormat);
                await context.Response.WriteAsync(jsonResult);
            }

        }
    }
}
