using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using LibGit2Sharp;

namespace Comm
{
	public partial class GitRepository
	{
		public override ObservableCollection<Change> GetChanges(string pathRepository, string shortDatabaseFolderName) {
			Regex regex = new Regex(shortDatabaseFolderName + @"(.*)(\.sql$)");
			var statuses = repository.RetrieveStatus().Where(t => regex.IsMatch(t.FilePath));

			ObservableCollection<Change> changes = new ObservableCollection<Change>();
			foreach (var status in statuses) {
				Change change = new Change();
				string state = GetState(status.State.ToString());
				change.Status = state;
				change.FilePath = pathRepository + "\\" + status.FilePath;
				if (state == Translation.Statuses.Added ) {
					change.Content = GetContent(change.FilePath, state);
				} else if (state == Translation.Statuses.Modified)
				{
					
					bool isPostDiployment = IsPostDiployment(change.FilePath);
					if (isPostDiployment) {
						List<string> fileList = new List<string>() { status.FilePath };
						var diff = repository.Diff.Compare<Patch>(fileList);
						string patch = "";
						foreach (var diffItem in diff) {
							patch = diffItem.Patch;}
						Inserts pathFile = GetFileChanges(patch);
						change.Content = GetContent(status.FilePath, state, pathFile);
						change.Inserts = pathFile;
					} else {
						change.Content = GetContent(change.FilePath, state);
					}
				}
				else if(state == Translation.Statuses.Deleted)
				{
					List<string> fileList = new List<string>() { status.FilePath };
					var diff = repository.Diff.Compare<Patch>(fileList);
					string patch = "";
					foreach (var diffItem in diff) {
						patch = diffItem.Patch;
					}
					string[] splitPatch = patch.Split(new string[] {"\r", "\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
					Regex reg = new Regex(@"^-\w+.*");
					string[] masMin = splitPatch.Select(t => t = t.Replace("\ufeff", "")).Where(t => reg.IsMatch(t)).ToArray();
					string element = masMin[0];
					element = element.Remove(0, 1);
					string[] mas = element.Split();
					bool isPostDiployment = mas[0] != "CREATE"; 
					if (isPostDiployment)
					{
						Inserts inserts = GetFileChanges(patch);
						change.Content = GetContent(change.FilePath, change.Status, inserts);
					}
					else
					{
						change.Content = DeleteOrDrop(element);
					}
				}
				if(changes.Count == 0)
					changes.Add(change);
				else
				{
					bool addOrNot = RemoveSameFiles(changes, change);
					if (addOrNot)
					{
						changes.Add(change);
					}
				} 
					
			}
			return changes;
		}
	}
}