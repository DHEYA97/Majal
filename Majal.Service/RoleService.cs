using Majal.Core.Abstractions;
using Majal.Core.Contract.Roles;
using Majal.Core.Entities.Identity;
using Majal.Core.Interfaces.Service;
using Majal.Repository.Persistence;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Majal.Core.Abstractions.Const.Errors;
namespace Majal.Service
{
    public class RoleService(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context) : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<RolesResponse>> GetAllRolesAsync(bool? isIncludeDeleted = false, CancellationToken cancellationToken = default)
        {
            var roles = await _roleManager.Roles
                                         .Where(x => (!x.IsDeleted || (isIncludeDeleted.HasValue && isIncludeDeleted.Value)))
                                         .ProjectToType<RolesResponse>()
                                         .ToListAsync(cancellationToken);
            return roles;
        }
        public async Task<Result<RolesResponse>> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default)
        {
            var role = await _roleManager.Roles
                                         .Where(x => x.Id == roleId)
                                         .SingleOrDefaultAsync(cancellationToken);
            if (role is null)
            {
                return Result.Failure<RolesResponse>(RoleErrors.RoleNotFound);
            }
            var permission = await _roleManager.GetClaimsAsync(role);
            var roleWithPermissions = new RolesResponse(
                role.Id,
                role.Name!,
                role.IsDeleted);
            return Result.Success(roleWithPermissions);
        }
        public async Task<Result> ToggleRolesAsync(string roleId, CancellationToken cancellationToken = default)
        {

            if (await _roleManager.FindByIdAsync(roleId) is not { } role)
                return Result.Failure(RoleErrors.RoleNotFound);

            role.IsDeleted = !role.IsDeleted;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
    }
}
