using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace HeliBot
{
	class Config
	{
		private const string _configFolder = "Resources";
		private const string _configFile = "config.json";

		public static BotConfig bot;
		static Config()
		{

			if (!Directory.Exists(_configFolder)) Directory.CreateDirectory(_configFolder);
			if (!File.Exists(_configFolder + "/" + _configFile))
			{
				bot = new BotConfig();
				string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
				File.WriteAllText(_configFolder + "/" + _configFile, json);
			}
			else
			{
				string json = File.ReadAllText(_configFolder + "/" + _configFile);
				bot = JsonConvert.DeserializeObject<BotConfig>(json);
				
			}
		}

		public struct BotConfig
		{
			public string token;
			public string cmdPrefix;
		}
	}
}
