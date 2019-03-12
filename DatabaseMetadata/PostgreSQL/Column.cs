using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseMetadata.PostgreSQL
{
	[Table("columns", Schema = "information_schema")]
	public class Column
	{
		[Column("table_catalog")]
		public string TableCatalog { get; set; }

		[Column("table_schema")]
		public string TableSchema { get; set; }

		[Column("table_name")]
		public string TableName { get; set; }

		[Column("ordinal_position")]
		public int Ordinal { get; set; }

		[Column("column_name")]
		public string Name { get; set; }

		[Column("data_type")]
		public string Type { get; set; }

		[Column("is_nullable")]
		public string Nullable { get; set; }

		[Column("column_default")]
		public string Default { get; set; }

		[Column("character_maximum_length")]
		public int? MaxLength { get; set; }
	}
}
