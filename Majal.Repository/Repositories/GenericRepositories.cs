using Majal.Core.Entities;
using Majal.Core.Interfaces.Repositories;
using Majal.Core.Specification;
using Majal.Repository.Persistence;
using Majal.Repository.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Repository.Repositories
{
    public class GenericRepositories<T>(ApplicationDbContext context) : IGenericRepositories<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context = context;
        
        public IQueryable<T> ApplaySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), specification);
        }
        public async Task<IEnumerable<T?>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }
        public async Task<IEnumerable<T?>> GetAllWithSpecificationAsync(ISpecification<T> specification)
        {
            return await ApplaySpecification(specification).ToListAsync();
        }
        public async Task<T?> GetByIdWithSpecificationAsync(ISpecification<T> specification)
        {
            return await ApplaySpecification(specification).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(ISpecification<T> specification)
        {
            return await ApplaySpecification(specification).CountAsync();
        }

        public async Task AddAsync(T Entity)
        {
            await _context.Set<T>().AddAsync(Entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
