using Majal.Core.Abstractions;
using Majal.Core.Abstractions.Const;
using Majal.Core.Contract.Client;
using Majal.Core.Specification.EntitySpecification;
using Microsoft.AspNetCore.Authorization;

namespace Majal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OurSystemController(IOurSystemService ourSystemService) : ControllerBase
    {
        private readonly IOurSystemService _ourSystemService = ourSystemService;
        [HttpGet("")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> getAll()
        {
            var ourSystemResult = await _ourSystemService.GetAllSystemAsync(new OurSystemSpecification());

            return ourSystemResult.IsSuccess ? Ok(ourSystemResult.Value) : ourSystemResult.ToProblem();
        }

        [HttpGet("{Id}")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> get(int Id)
        {
            var ourSystemResult = await _ourSystemService.GetSystemByIdAsync(new OurSystemSpecification(Id));

            return ourSystemResult.IsSuccess ? Ok(ourSystemResult.Value) : ourSystemResult.ToProblem();
        }
    }
}