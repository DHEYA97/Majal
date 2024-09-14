using Majal.Core.Abstractions;
using Majal.Core.Abstractions.Const.Errors;
using Majal.Core.Contract;
using Majal.Core.Entities;
using Majal.Core.Interfaces.Service;
using Majal.Core.Specification.EntitySpecification;
using Majal.Core.UnitOfWork;

namespace Majal.Service
{
    public class EmployeeService(IUnitOfWork unitOfWork) : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result<Employee>> GetEmployeeByIsAsync(EmployeeSpecification spe)
        {
            var employee = await _unitOfWork.Repositories<Employee>().GetByIdWithSpecificationAsync(spe);
            if (employee is null)
                return Result.Failure<Employee>(EmployeeError.EmployeeNotFound);
            return Result.Success<Employee>(employee);
        }
    }
}
