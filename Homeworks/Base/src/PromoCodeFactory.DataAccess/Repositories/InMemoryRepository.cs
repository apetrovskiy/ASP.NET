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
        // TODO: to protected
        public IEnumerable<T> Data { get; set; }

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
            await Task.FromResult(() =>
            {
                // var item = Data.FirstOrDefault(x => x.Id == entity.Id);
                //
                // Data.ToList().ForEach(d => Console.WriteLine($"UpdateAsync 1: {d.Id}"));
                //
                var preliminaryCollecition = Data.ToList();
                var item = preliminaryCollecition.FirstOrDefault(x => x.Id == entity.Id);
                preliminaryCollecition.Remove(item);
                Data = preliminaryCollecition;
                Data = Data.Append(entity);
                // Data.ToList().Remove(item);
                // Data = Data.Append(entity);
                //
                // Data.ToList().ForEach(d => Console.WriteLine($"UpdateAsync 2: {d.Id}"));
                //
            });
            return;
        }
        public async Task DeleteAsync(T entity)
        {
            await Task.FromResult(() =>
            {
                // var item = Data.FirstOrDefault(x => x.Id == entity.Id);
                var preliminaryCollecition = Data.ToList();
                var item = preliminaryCollecition.FirstOrDefault(x => x.Id == entity.Id);
                preliminaryCollecition.Remove(item);
                Data = preliminaryCollecition;
            });
            return;
        }
    }
}