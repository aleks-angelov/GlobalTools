using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseMetadata.PostgreSQL
{
	public class Reference
	{
		[Column("ordinal_position")]
		public int Ordinal { get; set; }

		[Column("constraint_name")]
		public string Name { get; set; }

		[Column("table_name")]
		public string TableName { get; set; }

		[Column("column_name")]
		public string ColumnName { get; set; }

		[Column("foreign_table_name")]
		public string ForeignTableName { get; set; }

		[Column("foreign_column_name")]
		public string ForeignColumnName { get; set; }
	}
}
