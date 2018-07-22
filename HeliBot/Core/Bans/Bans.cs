using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
namespace HeliBot.Core.Bans
{
	public class Bans
	{

		private static string bansFile = "Resources/accounts.json";
		public static List<Ban> BanList;

		static Bans()
		{
			if (DataStorage.SaveExists(bansFile))
			{
				BanList = DataStorage.LoadBans(bansFile).ToList();
			}
			else
			{
				BanList = new List<Ban>();
				SaveBans();
			}
		}

		private static Ban GetOrCreateBan(ulong id, ulong banTime, Timer banTimer)
		{
			var result = from a in BanList where a.UserID == id select a;
			var account = result.FirstOrDefault();
			if (account == null) account = CreateUserBan(id, banTime, banTimer);
			return account;
		}

		public static Ban CreateUserBan(ulong id, ulong banTime, Timer banTimer)
		{
			var newBan = new Ban()
			{
				UserID = id,
				BanTimeRemaining = banTime,
				BanID = DateTime.UtcNow.ToString(),
				BanTimer = banTimer
			};
			BanList.Add(newBan);
			SaveBans();
			return newBan;
		}

		public static Ban GetBan(Ban ban)
		{
			return GetOrCreateBan(ban.UserID, ban.BanTimeRemaining, ban.BanTimer);
		}

		public static void SaveBans()
		{
			DataStorage.SaveBans(BanList, bansFile);
		}
	}
}
