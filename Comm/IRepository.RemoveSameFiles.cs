using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Comm
{
	public abstract partial class IRepository
	{
		public bool RemoveSameFiles(ObservableCollection<Change> changes, Change change) {
			Change deleteChange = null;
			foreach (var item in changes)
			{
				if (item.Equals(change))
				{
					if (item.Status == Translation.Statuses.Deleted)
					{
						deleteChange = item;
						change.Status = Translation.Statuses.Moved;
						bool isPostDeploy = IsPostDiployment(change.FilePath);
						if (!isPostDeploy)
						{
							change.Content =  GetContent(change.FilePath, change.Status);
						}
						break;
					}
					else if (change.Status == Translation.Statuses.Deleted)
					{
						item.Status = Translation.Statuses.Moved;
						bool isPostDeploy = IsPostDiployment(item.FilePath);
						if (!isPostDeploy) {
							item.Content = GetContent(item.FilePath, item.Status);
						}
						return false;
					}
				}
			}
			if (deleteChange != null)
				changes.Remove(deleteChange);
			return true;
		}
	}
}