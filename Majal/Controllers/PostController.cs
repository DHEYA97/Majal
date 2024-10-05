using Majal.Core.Abstractions;
using Majal.Core.Abstractions.Const;
using Majal.Core.Contract.Post;
using Majal.Core.Specification.EntitySpecification;
using Microsoft.AspNetCore.Authorization;

namespace Majal.Api.Controllers
{
    
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.ContentWriter}")]
    public class PostController(IPostService postService) : BaseApiController
    {
        private readonly IPostService _postService = postService;
        
        [HttpGet("{Id}")]
        public async Task<IActionResult> get(int Id)
        {
            var PostResult = await _postService.GetPostAsync(new PostSpecification(Id));
            return PostResult.IsSuccess ? Ok(PostResult.Value) : PostResult.ToProblem();
        }
        
        [HttpGet("")]
        public async Task<IActionResult> getAll()
        {
            var PostResult = await _postService.GetAllPostAsync(new PostSpecification());

            return PostResult.IsSuccess ? Ok(PostResult.Value) : PostResult.ToProblem();
        }
        
        [HttpPut("")]
        public async Task<IActionResult> Update([FromBody] PostRequest request)
        {
            var PostResult = await _postService.UpdateAsync(request, new PostSpecification(request.Id));
            return PostResult.IsSuccess ? NoContent() : PostResult.ToProblem();
        }
        
        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PostRequest request)
        {
            var PostResult = await _postService.AddAsync(request, new PostSpecification(request.Id));
            return PostResult.IsSuccess ? NoContent() : PostResult.ToProblem();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var PostResult = await _postService.DeleteAsync(new PostSpecification(Id));
            return PostResult.IsSuccess ? NoContent() : PostResult.ToProblem();
        }
    }
}