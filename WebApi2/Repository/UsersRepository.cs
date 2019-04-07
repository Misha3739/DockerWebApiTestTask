using System.Linq;
using System.Threading.Tasks;
using WebApi2.Data;

namespace WebApi2.Repository
{
    public class UsersRepository : IRepository<User>
    {
        private readonly DataContext _context;

        public UsersRepository(DataContext context)
        {
            _context = context;
        }
        
        public async Task<User> GetAsync(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddOrUpdateAsync(User entity)
        {
            User existedUser = _context.Users.FirstOrDefault(user => user.Id == entity.Id || user.Name == entity.Name);
            if (existedUser == null)
            {
                await _context.Users.AddAsync(entity);
            }
            else
            {
                existedUser.Name = entity.Name;
                existedUser.Email = entity.Email;
            }

            await _context.SaveChangesAsync();
        }
    }
}