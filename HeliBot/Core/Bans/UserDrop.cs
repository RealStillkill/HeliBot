using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
namespace HeliBot.Core.Bans
{
	public class UserDrop
	{
		public SocketGuildUser GuildUser {get; set;}

		public bool DropFlag { get; set; }
	}
}
