using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Comm.Annotations;

namespace Comm
{
	public class Change : INotifyPropertyChanged {
		public string FilePath { get; set; }

		public string Status { get; set; }
		public string Content { get; set; }
		public bool Include { get; set; }
		public Inserts inserts;

		public Inserts Inserts {
			get { return inserts; }
			set {
				inserts = value;
				OnPropertyChanged("Inserts");
			}
		}

		public Change() {
			Include = true;
		}

		public override bool Equals(object obj) {
			Change change= obj as Change;
			if (change == null)
				return false;
			else
			{
				string name1 = new FileInfo(this.FilePath).Name;
				string name2 = new FileInfo(change.FilePath).Name;

				if (name1 == name2)
					return true;
				else
					return false;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}