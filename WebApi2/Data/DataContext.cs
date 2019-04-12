using Microsoft.EntityFrameworkCore;
using WebApi.Data.DomainObjects;

namespace WebApi2.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {   }
        public DbSet<User> Users { get; set; }
    }
}