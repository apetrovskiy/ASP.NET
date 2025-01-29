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
            if (null != Data)
            {
                var newData = data.ToList().Except(Data);
                if (null == newData || newData.Count() == 0) return;
                Data = Data.Concat(newData);
            }
            else Data = data;
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
            Data = await Task.FromResult(Data.Append(entity));
            return;
        }
        public async Task UpdateAsync(T entity)
        {
            var preliminaryCollecition = Data.ToList();
            var item = preliminaryCollecition.FirstOrDefault(x => x.Id == entity.Id);
            await Task.FromResult(preliminaryCollecition.Remove(item));
            Data = preliminaryCollecition;
            Data = await Task.FromResult(Data.Append(entity));
            return;
        }
        public async Task DeleteAsync(T entity)
        {
            var item = Data.FirstOrDefault(x => x.Id == entity.Id);
            var preliminaryCollection = Data.ToList();
            await Task.FromResult(preliminaryCollection.Remove(item));
            Data = preliminaryCollection;
            return;
        }
    }
}