using API.Model.Context;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace API.Filters
{
    public class TransactionFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetService<ApplicationDatabaseContext>();
            dbContext.Database.BeginTransaction();
            try
            {
                await next();
                dbContext.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                dbContext.Database.RollbackTransaction();
                throw ex;
            }
        }
    }
}
