namespace ControleDeAcesso.Middlewares.Extensions
{
    public static class InjectionDetectorMiddlewareExtension
    {
        public static IApplicationBuilder UseInjectionDetector(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<InjectionDetectorMiddleware>();
        }
    }
}
