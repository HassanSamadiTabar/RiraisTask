using Microsoft.EntityFrameworkCore;
using RiraisTask.Domain;

namespace RiraisTask.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Person> People => Set<Person>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Person>();

            entity.ToTable("People");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.FirstName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(x => x.LastName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(x => x.NationalCode)
                  .IsRequired()
                  .HasMaxLength(10);

            entity.HasIndex(x => x.NationalCode).IsUnique();

            entity.Property(x => x.BirthDate)
                  .HasColumnType("date");

            entity.Property(x => x.RowVersion)
                  .IsRowVersion();
        }
    }
}
