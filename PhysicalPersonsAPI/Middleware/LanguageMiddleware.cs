using System.Globalization;

namespace PhysicalPersonsAPI.Middleware
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;
        
        public LanguageMiddleware(RequestDelegate next)
        {
           _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();
            if (acceptLanguage == "ka")
            {
                var culture = new CultureInfo("ka");
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }
            else if (acceptLanguage == "en")
            {
                var culture = new CultureInfo("en");
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }

            await _next(context);
        }
    }
}
