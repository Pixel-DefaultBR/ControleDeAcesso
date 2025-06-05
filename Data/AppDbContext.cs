using Microsoft.EntityFrameworkCore;

namespace ControleDeAcesso.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Model.AuthModel> AuthModels { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.AuthModel>().ToTable("AuthModels");
            base.OnModelCreating(modelBuilder);
        }
    }
}
