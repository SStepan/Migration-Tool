using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Comm.Annotations;

namespace Comm
{
	public class ChangesList : INotifyPropertyChanged
	{
		public ObservableCollection<Change> changes;

		public ChangesList() {
			changes = new ObservableCollection<Change>();
			changes.CollectionChanged += ContentCollectionChanged;
		}

		public ObservableCollection<Change> Changes
		{
			get { return changes; }
			set { changes = value; }
		}

		public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			OnPropertyChanged("Changes");
		}


		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}