using Kanban.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Kanban.API.Context
{
    /// <summary>
    /// Representa a sessão com o banco de dados.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        /// <summary>
        /// Essa propriedade pode ser utilizada para consultar e ou persistir entidades tipo Card no banco de dados
        /// </summary>
        public DbSet<Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=kanban.db;Cache=Shared");




    }
}
