using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();

        Task<List<T>> GetAllAsync();
        Task<T> GetByIDAsync(int id);

        Task CreateAsync(T entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);

        void Dispose();
    }
}
