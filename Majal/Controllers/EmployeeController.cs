using Majal.Core.Abstractions.Const;
using Majal.Core.Specification.EntitySpecification;
using Microsoft.AspNetCore.Authorization;

namespace Majal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService) : ControllerBase
    {
        //private readonly IEmployeeService _employeeService = employeeService;
        //[HttpGet("{id}")]
        //[Authorize(Roles =DefaultRoles.Admin)]
        //public  async Task<IActionResult> get([FromRoute]int id)
        //{
        //    var employee = await _employeeService.GetEmployeeByIsAsync(new EmployeeSpecification(id));
            
        //    return employee.IsSuccess ? Ok(employee.Value.Adapt<EmployeeResponse>())
        //                              : employee.ToProblem();
        //}
    }
}