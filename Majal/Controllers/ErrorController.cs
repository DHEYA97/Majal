using Majal.Core.Abstractions;

namespace Majal.Api.Controllers
{
    [Route("ErroresNotFound/{statusCode}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErroresNotFoundController : ControllerBase
    {
        public IActionResult Error(int statusCode)
        {
            return NotFound(Result.Failure(new Error("ResourceNotFound","End Point Not Found",404)).Error);
        }
    }
}
