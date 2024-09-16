using Majal.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Interfaces.Authentication
{
    public interface IJwtProvider
    {
        (string Token, int Expirition) GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);

    }
}
