using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }
        public Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            return Task.FromResult(Data.Where(x => ids.Contains(x.Id)).AsEnumerable());
        }
        public async Task AddAsync(T entity)
        {
            Data.Append(entity);
            return;
        }
        public async Task UpdateAsync(T entity)
        {
            var item = Data.FirstOrDefault(x => x.Id == entity.Id);
            Data.ToList().Remove(item);
            Data.Append(entity);
            return;
        }
        public async Task DeleteAsync(T entity)
        {
            var item = Data.FirstOrDefault(x => x.Id == entity.Id);
            Data.ToList().Remove(item);
            return;
        }
    }
}