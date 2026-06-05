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

        public DbSet<ApplicationLog> Logs => Set<ApplicationLog>();

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

            var logEntity = modelBuilder.Entity<ApplicationLog>();

            logEntity.ToTable("Logs");
            logEntity.HasKey(x => x.Id);
            logEntity.Property(x => x.Id).ValueGeneratedOnAdd();

            logEntity.Property(x => x.Message).HasColumnType("nvarchar(max)");
            logEntity.Property(x => x.MessageTemplate).HasColumnType("nvarchar(max)");
            logEntity.Property(x => x.Level).HasMaxLength(128).HasColumnType("nvarchar(128)");
            logEntity.Property(x => x.TimeStamp).HasColumnType("datetime2");
            logEntity.Property(x => x.Exception).HasColumnType("nvarchar(max)");
            logEntity.Property(x => x.Properties).HasColumnType("nvarchar(max)");
            logEntity.Property(x => x.LogEvent).HasColumnType("nvarchar(max)");
            logEntity.Property(x => x.CorrelationId).HasMaxLength(64).HasColumnType("nvarchar(64)");
            logEntity.Property(x => x.GrpcMethod).HasMaxLength(256).HasColumnType("nvarchar(256)");
            logEntity.Property(x => x.SourceContext).HasMaxLength(256).HasColumnType("nvarchar(256)");

            logEntity.HasIndex(x => x.TimeStamp);
            logEntity.HasIndex(x => x.Level);
            logEntity.HasIndex(x => x.CorrelationId);
            logEntity.HasIndex(x => x.GrpcMethod);
        }
    }
}
