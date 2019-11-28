using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace U.IdentityService.Application.Services
{
    public interface IClaimsProvider
    {
         Task<IDictionary<string, string>> GetAsync(Guid userId);
    }
}