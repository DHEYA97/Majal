using Majal.Core.Abstractions.Const;
using Majal.Core.Contract.Auth.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Majal.Api.Controllers
{
    [Authorize(Roles = DefaultRoles.Admin)]
    public class UsersController(IUserService userService) : BaseApiController
    {
        private readonly IUserService _userService = userService;
        [HttpGet("")]
        public async Task<IActionResult> GetAllUser(CancellationToken cancellationToken)
        {
            return Ok(await _userService.GetAllUserAsync(cancellationToken));
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserDetails([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var result = await _userService.GetUserDetailsAsync(userId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost("")]
        public async Task<IActionResult> AddUser(AddUserRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _userService.AddUserAsync(request, cancellationToken);
            return result.IsSuccess ? CreatedAtAction(nameof(GetUserDetails), new { userId = result.Value.Id }, result.Value) : result.ToProblem();
        }
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string userId, UpdateUserRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _userService.UpdateUserAsync(userId, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        [HttpPut("{userId}/toggle-status")]
        public async Task<IActionResult> ToggleStatus([FromRoute] string userId, CancellationToken cancellationToken = default)
        {
            var result = await _userService.ToggleStatusAsync(userId, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        [HttpPut("{userId}/unlock")]
        public async Task<IActionResult> UnlockUser([FromRoute] string userId, CancellationToken cancellationToken = default)
        {
            var result = await _userService.UnLockAsync(userId, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
