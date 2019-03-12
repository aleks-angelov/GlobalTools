using System;

namespace DatabaseMetadata
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				switch (args[0])
				{
					case "postgresql":
						PostgreSQL.Exporter.ExportTableMetadata(args);
						break;

					case "sqlserver":
						SQLServer.Exporter.ExportTableMetadata(args);
						break;

					default:
						throw new IndexOutOfRangeException("Invalid DBMS name");
				}

				Console.WriteLine("Database metadata exported successfully");
			}
			catch (IndexOutOfRangeException ex)
			{
				Console.WriteLine("Invalid arguments. Parameters: [DBMSName] [Server] [Port]* [Database] [User Id] [Password] [OutputPath]");
				Console.WriteLine("Sample usages:");
				Console.WriteLine(@"abtools-dbmeta postgresql localhost 5432 Mefis2 postgres !QAZ2wsx#EDC D:\Downloads\PostgreSQLTableMetadata.html");
				Console.WriteLine(@"abtools-dbmeta sqlserver . Mefis2018 sa !QAZ2wsx#EDC D:\Downloads\SQLServerTableMetadata.html");
			}
		}
	}
}
