using API.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model.Repositories
{
    public abstract class BaseRepository
    {
        protected ApplicationDatabaseContext _dbContext;
        protected BaseRepository(ApplicationDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
