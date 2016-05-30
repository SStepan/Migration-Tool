using LibGit2Sharp;

namespace Comm
{
	public partial class GitRepository : IRepository {
		Repository repository;

		public GitRepository(string path) {
			repository = new Repository(path);
		}


		public override string GetState(string state) {
			if (state == "Added" || state == "NewInWorkdir")
				return Translation.Statuses.Added;
			else if(state == "Modified")
			{
				return Translation.Statuses.Modified;
			}
			else
			{
				return Translation.Statuses.Deleted;
			}
		}

		public override bool IsValid(string path) {
			return Repository.IsValid(path); 
		}
	}
}