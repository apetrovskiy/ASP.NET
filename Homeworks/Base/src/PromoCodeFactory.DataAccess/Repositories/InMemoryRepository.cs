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
            //
            Data.ToList().ForEach(d => Console.WriteLine($"GetAllAsync: {d.Id}"));
            //
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            //
            Data.ToList().ForEach(d => Console.WriteLine($"GetByIdAsync: {d.Id}"));
            //
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }
        public Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            //
            Data.ToList().ForEach(d => Console.WriteLine($"GetRangeByIdsAsync: {d.Id}"));
            //
            return Task.FromResult(Data.Where(x => ids.Contains(x.Id)).AsEnumerable());
        }
        public async Task AddAsync(T entity)
        {
            //
            Data.ToList().ForEach(d => Console.WriteLine($"AddAsync 1: {d.Id}"));
            //
            Task.FromResult(Data.Append(entity));
            Data.Append(entity);
            //
            Data.ToList().ForEach(d => Console.WriteLine($"AddAsync 2: {d.Id}"));
            //
            return;
        }
        public async Task UpdateAsync(T entity)
        {
            var item = Data.FirstOrDefault(x => x.Id == entity.Id);
            //
            Data.ToList().ForEach(d => Console.WriteLine($"UpdateAsync 1: {d.Id}"));
            //
            Task.FromResult(Data.ToList().Remove(item));
            Task.FromResult(Data.Append(entity));
            Data.ToList().Remove(item);
            Data.Append(entity);
            //
            Data.ToList().ForEach(d => Console.WriteLine($"UpdateAsync 2: {d.Id}"));
            //
            return;
        }
        public async Task DeleteAsync(T entity)
        {
            var item = Data.FirstOrDefault(x => x.Id == entity.Id);
            //
            Data.ToList().ForEach(d => Console.WriteLine($"DeleteAsync 1: {d.Id}"));
            //
            Task.FromResult(Data.ToList().Remove(item));
            Data.ToList().Remove(item);
            //
            Data.ToList().ForEach(d => Console.WriteLine($"DeleteAsync 2: {d.Id}"));
            //
            return;
        }
    }
}