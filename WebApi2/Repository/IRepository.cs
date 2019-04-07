using System.Threading.Tasks;

namespace WebApi2.Repository
{
    public interface IRepository<T> where  T : class, new()
    {
        Task<T> GetAsync(long id);

        Task AddOrUpdateAsync(T entity);
    }
}