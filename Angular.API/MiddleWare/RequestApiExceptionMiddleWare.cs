using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular.API.MiddleWare
{
    public static class RequestApiExceptionMiddleWare
    {
        public static IApplicationBuilder UseApiExceptionMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleWare>();
        }
    }
}
