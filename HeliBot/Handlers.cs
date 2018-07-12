using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using HeliBot.Core.UserAccounts;
using HeliBot.Core.Bans;

namespace HeliBot
{
	class CommandHandler
	{
		DiscordSocketClient _client;
		CommandService _CMDservice;
		readonly string[] bannedWords = { "Patootie", "owo", "UwU" };

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
				if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
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
			var message = msg as SocketUserMessage;
			var author = msg.Author as SocketGuildUser;
			var user = author as SocketUser;
			string[] badWords = File.ReadAllLines(@"BannedWords.txt");
			if (badWords.Any(msg.Content.Contains))
			{
				//ban time constant represents 2 min, not 2 milliseconds. Make sure that the timer creation compensates
				ulong banTime = UserAccounts.GetAccountBanCount(user) * 2;
				var role = author.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Being Dropped Out of a Helicopter");
				Bans.CreateUserBan(author.Id, banTime);
				Console.WriteLine("Banned word detected");
				await msg.Channel.SendMessageAsync("Dropped from the heli, faggot.");
			}
		}
	}
}
