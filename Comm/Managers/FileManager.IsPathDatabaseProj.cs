using System.IO;
using System.Text.RegularExpressions;

namespace Comm
{
	public static partial class FileManager {
		public static bool IsDatabaseProjectFolder(string path) {
			string[] files = Directory.GetFiles(path);
			Regex regexSqlProj = new Regex(@"\.sql$");

			for (var i = 0; i < files.Length; i++)
			{
				if (regexSqlProj.IsMatch(files[i]))
					return true;
			}

			return false;
		}
	}
}