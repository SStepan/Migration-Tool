using System.Text.RegularExpressions;

namespace Comm
{
	public abstract partial class IRepository
	{
		protected string GetDeleteFrom(string insert, string tableName) {
			Regex reg = new Regex(@"(?<=\().+?(?=\))");
			var allMatches = reg.Matches(insert);
			string[] masInto = null;
			string[] masValues = null;
			foreach (var match in allMatches) {
				string line = match.ToString();
				if (masInto == null)
					masInto = line.Split(',');
				else
					masValues = line.Split(',');
			}
			string deleteFrom = "DELETE FROM " + tableName + " WHERE ";
			for (int i = 0; i < masInto.Length; i++) {
				if (i != masInto.Length - 1)
					deleteFrom += masInto[i] + " = " + masValues[i] + " AND ";
				else {
					deleteFrom += masInto[i] + " = " + masValues[i];
				}
			}
			return deleteFrom;
		}
	}
}