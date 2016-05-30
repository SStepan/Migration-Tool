using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Comm
{
	public static partial class FileManager
	{
		public static string GetMigrationName(string pathMigr) {
			string[] files = Directory.GetFiles(pathMigr);

			for (int i = 0; i < files.Length; i++) {
				files[i] = Path.GetFileName(files[i]);
			}
			string exten = ".sql";
			Regex reg = new Regex(@"(\.sql$)");
			string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
			string name = String.Empty;
			if (files.Length > 0 && reg.IsMatch(files[files.Length - 1]))
			{
				int indexExt = files[files.Length - 1].IndexOf(exten);
				name = files[files.Length - 1].Remove(indexExt);
				string[] mas = name.Split();

				if (mas[0] == todayDate)
				{
					int number = Int32.Parse(mas[1]) + 1;
					name = mas[0] + " " + number + ".sql";
				}
				else
				{
					name = todayDate + " " + "1.sql";
				}
			}
			else
			{
				name = todayDate + " " + "1.sql";
			}
			return name;
		}
	}
}