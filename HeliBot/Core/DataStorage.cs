using HeliBot.Core.UserAccounts;
using HeliBot.Core.Bans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace HeliBot.Core
{
	public static class DataStorage
	{
		public static void SaveUserAccounts(IEnumerable<UserAccount> accounts, string filePath)
		{
			string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
			File.WriteAllText(filePath, json);
		}

		public static IEnumerable<UserAccount> LoadUserAccounts(string filePath)
		{
			if (!File.Exists(filePath)) return null;
			string json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<List<UserAccount>>(json);
		}

		public static bool SaveExists(string filePath)
		{
			return File.Exists(filePath);
		}

		public static IEnumerable<Ban> LoadBans(string filePath)
		{
			if (!File.Exists(filePath)) return null;
			string json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<List<Ban>>(json);
		}

		public static void SaveBans(IEnumerable<Ban> bans, string filePath)
		{
			string json = JsonConvert.SerializeObject(bans, Formatting.Indented);
			File.WriteAllText(filePath, json);
		}
	}
}