using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Comm {

	public abstract partial class IRepository {
		public abstract ObservableCollection<Change> GetChanges(string pathRepository, string shortDatabaseFolderName);
		public abstract string GetState(string state);
		public abstract string DeleteOrDrop(string line);
		public abstract bool IsValid(string path);

		public List<string> DROP = new List<string> {"TABLE", "PROCEDURE", "SCHEME"};
		public List<string> DELETE = new List<string> {"VIEW"};

	}
}
