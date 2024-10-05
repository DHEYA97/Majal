using Majal.Core.Entities;
using Majal.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Interfaces.Repositories
{
    public interface IGenericRepositories<T> where T : class
    {
        Task<T?> GetByIdAsync(int Id);
        Task<IEnumerable<T?>> GetAllAsync();

        Task<T?> GetByIdWithSpecificationAsync(ISpecification<T> specification);
        Task<IEnumerable<T?>> GetAllWithSpecificationAsync(ISpecification<T> specification);
        Task<int> GetCountAsync(ISpecification<T> specification);
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
        public IQueryable<T> ApplaySpecification(ISpecification<T> specification);
    }
}
