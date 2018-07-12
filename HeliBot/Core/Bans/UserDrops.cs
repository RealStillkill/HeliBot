using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using Discord;
namespace HeliBot.Core.Bans
{
	public class UserDrops
	{
		private static List<UserDrop> DropList;

		private static UserDrop CreateUserDrop(SocketGuildUser user)
		{
			var NewDrop = new UserDrop()
			{
				GuildUser = user,
				DropFlag = true
			};
			DropList.Add(NewDrop);
			return NewDrop;
		}

		public static UserDrop GetOrCreateDrop(SocketGuildUser user)
		{
			var result = from a in DropList where a.GuildUser == user select a;
			var Drop = result.FirstOrDefault();
			if (Drop == null) Drop = CreateUserDrop(user);
			return Drop;
		}
	}
}
