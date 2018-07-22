using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HeliBot.Core.UserAccounts;
using System;
using System.Linq;
using System.Threading.Tasks;

// drop image url https://swanbakedotcom.files.wordpress.com/2017/04/scarface-omar-thrown-from-helicopter1.jpg?w=700
// Possible voting command tutorial: https://www.youtube.com/watch?v=atnvzBpCG8g&
#region Things we've learned
//Required invoking USER to have a specific guild permission
//[RequireUserPermission(GuildPermission.Administrator)]
//replace "RequireUserPermission" with "RequireBotPermission" to require the BOT to have a specific permisssion

//Fast loading
//string[] badWords2 = new string [] { "1", "2" };

#endregion

namespace HeliBot.Modules
{

	public class Commands : ModuleBase<SocketCommandContext>
	{

		private bool UserIsDeveloper(SocketGuildUser user)
		{
			if (user.Id == 253313886466473997) return true;
			return false;
		}
		//TODO: Add //WordAdd

		//TODO: Add //DropVote

		//check if user has role
		private bool UserIsModerator(SocketGuildUser user)
		{
			string targetRoleName = "Moderator";
			var result = from r in user.Guild.Roles where r.Name == targetRoleName select r.Id;
			ulong roleID = result.FirstOrDefault();
			if (roleID == 0) return false;
			var targetRole = user.Guild.GetRole(roleID);
			return user.Roles.Contains(targetRole);
		}

		[Command("AddPhrase")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task AddPhrase([Remainder]string message)
		{
			string word = message.ToString();
			Handlers.AddNewWord(word);
			await Context.Channel.SendMessageAsync($"{Context.User.Username} has added the phrase '{word} to the banned phrases list");
		}

		[Command("BannedPhrases")]
		public async Task BannedPhrases()
		{
			await Context.Channel.SendMessageAsync(string.Empty, false, Misc.GetBannedPhraseEmbed());
		}

		[Command("DevAddPhrase")]
		public async Task DevAddPhrase([Remainder]string message)
		{
			if (UserIsDeveloper((SocketGuildUser)Context.User) != true)
			{
				await Context.Channel.SendMessageAsync("This command is reserved for developer use only");
			}
			string word = message.ToString();
			Handlers.AddNewWord(word);
			await Context.Channel.SendMessageAsync($"{Context.User.Username} has added the phrase '{word} to the banned phrases list");
		}
		[Command("DevDrop")]
		public async Task DevDrop(IGuildUser user, [Remainder]string reason)
		{
			if (Context.User.Id != 253313886466473997) return;
			var userAccount = UserAccounts.GetAccount((SocketUser)user);
			userAccount.BanCount++;
			UserAccounts.SaveAccounts();
			var role = user.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Being Dropped Out of a Helicopter");
			await user.AddRoleAsync(role);
			await Context.Channel.SendMessageAsync(string.Empty, false, Misc.CreateHeliDropEmbed(user.Username, Context.User.Username, false, string.Empty, reason));
		}

		[Command("DevLand")]
		public async Task DevLand(IGuildUser user)
		{
			var userAccount = UserAccounts.GetAccount((SocketUser)user);
			var role = user.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Being Dropped Out of a Helicopter");
			await user.RemoveRoleAsync(role);
			await Context.Channel.SendMessageAsync(string.Empty, false, Misc.CreateHeliLandEmbed(user.Username, Context.User.Username));
		}

		[Command("DevIntroduction")]
		public async Task DevIntro()
		{
			await Context.Channel.SendMessageAsync("", false, Misc.CreateIntroEmbed());
		}

		[Command("Drop")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task Drop(IGuildUser user, [Remainder]string reason)
		{
			var userAccount = UserAccounts.GetAccount((SocketUser)user);
			userAccount.BanCount++;
			UserAccounts.SaveAccounts();
			var role = user.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Being Dropped Out of a Helicopter");
			await user.AddRoleAsync(role);
			await Context.Channel.SendMessageAsync(string.Empty, false, Misc.CreateHeliDropEmbed(user.Username, Context.User.Username, false, string.Empty, reason));
		}

		[Command("DevEcho")]
		public async Task Echo([Remainder]string message)
		{
			if (Context.User.Id != 253313886466473997) return;
			var embed = new EmbedBuilder();
			embed.WithTitle(("Echo"));
			embed.WithDescription(message);
			embed.WithColor(new Color(0, 0, 255));
			await Context.Channel.SendMessageAsync(string.Empty, false, embed);
		}

		[Command("DevData")]
		public async Task GetData()
		{
			if (Context.User.Id != 253313886466473997) return;
			await Context.Channel.SendMessageAsync($"Data has {DataStorage.GetPairsCount()} pairs");
			DataStorage.AddPairToStorage("Count" + DataStorage.GetPairsCount(), "theCount" + DataStorage.GetPairsCount());
		}
		[Command("Help")]
		public async Task Help()
		{
			await Context.Channel.SendMessageAsync(string.Empty, false, Misc.CreateHelpEmbed((SocketGuildUser)Context.User));
		}

		[Command("Land")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task Land(IGuildUser user)
		{
			var userAccount = UserAccounts.GetAccount((SocketUser)user);
			var role = user.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Being Dropped Out of a Helicopter");
			await user.RemoveRoleAsync(role);
			await Context.Channel.SendMessageAsync(string.Empty, false, Misc.CreateHeliLandEmbed(user.Username, Context.User.Username));

		}

		[Command("MyStats")]
		public async Task MyStats(IGuildUser user)
		{
			var account = UserAccounts.GetAccount((SocketUser)user);
			await Context.Channel.SendMessageAsync($"You have {account.BanCount} drops and {account.BanTime} Remaining");
		}

		[Command("DevPick")]
		public async Task Pick([Remainder]string message)
		{
			if (Context.User.Id != 253313886466473997) return;
			string[] options = message.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

			Random rand = new Random();
			string selection = options[rand.Next(0, options.Length)];

			var embed = new EmbedBuilder();
			embed.WithTitle("Choice for " + Context.User.Username);
			embed.WithDescription(selection);
			embed.WithColor(new Color(0, 255, 0));
			embed.WithThumbnailUrl(Context.User.GetAvatarUrl());
			await Context.Channel.SendMessageAsync(string.Empty, false, embed);
		}

		[Command("Ping")]
		public async Task Ping()
		{
			var before = System.Environment.TickCount;
			var message = await Context.Channel.SendMessageAsync(string.Empty, false, Misc.CreatePingEmbed(Context.Guild.Name.ToString()));
			var after = System.Environment.TickCount;
			int ping = after - before;
			await message.ModifyAsync(x =>
			{
				x.Embed = Misc.CreatePingEmbed2(Context.Guild.Name.ToString(), ping);
			});
		}

		[Command("Secret")]
		public async Task Secret([Remainder]string arg = "")
		{
			//if (!UserIsModerator((SocketGuildUser)Context.User)) return;
			var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
			await dmChannel.SendMessageAsync("`I told you that this does nothing`");
			await Context.Channel.SendMessageAsync("Message sent! Check your DMs!");
		}
	}
}
