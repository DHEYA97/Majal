using Majal.Core.Abstractions;
using Majal.Core.Abstractions.Const;
using Majal.Core.Contract.Client;
using Majal.Core.Specification.EntitySpecification;
using Microsoft.AspNetCore.Authorization;

namespace Majal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController(IClientService clientService,IImageService imageService) : ControllerBase
    {
        private readonly IClientService _clientService = clientService;
        private readonly IImageService _imageService = imageService;
        
        [HttpGet("{Id}")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> get(int Id)
        {
            var clientResult = await _clientService.GetClientAsync(new ClientSpecification(Id));
            return clientResult.IsSuccess ? Ok(clientResult.Value) : clientResult.ToProblem();
        }
        
        [HttpGet("")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> getAll()
        {
            var clientResult = await _clientService.GetAllClientAsync(new ClientSpecification());

            return clientResult.IsSuccess ? Ok(clientResult.Value) : clientResult.ToProblem();
        }
        
        [HttpPut("")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> Update([FromBody] ClientRequest request)
        {
            var isClientUpdated = await _clientService.UpdateAsync(request, new ClientSpecification(request.Id));
            return isClientUpdated.IsSuccess ? NoContent() : isClientUpdated.ToProblem();
        }
        
        [HttpPost("")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> Add([FromBody] ClientRequest request)
        {
            var isClientUpdated = await _clientService.AddAsync(request, new ClientSpecification(request.Id));
            return isClientUpdated.IsSuccess ? NoContent() : isClientUpdated.ToProblem();
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var isClientUpdated = await _clientService.DeleteAsync(new ClientSpecification(Id));
            return isClientUpdated.IsSuccess ? NoContent() : isClientUpdated.ToProblem();
        }
    }
}