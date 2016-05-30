using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Comm.Annotations;

namespace Comm
{
	
	public class Inserts : INotifyPropertyChanged
	{
		public Dictionary<string, IncludeContent> addedLines;

		public Dictionary<string, IncludeContent> AddedLines
		{
			get { return addedLines; }
			set
			{
				addedLines = value;
				OnPropertyChanged("AddedLines");
			}
		} 
		public Inserts() {
			addedLines = new Dictionary<string, IncludeContent>();
		}
		//System.Environment.NewLine;
		public string GetContent() {
			string result = "";
			foreach (var addedLine in AddedLines)
			{
				if (addedLine.Value.Include)
					result += addedLine.Value.GetLines();
			}
			return result;
		}
		//public ICollection<ChangePostDeploy> AddedLines;
		//public ICollection<ChangePostDeploy> DeletedLines;
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class IncludeContent : INotifyPropertyChanged {

		public ICollection<string> Lines { get; set; }
		private bool include;
		public bool Include {
			get { return include; }
			set {
				include = value;
				OnPropertyChanged("Include");
			}

		}

		public string GetLines() {
			string result = "";
			foreach (var line in Lines) {
				result += line + System.Environment.NewLine;
			}
			return result;
		}

		public IncludeContent() {
			Lines = new List<string>();
			Include = true;
		}

		public IncludeContent(ICollection<string> Lines, bool boo) {
			this.Lines = Lines;
			Include = boo;
		}

		public IncludeContent(string element) {
			Lines = new List<string>();
			Lines.Add(element);
			Include = true;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}