using Majal.Core.Interfaces.Service;
using Majal.Core.Specification.EntitySpecification;
using Majal.Core.UnitOfWork;
using Majal.Repository.Persistence;
using Majal.Repository.Specification;

namespace Majal.Api.Controllers
{
    public class EmployeeController(IEmployeeService employeeService) : BaseApiController
    {
        private readonly IEmployeeService _employeeService = employeeService;
        
        [HttpGet("{id}")]
        public  async Task<IActionResult> get([FromRoute]int id)
        {
            //var emp = await SpecificationEvaluator<Employee>
            //         .GetQuery(_context.Employees.AsQueryable(),new EmployeeSpecification(id))
            //         .Include(e => e.Departments)
            //         .ThenInclude(ed => ed.Department)
            //         .ToListAsync();

            var employee = await _employeeService.GetEmployeeByIsAsync(new EmployeeSpecification(id));
            
            return employee.IsSuccess ? Ok(employee.Value.Adapt<EmployeeResponse>()) : employee.ToProblem();
        }
    }
}
