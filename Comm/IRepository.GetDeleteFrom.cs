using System.Text.RegularExpressions;

namespace Comm
{
	public abstract partial class IRepository
	{
		protected string GetDeleteFrom(string insert, string tableName) {
			//Regex reg = new Regex(@"(?<=\().+?(?=\))");
			//Regex reg = new Regex(@"(\((([^\(\)])*|(\(\))*)+\))");
			insert = @"INSERT INTO [plt].[EntityInfo] ([Id], [Guid], [CreatedDateUtc], [EntityTypeId], [CustomerId]) VALUES (851, NEWID(), GETUTCDATE(), 428, 1) --Page > CRM > Clients List(852, NEWID(), GETUTCDATE(), 429, 1) --Page > CRM > Clients Details
(853, NEWID(), GETUTCDATE(), 430, 1) --Page > CRM > Bids List
(854, NEWID(), GETUTCDATE(), 431, 1) --Page > CRM > Bids Details
(855, NEWID(), GETUTCDATE(), 432, 1) --Page > CRM > Orders List
(856, NEWID(), GETUTCDATE(), 433, 1) --Page > CRM > Orders Details
(857, NEWID(), GETUTCDATE(), 434, 1) --Page > CRM > Contact Persons List
(858, NEWID(), GETUTCDATE(), 435, 1) --Page > CRM > Contact Persons Details
(859, NEWID(), GETUTCDATE(), 436, 1) --Page > CRM > Rates List
(860, NEWID(), GETUTCDATE(), 437, 1) --Page > CRM > Rates Details
(866, NEWID(), GETUTCDATE(), 447, 1) --Page > CRM > Spider List
(870, NEWID(), GETUTCDATE(), 10, 1) --CRM Role
(871, NEWID(), GETUTCDATE(), 10, 1) --CRM Admin Role
(872, NEWID(), GETUTCDATE(), 10, 1) --Sale Role
(873, NEWID(), GETUTCDATE(), 10, 1) --Customer Admin Role
(874, NEWID(), GETUTCDATE(), 10, 1) --Subscription Role
(875, NEWID(), GETUTCDATE(), 11, 1) --CRM Subscription
(876, NEWID(), GETUTCDATE(), 12, 2) --Customer Subscription
";
			//Regex reg = new Regex(@"(INSERT)([^\(\)]*)(?<ids>\((([^\(\)])*|(\(\))*)+\))\s*(VALUES)\s*((?<vals>(\((([^\(\)])*|(\(\))*)+\))).*)+");
			Regex reg = new Regex(@"((\w+)(?:\W*))*");
			var allMatches = reg.Matches(insert);
			var match1 = reg.Match(insert);
			string[] masInto = null;
			string[] masValues = null;
			foreach (var match in allMatches) {
				string line = match.ToString();
				if (masInto == null)
					masInto = line.Split(',');
				else
					masValues = line.Split(',');
			}
			string deleteFrom = "DELETE FROM " + tableName + " WHERE ";
			for (int i = 0; i < masInto.Length; i++) {
				if (i != masInto.Length - 1)
					deleteFrom += masInto[i] + " = " + masValues[i] + " AND ";
				else {
					deleteFrom += masInto[i] + " = " + masValues[i];
				}
			}
			return deleteFrom;
		}
	}
}