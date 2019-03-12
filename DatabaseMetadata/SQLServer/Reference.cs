using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseMetadata.SQLServer
{
	public class Reference
	{
		[Column("ORDINAL_POSITION")]
		public long Ordinal { get; set; }

		[Column("CONSTRAINT_NAME")]
		public string Name { get; set; }

		[Column("TABLE_NAME")]
		public string TableName { get; set; }

		[Column("COLUMN_NAME")]
		public string ColumnName { get; set; }

		[Column("FOREIGN_TABLE_NAME")]
		public string ForeignTableName { get; set; }

		[Column("FOREIGN_COLUMN_NAME")]
		public string ForeignColumnName { get; set; }
	}
}
