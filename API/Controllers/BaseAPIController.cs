using API.Filters;
using API.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;

namespace API.Controllers.API
{
    [ApiController]
    [Authorize]
    [UserFilter]
    public abstract class BaseAPIController : ControllerBase
    {
        protected User CurrentUser {
            get { return HttpContext.Items[Constants.HTTP_CONTEXT_CURRENT_USER_ITEM] as User; }
        }
    }
}
