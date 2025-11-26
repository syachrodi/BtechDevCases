using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TakeHomeAssignment.General.AccountAPI.Model.General;
using TakeHomeAssignment.General.AccountAPI.Utils;

namespace TakeHomeAssignment.General.AccountAPI.Middlewares
{
    public class InactivityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TimeSpan _idleLimit;

        public InactivityMiddleware(RequestDelegate next, IOptions<ApplicationSetting> appSettings)
        {
            _next = next;
            _idleLimit = TimeSpan.FromMinutes(int.Parse(appSettings.Value.JWTIdleMinutes));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                string? email = context.User.FindFirst(ClaimTypes.Email)?.Value;

                if (!string.IsNullOrEmpty(email))
                {
                    var now = DateTime.UtcNow;

                    if (SessionStore.UserActivity.TryGetValue(email, out var lastActivity))
                    {
                        if (now - lastActivity > _idleLimit)
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Session expired due to inactivity.");
                            return;
                        }
                    }

                    // update last activity
                    SessionStore.UserActivity[email] = now;
                }
            }

            await _next(context);
        }
    }
}
