using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseMetadata.PostgreSQL
{
	public class Index
	{
		[Column("ordinal_position")]
		public int Ordinal { get; set; }

		[Column("tablename")]
		public string TableName { get; set; }

		[Column("indexname")]
		public string Name { get; set; }

		[Column("indexdef")]
		public string Definition { get; set; }

		[NotMapped]
		public string Type { get; set; }

		[NotMapped]
		public string ColumnNames { get; set; }
	}
}
