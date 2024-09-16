using Majal.Core.Abstractions;
using Majal.Core.Contract.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Interfaces.Service
{
    public interface IRoleService
    {
        Task<IEnumerable<RolesResponse>> GetAllRolesAsync(bool? isIncludeDeleted = false, CancellationToken cancellationToken = default);
        Task<Result<RolesResponse>> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default);
        Task<Result> ToggleRolesAsync(string roleId, CancellationToken cancellationToken = default);
    }
}
