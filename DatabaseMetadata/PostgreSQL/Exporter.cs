using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DatabaseMetadata.PostgreSQL
{
	public static class Exporter
	{
		public static void ExportTableMetadata(string[] args)
		{
			using (var dbContext = new DatabaseContext(args))
			{
				var tables = dbContext.Tables
					.Where(e => e.Schema == "public")
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
				File.WriteAllText(args[6], html);
			}
		}

		private static string GetColumnMetadata(DatabaseContext dbContext, string tableName)
		{
			const string caption = "Колони";
			string[] headers = { "№", "Наименование", "Тип на данните", "Mаксимална дължина", "Nullable", "Стойност по подразбиране" };
			string[] properties = { "Ordinal", "Name", "Type", "MaxLength", "Nullable", "Default" };

			var items = dbContext.Columns
				.Where(e => e.TableSchema == "public" && e.TableName == tableName)
				.OrderBy(e => e.Ordinal)
				.ToArray();
			for (var i = 0; i < items.Length; i++)
			{
				if (items[i].Default?.Contains("nextval(") == true)
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
						$@"select
							row_number() over(order by kcu1.column_name) as ordinal_position,
							kcu1.constraint_name as constraint_name,
							kcu1.table_name as table_name,
							kcu1.column_name as column_name,
							kcu2.table_name as foreign_table_name,
							kcu2.column_name as foreign_column_name
						from information_schema.referential_constraints rc
							inner join information_schema.key_column_usage kcu1
							on kcu1.constraint_name = rc.constraint_name
							inner join information_schema.key_column_usage kcu2
							on kcu2.constraint_name = rc.unique_constraint_name
						where kcu1.constraint_schema = 'public' and kcu1.table_name = {tableName}")
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
						$@"select
							row_number() OVER (order by indexname) as ordinal_position,
							tablename,
							indexname,
							indexdef
						from pg_indexes
						where schemaname = 'public' and tablename = {tableName}")
				.ToArray();

			if (items.Length > 0)
			{
				for (var i = 0; i < items.Length; i++)
				{
					var usingInfo = items[i].Definition.Split(" USING ")[1];
					var usingFragments = usingInfo.Split(" (");
					items[i].Type = usingFragments[0];
					items[i].ColumnNames = usingFragments[1].Split(')')[0];
				}

				return HTMLFormatter.GenerateTable(caption, headers, properties, items);
			}
			return string.Empty;
		}
	}
}
