namespace DatabaseMetadata
{
	public static class HTMLFormatter
	{
		public static string GenerateTable(string caption, string[] headers, string[] properties, object[] items)
		{
			var htmlTable = "<table border=\"1\" style=\"border-collapse:collapse;\">\n";

			htmlTable += $"<h4>{caption}</h4>\n";

			htmlTable += "<thead>\n";
			htmlTable += "<tr>\n";
			for (var i = 0; i < headers.Length; i++)
			{
				htmlTable += $"<th>{headers[i]}</th>\n";
			}
			htmlTable += "</tr>\n";
			htmlTable += "</thead>\n";

			htmlTable += "<tbody>\n";
			for (var i = 0; i < items.Length; i++)
			{
				htmlTable += "<tr>\n";
				for (var j = 0; j < properties.Length; j++)
				{
					var objectType = items[i].GetType();
					var propertyValue = objectType.GetProperty(properties[j]).GetValue(items[i]);
					htmlTable += $"<td>{propertyValue}</td>\n";
				}
				htmlTable += "</tr>\n";
			}
			htmlTable += "</tbody>\n";

			htmlTable += "</table>\n";

			return htmlTable;
		}
	}
}
