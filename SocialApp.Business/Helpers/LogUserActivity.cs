using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SocialApp.DataAccess.Interfaces;

namespace SocialApp.Business.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var dbContext = resultContext.HttpContext.RequestServices.GetService<IAppDataAccess>();

            var user = await dbContext.GetUser(userId, true);
            user.LastActive = DateTime.Now;
            await dbContext.SaveAll();
        }
    }
}
