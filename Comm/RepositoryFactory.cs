using LibGit2Sharp;

namespace Comm
{
	public static class RepositoryFactory
	{
		public static IRepository GetRepository(string path) {
			if(Repository.IsValid(path))
				return new GitRepository(path);
			else
			{
				return null;
			}
		}
	}
}