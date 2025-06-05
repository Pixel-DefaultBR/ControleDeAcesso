using System.Text;
using System.Text.RegularExpressions;

namespace ControleDeAcesso.Middlewares
{
    public class InjectionDetectorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<InjectionDetectorMiddleware> _logger;
        public InjectionDetectorMiddleware(RequestDelegate next, ILogger<InjectionDetectorMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check for SQL injection patterns in the request path
            var request = context.Request;
            var queryString = request.QueryString.Value;
            if (!string.IsNullOrEmpty(queryString) && (queryString.Contains("'") || queryString.Contains("--") || queryString.Contains(";")))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Acesso Negado: Nada para ver aqui.");
                return;
            }

            // Check for SQL injection patterns in the request body
            if (context.Request.Method is "POST" or "PUT" or "DELETE" &&
                context.Request.ContentType?.Contains("application/json") == true)
            {
                context.Request.EnableBuffering();

                using var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true);

                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                if (ContainsInjectionPatterns(body))
                {
                    await BlockAccess(context, "Acesso Negado: Nada para ver aqui.", body);
                    return;
                }
            }

            await _next(context);
        }

        private async Task BlockAccess(HttpContext context, string message, string payload = null)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            _logger.LogWarning($"[!] Access blocked: {message} | IP: {ipAddress}");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync(message);
        }

        private bool ContainsInjectionPatterns(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            input = input.ToLowerInvariant();

            var patterns = new List<string>
            {
                @"(\%27)|(\')|(\-\-)|(\%23)|(#)",
                @"(\b(select|union|insert|delete|update|drop|exec|truncate|alter|create|rename)\b)",
                @"(\b(or|and)\b\s+\d+\s*=\s*\d+)",
                @"(\b(or|and)\b\s+\w+\s*=\s*\w+)",
                @"('.+--)",
                @"(\bwaitfor\s+delay\b|\bsleep\s*\()",
                @"(\bchar\s*\(|\bconcat\s*\()",
                @"(\bxp_cmdshell\b|\bexec\s+xp_)",
                @"(\b0x[0-9a-fA-F]+)",
            };

            foreach (var pattern in patterns)
            {
                if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
