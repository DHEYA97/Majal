using Majal.Core.Abstractions.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Majal.Api.Controllers
{
    [Authorize(Roles = DefaultRoles.Admin)]
    public class RolesController(IRoleService roleService) : BaseApiController
    {
        private readonly IRoleService _roleService = roleService;
        [HttpPut("{roleId}")]
        public async Task<IActionResult> ToggleRoleById([FromRoute] string roleId, CancellationToken cancellationToken)
        {
            var result = await _roleService.ToggleRolesAsync(roleId, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllRoles([FromQuery] bool isIncludeDeleted, CancellationToken cancellationToken)
        {
            var result = await _roleService.GetAllRolesAsync(isIncludeDeleted, cancellationToken);
            return Ok(result);
        }
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById([FromRoute] string roleId, CancellationToken cancellationToken)
        {
            var result = await _roleService.GetRoleByIdAsync(roleId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}