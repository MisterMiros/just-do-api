using API.Model.Repositories;
using API.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Filters
{
    public class UserFilterAttribute : Attribute, IAsyncResourceFilter
    {

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var userId = int.Parse(context.HttpContext.User.Identity.Name);
            var userRepository = context.HttpContext.RequestServices.GetService<UserRepository>();

            var user = userRepository.Find(userId);
            if (user == null)
            {
                context.Result = new JsonResult(new ResponseData() { Message = "User does not exists" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }
            context.HttpContext.Items.Add(Constants.HTTP_CONTEXT_CURRENT_USER_ITEM, user);
            await next();
        }

        public class ResponseData
        {
            public string Message { get; set; }
        }

    }
}
