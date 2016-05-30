using System.IO;

namespace Comm {
	public abstract partial class IRepository {
		protected string GetContent(string path, string state, Inserts patch = null) {
			string content = "";
			if (state == Translation.Statuses.Added) {
				content = ReadFile(path);
			} else if (state == Translation.Statuses.Modified || state == Translation.Statuses.Moved) {
				if (patch == null) {
					content = ReadFile(path);
					content = content.Replace("CREATE", "ALTER");
				} else {
					content = patch.GetContent();
				}
			}
			else
			{
				if (patch != null)
					content = patch.GetContent();
			}
			return content;
		}

		protected string ReadFile(string path) {
			string result = "";
			using (StreamReader streamReader = new StreamReader(path)) {
				result = streamReader.ReadToEnd();
			}
			return result;
		}
	}
}