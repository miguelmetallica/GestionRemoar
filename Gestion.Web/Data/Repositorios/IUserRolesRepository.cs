using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IUserRolesRepository : IGenericRepository<RolesDto>
    {
        Task<int> spInsertar(RolesDto item);
        Task<int> spDelete(RolesDto item);
    }    
}
