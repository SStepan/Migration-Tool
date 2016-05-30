using System.Text.RegularExpressions;

namespace Comm {
	public static partial class FileManager {
		public static bool IsSubfolder(string folder, string subfolder) {
			Regex reg = new Regex(folder);
			return reg.IsMatch(subfolder);
		}
	}
}