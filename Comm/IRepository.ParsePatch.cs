using System;

namespace Comm
{
	public abstract partial class IRepository
	{
		public Inserts GetInserts(string text) {
			string[] splitPatch = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			Inserts inserts = new Inserts();
			for (var i = 0; i < splitPatch.Length; i++)
			{
				string[] masLine = splitPatch[i].Split();
				string key = masLine[0] + " " + masLine[1] + " " + masLine[2];
				if (inserts.AddedLines.ContainsKey(key))
				{
					inserts.AddedLines[key].Lines.Add(splitPatch[i]);
				}
				else
				{
					inserts.AddedLines.Add(key, new IncludeContent(splitPatch[i]));
				}
			}
			return inserts;
		}
	}
}