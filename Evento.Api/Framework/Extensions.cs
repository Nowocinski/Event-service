using Microsoft.AspNetCore.Builder;

namespace Evento.Api.Framework
{
    public static class Extensions
    {
        public static IApplicationBuilder UserExceptionHandler(this IApplicationBuilder builer)
            => builer.UseMiddleware(typeof(ExceptionHandlerMiddleware));
    }
}
