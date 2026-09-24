using Microsoft.EntityFrameworkCore;
using ToDoProject.Entities;

namespace ToDoProject.Data
{
    public class ToDoContext : DbContext
    {
        public const string ConnectionString =
            "Host=localhost;Port=5433;Database=ToDo;Username=postgres;Password=postgres";

        public ToDoContext()
        {
        }

        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {
        }

        public DbSet<ToDo> ToDos => Set<ToDo>();
        public DbSet<User> Users => Set<User>();


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options
                    .UseNpgsql(ConnectionString)
                    .LogTo(System.Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.UserName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.HasIndex(u => u.UserName)
                    .IsUnique();
                entity.Property(u => u.PasswordHash)
                    .IsRequired();
            });
            modelBuilder.Entity<ToDo>(entity =>
            {
                entity.ToTable("ToDos");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(t => t.Description)
                    .HasMaxLength(1000);
                entity.Property(t => t.CreatedAt)
                    .HasColumnType("timestamptz");
                entity.Property(t => t.Likes)
                    .HasDefaultValue(0);
                entity.Property(t => t.Dislikes)
                    .HasDefaultValue(0);
                entity.Property(t => t.IsPublic)
                    .HasDefaultValue(false);
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
        }
    }
}