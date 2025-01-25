namespace CatalogService.Presentation.Middleware
{
    public static class ExceptionHandlerMiddlewareExtention
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
