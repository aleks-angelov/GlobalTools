using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DatabaseMetadata.SQLServer
{
	public static class Exporter
	{
		public static void ExportTableMetadata(string[] args)
		{
			using (var dbContext = new DatabaseContext(args))
			{
				var tables = dbContext.Tables
					.Where(e => e.Schema == "dbo")
					.OrderBy(e => e.Name)
					.ToArray();

				var html = string.Empty;
				for (var i = 0; i < tables.Length; i++)
				{
					var table = tables[i];
					if (i > 0)
					{ html += "<hr style=\"margin-top: 25px;\">\n"; }

					html += "<section>\n";
					html += $"<h3>Таблица {table.Catalog}.{table.Schema}.{table.Name}</h3>\n";
					html += GetColumnMetadata(dbContext, table.Name);
					html += "<br>";
					html += GetReferenceMetadata(dbContext, table.Name);
					html += "<br>";
					html += GetIndexMetadata(dbContext, table.Name);
					html += "</section>\n";
				}
				File.WriteAllText(args[5], html);
			}
		}

		private static string GetColumnMetadata(DatabaseContext dbContext, string tableName)
		{
			const string caption = "Колони";
			string[] headers = { "№", "Наименование", "Тип на данните", "Mаксимална дължина", "Nullable", "Стойност по подразбиране" };
			string[] properties = { "Ordinal", "Name", "Type", "MaxLength", "Nullable", "Default" };

			var items = dbContext.Columns
				.Where(e => e.TableSchema == "dbo" && e.TableName == tableName)
				.OrderBy(e => e.Ordinal)
				.ToArray();
			for (var i = 0; i < items.Length; i++)
			{
				if (items[i].Default?.Contains("newid(") == true)
				{ items[i].Default = null; }
			}

			return HTMLFormatter.GenerateTable(caption, headers, properties, items);
		}

		private static string GetReferenceMetadata(DatabaseContext dbContext, string tableName)
		{
			const string caption = "Референтни връзки";
			string[] headers = { "№", "Наименование", "Колона", "Реферирана таблица", "Реферирана колона" };
			string[] properties = { "Ordinal", "Name", "ColumnName", "ForeignTableName", "ForeignColumnName" };

			var items = dbContext.References
				.FromSql(
						$@"SELECT
							ROW_NUMBER() OVER (ORDER BY KCU1.COLUMN_NAME) AS 'ORDINAL_POSITION',
							KCU1.CONSTRAINT_NAME AS 'CONSTRAINT_NAME',
							KCU1.TABLE_NAME AS 'TABLE_NAME',
							KCU1.COLUMN_NAME AS 'COLUMN_NAME',
							KCU2.TABLE_NAME AS 'FOREIGN_TABLE_NAME',
							KCU2.COLUMN_NAME AS 'FOREIGN_COLUMN_NAME'
						FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC
							INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1
							ON KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
							INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2
							ON KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME
						WHERE KCU1.CONSTRAINT_SCHEMA = 'dbo' AND KCU1.TABLE_NAME = {tableName}")
				.ToArray();

			if (items.Length > 0)
			{
				return HTMLFormatter.GenerateTable(caption, headers, properties, items);
			}
			return string.Empty;
		}

		private static string GetIndexMetadata(DatabaseContext dbContext, string tableName)
		{
			const string caption = "Индекси";
			string[] headers = { "№", "Наименование", "Тип", "Колони" };
			string[] properties = { "Ordinal", "Name", "Type", "ColumnNames" };

			var items = dbContext.Indexes
				.FromSql(
						$@"SELECT
							ROW_NUMBER() OVER (ORDER BY I.name) AS 'ORDINAL_POSITION',
							T.name AS 'TABLE_NAME',
							I.name AS 'INDEX_NAME',
							I.type_desc AS 'TYPE',
							AC.name AS 'COLUMN_NAME'
							FROM sys.tables AS T
								INNER JOIN sys.indexes I
								ON T.object_id = I.object_id
								INNER JOIN sys.index_columns IC
								ON I.object_id = IC.object_id
								INNER JOIN sys.all_columns AC
								ON T.object_id = AC.object_id AND IC.column_id = AC.column_id
							WHERE T.name = {tableName} AND T.is_ms_shipped = 0 AND I.type_desc != 'HEAP'")
				.ToArray();

			if (items.Length > 0)
			{
				return HTMLFormatter.GenerateTable(caption, headers, properties, items);
			}
			return string.Empty;
		}
	}
}
