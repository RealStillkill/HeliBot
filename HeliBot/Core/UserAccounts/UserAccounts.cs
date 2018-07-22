using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using HeliBot.Core;

namespace HeliBot.Core.UserAccounts
{
	public static class UserAccounts
	{
		private static List<UserAccount> accounts;

		private static string accountsFile = "Resources/accounts.json";

		static UserAccounts()
		{
			if(DataStorage.SaveExists(accountsFile))
			{
				accounts = DataStorage.LoadUserAccounts(accountsFile).ToList();
			}
			else
			{
				accounts = new List<UserAccount>();
				SaveAccounts();
			}
		}

		public static void SaveAccounts()
		{
			DataStorage.SaveUserAccounts(accounts, accountsFile);
		}

		public static ulong GetAccountBanCount(SocketUser user)
		{
			return GetAccount(user).BanCount;
		}

		public static void AddAccountBanCount(SocketUser user)
		{
			var account = GetAccount(user);
			account.BanCount += 1;
		}

		public static UserAccount GetAccount(SocketUser user)
		{
			return GetOrCreateAccount(user.Id);
		}

		public static ulong GetAccountID(SocketUser user)
		{
			return GetAccount(user).ID;
		}

		private static UserAccount GetOrCreateAccount(ulong id)
		{
			var result = from a in accounts where a.ID == id select a;
			var account = result.FirstOrDefault();
			if (account == null) account = CreateUserAccount(id);
			return account;
		}

		private static UserAccount CreateUserAccount(ulong id)
		{
			var newAccount = new UserAccount()
			{
				ID = id,
				BanCount = 0,
				BanTime = 0
			};
			accounts.Add(newAccount);
			SaveAccounts();
			return newAccount;
		}
	}
}
