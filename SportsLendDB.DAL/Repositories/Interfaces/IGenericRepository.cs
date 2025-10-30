﻿using System.Linq.Expressions;
namespace SportsLendDB.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<int> CreateAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<bool> RemoveAsync(T entity);
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAllAsQueryable();
        Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>>? predicate = null,
            params Expression<Func<T, object>>[] includes);
        Task<int> SaveAsync();
    }
}
