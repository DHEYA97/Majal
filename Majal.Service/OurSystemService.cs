using Azure;
using Majal.Core.Abstractions;
using Majal.Core.Abstractions.Const.Errors;
using Majal.Core.Contract;
using Majal.Core.Contract.Client;
using Majal.Core.Contract.OurSystem;
using Majal.Core.Entities;
using Majal.Core.Interfaces.Service;
using Majal.Core.Specification.EntitySpecification;
using Majal.Core.UnitOfWork;
using Mapster;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;

namespace Majal.Service
{
    public class OurSystemService(IUnitOfWork unitOfWork) : IOurSystemService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result<IEnumerable<OurSystemsResponse>>> GetAllSystemAsync(OurSystemSpecification sp)
        {
            var systmes = await _unitOfWork.Repositories<OurSystem>().GetAllWithSpecificationAsync(sp);
            var response = systmes.Adapt<IEnumerable<OurSystemsResponse>>();
            return Result.Success(response);
        }
        public async Task<Result<OurSystemResponse>> GetSystemByIdAsync(OurSystemSpecification sp)
        {
            var systme = await _unitOfWork.Repositories<OurSystem>().GetByIdWithSpecificationAsync(sp);
            if (systme is null) { 
                 return Result.Failure<OurSystemResponse>(OurSystemError.OurSystemNotFound);
            }
            var response = systme.Adapt<OurSystemResponse>();
            return Result.Success(response);
        }
    }
}
