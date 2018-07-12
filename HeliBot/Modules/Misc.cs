using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HeliBot.Core.UserAccounts;
using HeliBot.Core.Bans;
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
	public class Misc : ModuleBase<SocketCommandContext>
	{
		[Command("MyStats")]
		public async Task MyStats(IGuildUser user)
		{
			var account = UserAccounts.GetAccount((SocketUser)user);
			await Context.Channel.SendMessageAsync($"You have {account.BanCount} drops and {account.BanTime} Remaining");
		}

		[Command("DevDrop")]
		public async Task DevDrop(IGuildUser user)
		{
			if (Context.User.Id != 253313886466473997) return;
		
			var userAccount = UserAccounts.GetAccount((SocketUser)user);
			userAccount.BanCount++;
			UserAccounts.SaveAccounts();
			var role = user.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Being Dropped Out of a Helicopter");
			await user.AddRoleAsync(role);
			await Context.Channel.SendMessageAsync($"[Developer] Dropping {user.Username} from the helicopter");
		}
		[Command("Drop")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task Drop(IGuildUser user)
		{
			var userAccount = UserAccounts.GetAccount((SocketUser)user);
			userAccount.BanCount++;
			UserAccounts.SaveAccounts();
			var role = user.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Being Dropped Out of a Helicopter");
			await user.AddRoleAsync(role);
			await Context.Channel.SendMessageAsync($"Dropping {user.Username} from the helicopter");
		}
		[Command("Land")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task Land(IGuildUser user)
		{
			var userAccount = UserAccounts.GetAccount((SocketUser)user);
			var role = user.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Being Dropped Out of a Helicopter");
			await user.RemoveRoleAsync(role);
		}

		[Command("DevLand")]
		public async Task DevLand(IGuildUser user)
		{
			if (Context.User.Id != 253313886466473997) return;
			var userAccount = UserAccounts.GetAccount((SocketUser)user);
			var role = user.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Being Dropped Out of a Helicopter");
			await user.RemoveRoleAsync(role);
		}

		[Command("DevEcho")]
		public async Task Echo([Remainder]string message)
		{
			if (Context.User.Id != 253313886466473997) return;
			var embed = new EmbedBuilder();
			embed.WithTitle("Echo");
			embed.WithDescription(message);
			embed.WithColor(new Color(0, 0, 255));
			await Context.Channel.SendMessageAsync("", false, embed);
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
			await Context.Channel.SendMessageAsync("", false, embed);
		}

		[Command("Secret")]
		public async Task Secret([Remainder]string arg = "")
		{
			//if (!UserIsModerator((SocketGuildUser)Context.User)) return;
			var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
			await dmChannel.SendMessageAsync("`Chicken Tendies`");
			await Context.Channel.SendMessageAsync("Message sent! Check your DMs!");
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

		[Command("DevData")]
		public async Task GetData()
		{
			if (Context.User.Id != 253313886466473997) return;
			await Context.Channel.SendMessageAsync($"Data has {DataStorage.GetPairsCount()} pairs");
			DataStorage.AddPairToStorage("Count" + DataStorage.GetPairsCount(), "theCount" + DataStorage.GetPairsCount());
		}
	}
}
