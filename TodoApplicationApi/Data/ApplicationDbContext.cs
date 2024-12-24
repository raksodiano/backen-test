using Microsoft.EntityFrameworkCore;
using TodoApplicationApi.Models;
using Microsoft.EntityFrameworkCore.Design;
using TodoApplicationApi.Common.Enums;


namespace TodoApplicationApi.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<TodoItem> TodoItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TodoItem>()
					.Property(e => e.Status)
					.HasConversion<string>();
		}

		public override int SaveChanges()
		{
			var entries = ChangeTracker.Entries<TodoItem>();

			foreach (var entry in entries)
			{
				if (entry.State == EntityState.Added)
				{
					entry.Entity.Status = Status.Todo;
					entry.Entity.IsApproved = false;
					entry.Entity.CreatedAt = DateTime.UtcNow;
					entry.Entity.UpdatedAt = DateTime.UtcNow;
				}
				else if (entry.State == EntityState.Modified)
				{
					entry.Entity.UpdatedAt = DateTime.UtcNow;
				}
			}

			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker.Entries<TodoItem>();

			foreach (var entry in entries)
			{
				if (entry.State == EntityState.Added)
				{
					entry.Entity.Status = Status.Todo;
					entry.Entity.IsApproved = false;
					entry.Entity.CreatedAt = DateTime.UtcNow;
					entry.Entity.UpdatedAt = DateTime.UtcNow;
				}
				else if (entry.State == EntityState.Modified)
				{
					entry.Entity.UpdatedAt = DateTime.UtcNow;
				}
			}

			return await base.SaveChangesAsync(cancellationToken);
		}
	}

	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

			// Obtener la cadena de conexión desde las variables de entorno
			var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

			if (string.IsNullOrEmpty(connectionString))
			{
				throw new InvalidOperationException("La cadena de conexión no está definida en las variables de entorno.");
			}

			// Configurar el DbContext para usar PostgreSQL con la cadena de conexión
			optionsBuilder.UseNpgsql(connectionString);

			return new ApplicationDbContext(optionsBuilder.Options);
		}
	}
}
