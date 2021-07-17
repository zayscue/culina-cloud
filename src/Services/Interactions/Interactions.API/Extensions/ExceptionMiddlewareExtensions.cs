using System.Dynamic;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace CulinaCloud.Interactions.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appError =>
            {
                const string applicationJsonContentType = "application/json";
                const string errorCodePrefix = "interactions.api-e-";
                const string emptyPayloadErrorMessage = "Unable to read the request as JSON because the request content type '' is not a known JSON content type.";

                appError.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var error = exceptionHandlerPathFeature.Error;
                    dynamic errorResponse = new ExpandoObject();
                    if (error.Message.Equals(emptyPayloadErrorMessage))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = applicationJsonContentType;
                        errorResponse.errorCode = $"{errorCodePrefix}002";
                        errorResponse.message = "Unable to read an empty request as JSON.";
                        if (env.IsDevelopment())
                        {
                            errorResponse.exception = new
                            {
                                message = error.Message,
                                stackTrace = error.StackTrace,
                                source = error.Source
                            };
                        }
                        await context.Response.WriteAsJsonAsync((object)errorResponse);
                        return;
                    }

                    switch (error)
                    {
                        case ValidationException ve:
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}003";
                                errorResponse.message = ve.Message;
                                errorResponse.validationErrors = ve.Errors;
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = ve.Message,
                                        stackTrace = ve.StackTrace,
                                        source = ve.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        case EntityConflictException ce:
                            {
                                context.Response.StatusCode = StatusCodes.Status409Conflict;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}004";
                                errorResponse.message = ce.Message;
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = ce.Message,
                                        stackTrace = ce.StackTrace,
                                        source = ce.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        case NotFoundException ne:
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}005";
                                errorResponse.message = ne.Message;
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = ne.Message,
                                        stackTrace = ne.StackTrace,
                                        source = ne.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        default:
                            {
                                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}001";
                                errorResponse.message = "Internal Server Error.";
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = error.Message,
                                        stackTrace = error.StackTrace,
                                        source = error.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                    }
                });
            });
        }
    }
}
