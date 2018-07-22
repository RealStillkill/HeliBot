using System;
using Discord;
using System.IO;
using System.Linq;
using HeliBot.Modules;
using Discord.Commands;
using HeliBot.Core.Bans;
using System.Reflection;
using Discord.WebSocket;
using System.Threading.Tasks;
using HeliBot.Core.UserAccounts;

namespace HeliBot
{
	class Handlers
	{
		DiscordSocketClient _client;
		CommandService _CMDservice;
		//public static string[] badWords = new string[] { "uwu", "owo", "patootie" };
		public static string[] badWords = File.ReadAllLines(@"BannedWords.txt");


		public static void AddNewWord(string word)
		{
			if (word == null) return;
			badWords.Append(word.ToLower());
			File.AppendAllText(@"BannedWords.txt", word.ToLower());
		}

		public async Task InitializeAsync(DiscordSocketClient client)
		{
			_client = client;
			_CMDservice = new CommandService();
			await _CMDservice.AddModulesAsync(Assembly.GetEntryAssembly());
			_client.MessageReceived += HandleCommandAsync;
			_client.MessageReceived += HandleMessageAsync;
			_client.MessageReceived += BannedWordsDrop;
		}

		private async Task HandleCommandAsync(SocketMessage s)
		{
			var msg = s as SocketUserMessage;
			if (msg == null) return;
			var context = new SocketCommandContext(_client, msg);
			int argPos = 0;
			//can use string prefix or can me @ mentioned
			if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
			{
				var result = await _CMDservice.ExecuteAsync(context, argPos);
				//throw error in debug console
				if(!result.IsSuccess && result.Error != CommandError.UnknownCommand)
				{
					Console.WriteLine(result.ErrorReason);
				}
			}
		}

		private async Task HandleMessageAsync(SocketMessage s)
		{
			var msg = s as SocketUserMessage;
			if (msg == null) return;
			var context = new SocketCommandContext(_client, msg);
		}

		private async Task BannedWordsDrop(SocketMessage msg)
		{
			SocketUserMessage message = msg as SocketUserMessage;
			string msgsent = message.ToString().ToLower();
			SocketGuildUser author = message.Author as SocketGuildUser;
			SocketUser user = author as SocketUser;
			//string[] badWords = File.ReadAllLines(@"BannedWords.txt");
			if (badWords.Any(msgsent.Contains))
			{
				Console.WriteLine("Banned Phrase Detected!");
				Console.WriteLine($"User: {msg.Author.Username}");
				Console.WriteLine($"Phrase: {msg.Content}");
				
				//ban time constant represents 2 min, not 2 milliseconds. Make sure that the timer creation compensates
				UserAccounts.AddAccountBanCount(user);
				ulong banTimeC = 300000 + UserAccounts.GetAccountBanCount(user) * 60000;
				var role = author.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Being Dropped Out of a Helicopter");
				Console.WriteLine("Dropping user...");
				await author.AddRoleAsync(role);
				await msg.Channel.SendMessageAsync("", false, Misc.CreateHeliDropEmbed(user.Username, "Helibot Automatic Detection Service", true, msg.ToString(), "Banned Word Detected", banTimeC));
				Console.WriteLine("User Dropped!");
				Console.WriteLine("Initializing AutoLanding Service");
				DropTimer Countdown = new DropTimer();
				Countdown.SetComponents(author, message, banTimeC);
				Countdown.InitTimer();
				Countdown.TimerElapsed += CountDownElapsed;
				//Countdown.ClockElapsed += CountDownClockElapsed;
				Console.WriteLine("AutoLanding Service initialized.");
			}
		}

		private async void CountDownElapsed(object source, TimerEventArgs e)
		{
			SocketUserMessage message = e.GetEventInfoMessage();
			SocketGuildUser User = e.GetEventInfoUser();
			var role = User.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Being Dropped Out of a Helicopter");
			await User.RemoveRoleAsync(role);
			Embed UndropEmbed = Misc.CreateHeliLandEmbed(User.Username, "Helibot Automatic Detection Service");
			var msg = await message.Channel.SendMessageAsync("", false, UndropEmbed);
		}
	}
}