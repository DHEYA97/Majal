using Majal.Core.Abstractions;
using Majal.Core.Abstractions.Const;
using Majal.Core.Contract.Client;
using Majal.Core.Contract.OurSystem;
using Majal.Core.Specification.EntitySpecification;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Cms;

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
        public async Task<IActionResult> get([FromRoute]int Id)
        {
            var ourSystemResult = await _ourSystemService.GetSystemByIdAsync(new OurSystemSpecification(Id));

            return ourSystemResult.IsSuccess ? Ok(ourSystemResult.Value) : ourSystemResult.ToProblem();
        }

        [HttpPost("")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> Add([FromBody]OurSystemRequest request)
        {
            var ourSystemResult = await _ourSystemService.AddAsync(request,new OurSystemSpecification(request.Id));

            return ourSystemResult.IsSuccess ? NoContent() : ourSystemResult.ToProblem();
        }

        [HttpPut("")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> Update([FromBody]OurSystemRequest request)
        {
            var ourSystemResult = await _ourSystemService.UpdateAsync(request, new OurSystemSpecification(request.Id));

            return ourSystemResult.IsSuccess ? NoContent() : ourSystemResult.ToProblem();
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> Delete([FromRoute]int Id)
        {
            var ourSystemResult = await _ourSystemService.DeleteAsync(new OurSystemSpecification(Id));

            return ourSystemResult.IsSuccess ? NoContent() : ourSystemResult.ToProblem();
        }
    }
}