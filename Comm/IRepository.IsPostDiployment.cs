using System.IO;

namespace Comm
{
	public abstract partial class IRepository
	{
		 public bool IsPostDiployment(string path) {
			bool isPostDiployment = false;
			using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default)) {
				string line = sr.ReadLine();
				string[] mas = line.Split();
				if (mas[0].ToUpper() == "CREATE")
					return false;
				else
				{
					return true;
				}
			}
		}
	}
}