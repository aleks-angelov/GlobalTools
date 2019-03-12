using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseMetadata.PostgreSQL
{
	[Table("tables", Schema = "information_schema")]
	public class Table
	{
		[Column("table_catalog")]
		public string Catalog { get; set; }

		[Column("table_schema")]
		public string Schema { get; set; }

		[Column("table_name")]
		public string Name { get; set; }
	}
}
