using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DatabaseLayer.Entity.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace WebLayer.Security.Requirement
{
    public class AppAuthoriaztionHandler : IAuthorizationHandler
    {
        private readonly ILogger<AppAuthoriaztionHandler> _logger;
        private readonly UserManager<AppUser> _userManager;
        public AppAuthoriaztionHandler(
            ILogger<AppAuthoriaztionHandler> logger,
            UserManager<AppUser> userManager
            )
        {
            _logger = logger;
            _userManager = userManager;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            _logger.LogInformation("context.Resource ~ " + context.Resource?.GetType().Name);

            // var httpContext = context.Resource as HttpContext;
            // _logger.LogInformation(httpContext.Request.Path.ToString());

            var requirements = context.PendingRequirements.ToList();
            foreach (var requirement in requirements)
            {
                if (requirement is GenzRequirements)
                {

                    if (IsGenZ(context.User, (GenzRequirements)requirement))
                    {
                        context.Succeed(requirement);
                    }
                    // code xu ly
                }
            }
            return Task.CompletedTask;
        }


        private bool IsGenZ(ClaimsPrincipal user, GenzRequirements requirement)
        {
            var appUserTask = _userManager.GetUserAsync(user);
            Task.WaitAll(appUserTask);
            var appUser = appUserTask.Result;
            if (appUser.Birthday == null)
            {
                _logger.LogInformation($"{appUser.UserName} does not have birthday");
                return false;
            }
            int year = appUser.Birthday.Value.Year;
            bool success = (year >= requirement.fromYear && year <= requirement.toYear);
            _logger.LogInformation($"{appUser.UserName}" + ((success) ? " satify with GenZ condition" : " does not satify with GenZ condition"));
            return success;
        }
    }
}