using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseMetadata.SQLServer
{
	[Table("TABLES", Schema = "INFORMATION_SCHEMA")]
	public class Table
	{
		[Column("TABLE_CATALOG")]
		public string Catalog { get; set; }

		[Column("TABLE_SCHEMA")]
		public string Schema { get; set; }

		[Column("TABLE_NAME")]
		public string Name { get; set; }
	}
}
