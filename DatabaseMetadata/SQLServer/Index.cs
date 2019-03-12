using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseMetadata.SQLServer
{
	public class Index
	{
		[Column("ORDINAL_POSITION")]
		public long Ordinal { get; set; }

		[Column("TABLE_NAME")]
		public string TableName { get; set; }

		[Column("INDEX_NAME")]
		public string Name { get; set; }

		[Column("TYPE")]
		public string Type { get; set; }

		[Column("COLUMN_NAME")]
		public string ColumnName { get; set; }
	}
}
