using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApiDTO.Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
