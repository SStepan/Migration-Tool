using System;
using System.Collections.Generic;
using System.Linq;

namespace Comm
{
	public abstract  partial class IRepository
	{
		public ICollection<string> ParseInserts(string content) {
			string[] splitPatch = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			return splitPatch.ToList();
		} 
	}
}