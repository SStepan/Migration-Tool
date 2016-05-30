using System;

namespace Comm
{
	public partial class GitRepository {
		public override string DeleteOrDrop(string line) {
			string[] mas = line.Split();

			if (DROP.Contains(mas[1]))
			{
				return "DROP " + mas[1] + " " + mas[2];
			}
			else
			{
				return "DELETE " + mas[1] + " " + mas[2];
			}
		}
	}
}