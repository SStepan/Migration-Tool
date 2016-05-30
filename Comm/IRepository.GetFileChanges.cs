using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Comm
{
	public abstract partial class IRepository
	{
		protected Inserts GetFileChanges(string patch) {
			Inserts cont = new Inserts();
			Regex regPlus = new Regex("^\\+\\w+.*");
			Regex regMin = new Regex("^\\-\\w+.*");


			Regex comment = new Regex("--");
			Regex setIdent = new Regex("SET IDENTITY_INSERT");
			string[] splitPatch = patch.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			List<string> added = splitPatch.Where(t => regPlus.IsMatch(t)).ToList();
			foreach (string line in added) {
				string element = line.Remove(0, 1);
				if (element != String.Empty && !comment.IsMatch(element)) {
					string[] mas = element.Split(' ');
					string key = "INSERT INTO " + mas[2];
					if (cont.AddedLines.ContainsKey(key)) {
						cont.AddedLines[key].Lines.Add(element);
					} else {
						cont.AddedLines.Add(key, new IncludeContent(element));
					}
				}
			}
			List<string> deleted = splitPatch.Where(t => regMin.IsMatch(t)).ToList();

			foreach (string line in deleted) {
				string element = line.Remove(0, 1);
				if (element != String.Empty && !comment.IsMatch(element) && !setIdent.IsMatch(element)) {
					string[] mas = element.Split(' ');
					string key = "DELETE FROM " + mas[2];
					if (cont.AddedLines.ContainsKey(key)) {
						string generDelete = GetDeleteFrom(element, mas[2]);
						cont.AddedLines[key].Lines.Add(generDelete);
					} else {
						string generDelete = GetDeleteFrom(element, mas[2]);
						cont.AddedLines.Add(key, new IncludeContent(generDelete));
					}
				}
			}

			return cont;
		}
	}
}