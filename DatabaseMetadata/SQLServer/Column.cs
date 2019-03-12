using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseMetadata.SQLServer
{
	[Table("COLUMNS", Schema = "INFORMATION_SCHEMA")]
	public class Column
	{
		[Column("TABLE_CATALOG")]
		public string TableCatalog { get; set; }

		[Column("TABLE_SCHEMA")]
		public string TableSchema { get; set; }

		[Column("TABLE_NAME")]
		public string TableName { get; set; }

		[Column("ORDINAL_POSITION")]
		public int Ordinal { get; set; }

		[Column("COLUMN_NAME")]
		public string Name { get; set; }

		[Column("DATA_TYPE")]
		public string Type { get; set; }

		[Column("IS_NULLABLE")]
		public string Nullable { get; set; }

		[Column("COLUMN_DEFAULT")]
		public string Default { get; set; }

		[Column("CHARACTER_MAXIMUM_LENGTH")]
		public int? MaxLength { get; set; }
	}
}
