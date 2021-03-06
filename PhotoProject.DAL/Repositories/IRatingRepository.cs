﻿using PhotoProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Repositories
{
    public interface IRatingRepository : IRepository<Rating>
    {
        Task<Rating> FindByUserIdAsync(string userId);
    }
}
