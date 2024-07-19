using App.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Serilog;
using System.Diagnostics;

namespace App.Handlers
{
    public class ExceptionHandlingMiddleware : IMiddleware   
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFound)
            {

                Log.Error(notFound.Message);

                context.Response.StatusCode = 404;
               await context.Response.WriteAsync(notFound.Message);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);

                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ex.Message);
            }
            

        }


    }
}
