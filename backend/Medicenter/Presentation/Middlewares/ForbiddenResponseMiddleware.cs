using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Presentation.Middlewares
{
    public class ForbiddenResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ForbiddenResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            // Si el middleware de autorización generó un 403, personalizamos la respuesta
            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"Acceso denegado: no tenés permisos para esta acción.\"}");
            }
            else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"No estás autenticado o tu token es inválido.\"}");
            }
        }
    }
}
