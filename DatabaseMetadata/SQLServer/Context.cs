using Microsoft.EntityFrameworkCore;

namespace DatabaseMetadata.SQLServer
{
	public class DatabaseContext : DbContext
	{
		private readonly string ConnectionString;

		public DbQuery<Table> Tables { get; set; }
		public DbQuery<Column> Columns { get; set; }
		public DbQuery<Reference> References { get; set; }
		public DbQuery<Index> Indexes { get; set; }

		public DatabaseContext(string[] args)
		{
			ConnectionString = GetConnectionString(args);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(ConnectionString);
				optionsBuilder.EnableSensitiveDataLogging(true);
			}
		}

		private string GetConnectionString(string[] args)
		{
			return $"Server={args[1]};Database={args[2]};User Id={args[3]};Password={args[4]};";
		}
	}
}
