using Majal.Core.Entities;
using Majal.Core.Interfaces.Repositories;
using Majal.Core.UnitOfWork;
using Majal.Repository.Persistence;
using Majal.Repository.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private IDbContextTransaction _transaction;
        private readonly Hashtable _repositoriesHashSet;
        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _repositoriesHashSet = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
            return await _applicationDbContext.SaveChangesAsync();
        }
        public async ValueTask DisposeAsync()
        {
            await _applicationDbContext.DisposeAsync();
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
        }
        public IGenericRepositories<TEntity> Repositories<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;
            if (!_repositoriesHashSet.ContainsKey(type))
            {
                var repo = new GenericRepositories<TEntity>(_applicationDbContext);
                _repositoriesHashSet.Add(type, repo);
            }
            return _repositoriesHashSet[type] as IGenericRepositories<TEntity>;
        }
        public async Task BeginTransactionAsync()
        {
            _transaction = await _applicationDbContext.Database.BeginTransactionAsync(); // بدء المعاملة
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync(); // تأكيد المعاملة
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync(); // إلغاء المعاملة
        }
    }
}
