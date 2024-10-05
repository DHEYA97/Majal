using Majal.Core.Abstractions.Const;
using Majal.Core.Contract.Client;
using Majal.Core.Contract.PostCategory;
using Majal.Core.Entities;
using Majal.Core.Interfaces.Service;
using Majal.Core.Specification.EntitySpecification;
using Majal.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Majal.Api.Controllers
{
  
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.ContentWriter}")]
    public class PostCategoryController(IPostCategoryService postCategory) : BaseApiController
    {
        private readonly IPostCategoryService _postCategory = postCategory;

        [HttpGet("{Id}")]
        public async Task<IActionResult> get(int Id)
        {
            var postCategoryResult = await _postCategory.GetPostCategoryAsync(new PostCategorySpecification(Id));
            return postCategoryResult.IsSuccess ? Ok(postCategoryResult.Value) : postCategoryResult.ToProblem();
        }

        [HttpGet("")]
        public async Task<IActionResult> getAll()
        {
            var postCategoryResult = await _postCategory.GetAllPostCategoryAsync(new PostCategorySpecification());

            return postCategoryResult.IsSuccess ? Ok(postCategoryResult.Value) : postCategoryResult.ToProblem();
        }

        [HttpPut("")]
        public async Task<IActionResult> Update([FromBody] PostCategoryRequest request)
        {
            var postCategoryResult = await _postCategory.UpdateAsync(request, new PostCategorySpecification(request.Id));
            return postCategoryResult.IsSuccess ? NoContent() : postCategoryResult.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PostCategoryRequest request)
        {
            var postCategoryResult = await _postCategory.AddAsync(request, new PostCategorySpecification(request.Id));
            return postCategoryResult.IsSuccess ? NoContent() : postCategoryResult.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var postCategoryResult = await _postCategory.DeleteAsync(new PostCategorySpecification(Id));
            return postCategoryResult.IsSuccess ? NoContent() : postCategoryResult.ToProblem();
        }
    }
}
