﻿using Majal.Core.Entities;
using Majal.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepositories<TEntity> Repositories<TEntity>() where TEntity : class;
        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync(); // تأكيد المعاملة
        Task RollbackTransactionAsync();
    }
}
