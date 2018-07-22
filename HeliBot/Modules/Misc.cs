using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.WebSocket;

namespace HeliBot.Modules
{
	public static class Misc
	{
		public static string HelibotLandImageUrl = "https://i.kym-cdn.com/entries/icons/original/000/017/301/top_10_most_painful_faceplants_1_.jpg";
		public static string HelibotDropImageUrl = "https://swanbakedotcom.files.wordpress.com/2017/04/scarface-omar-thrown-from-helicopter1.jpg?w=700";
		public static string HelibotImageURL = "http://www.emoji.co.uk/files/twitter-emojis/travel-places-twitter/10901-helicopter.png";
		
		public static Embed GetBannedPhraseEmbed()
		{
			EmbedBuilder PhrasesEmbed = new EmbedBuilder();
			StringBuilder stringBuilder = new StringBuilder();
			PhrasesEmbed.WithTitle($"Current list of banned phrases");
			foreach (string phrase in Handlers.badWords)
			{
				stringBuilder.Append($"{phrase} ");
				stringBuilder.Append("\n");
			}
			PhrasesEmbed.WithDescription(stringBuilder.ToString());
			PhrasesEmbed.WithColor(218, 165, 32);
			PhrasesEmbed.WithThumbnailUrl(HelibotImageURL);
			PhrasesEmbed.WithFooter("Banned phrases", HelibotImageURL);
			PhrasesEmbed.WithCurrentTimestamp();
			return PhrasesEmbed;
		}

		public static void AddNewWord(string word)
		{
			if (word == null) return;
			Handlers.badWords.Append(word.ToLower());
			File.AppendAllText(@"BannedWords.txt", word.ToLower());
		}

		public static Embed CreateHeliDropEmbed(string username, string PilotName, bool IsAutomaticDrop, string msg = "", string reason = "Used Banned Phrase", ulong banTime = 0)
		{
			string TimeType;
			if (banTime < 60000)
			{
				TimeType = "Seconds";
			}
			else
			{
				TimeType = "Minutes";
			}
			TimeSpan result = TimeSpan.FromMilliseconds(banTime);
			string BanTimeFormatted = result.ToString("mm':'ss");
			EmbedBuilder DropEmbed = new EmbedBuilder();
			DropEmbed.WithTitle($"{username} has been dropped from the helicopter!");
			if (reason != null) DropEmbed.AddField("Drop Reason:", reason);
			DropEmbed.AddField(CreateEmbedField(false, "Dropped by:", PilotName));
			DropEmbed.AddField(CreateEmbedField(false, "Drop Length", $"{BanTimeFormatted} {TimeType}"));
			DropEmbed.WithThumbnailUrl(HelibotDropImageUrl);
			DropEmbed.WithCurrentTimestamp();
			DropEmbed.WithFooter("Helicopter Drop", HelibotImageURL);
			DropEmbed.WithColor(255, 0, 0);
			return DropEmbed.Build();
		}

		public static Embed CreateHeliLandEmbed(string username,string PilotName)
		{
			EmbedBuilder DropEmbed = new EmbedBuilder();
			DropEmbed.WithTitle($"@{username} has hit the ground hard!");
			DropEmbed.AddField(CreateEmbedField(false, "Landed by:", PilotName));
			DropEmbed.WithThumbnailUrl(HelibotLandImageUrl);
			DropEmbed.WithCurrentTimestamp();
			DropEmbed.WithFooter("Helicopter Drop", HelibotImageURL);
			DropEmbed.WithColor(135, 206, 235);
			return DropEmbed.Build();
		}

		public static Embed CreatePingEmbed(string GuildName)
		{
			EmbedBuilder PingEmbed = new EmbedBuilder();
			PingEmbed.AddField(CreateEmbedField(false, $"Ping to {GuildName}:", "Pinging"));
			PingEmbed.WithColor(135, 206, 235);
			PingEmbed.WithTitle("Checking ping to Discord API");
			PingEmbed.WithDescription("It's like a virtual game of tennis");
			PingEmbed.WithThumbnailUrl(HelibotImageURL);
			PingEmbed.WithFooter("Ping check", HelibotImageURL);
			PingEmbed.WithCurrentTimestamp();
			return PingEmbed.Build();
		}

		public static Embed CreatePingEmbed2(string GuildName, int ping)
		{
			EmbedBuilder PingEmbed = new EmbedBuilder();
			PingEmbed.AddField(CreateEmbedField(false, $"Ping to {GuildName}:", $"{ping}ms"));
			PingEmbed.WithColor(135, 206, 235);
			PingEmbed.WithTitle("Checking ping to Discord API");
			PingEmbed.WithDescription("It's like a virtual game of tennis");
			PingEmbed.WithThumbnailUrl(HelibotImageURL);
			PingEmbed.WithFooter("Ping check", HelibotImageURL);
			PingEmbed.WithCurrentTimestamp();
			return PingEmbed.Build();
		}

		private static EmbedFieldBuilder CreateEmbedField(bool inline, string name, object value)
		{
			EmbedFieldBuilder FieldBuilder = new EmbedFieldBuilder();
			FieldBuilder.WithIsInline(inline);
			FieldBuilder.WithName(name);
			FieldBuilder.WithValue(value);
			return FieldBuilder;
		}

		public static Embed CreateIntroEmbed()
		{
			EmbedBuilder IntroEmbed = new EmbedBuilder();
			IntroEmbed.AddField(CreateEmbedField(false, "Function:", "Ban people who use certain banned words"));
			IntroEmbed.AddField(CreateEmbedField(false, "Devoped by:", "Stillkill"));
			IntroEmbed.AddField(CreateEmbedField(false, "List of Commands", "Type //Help"));
			IntroEmbed.AddField(CreateEmbedField(false, "Command Prefixes", "'//' or '@helibot'"));
			IntroEmbed.AddField(CreateEmbedField(false, "About Helibot:", "Helibot is written in C# using the Discord.net 1.0.2 API"));
			IntroEmbed.AddField(CreateEmbedField(false, "Source:", "https://github.com/RealStillkill/HeliBot"));
			IntroEmbed.WithTitle("Hello, My name is Helibot");
			IntroEmbed.WithDescription("Allow me to tell you a little about my self!");
			IntroEmbed.WithColor(0, 0, 255);
			IntroEmbed.WithThumbnailUrl(HelibotImageURL);
			IntroEmbed.WithFooter("I'll always be watching", HelibotImageURL);
			return IntroEmbed.Build();
		}

		public static Embed CreateHelpEmbed(SocketGuildUser user)
		{
			//max of 20 commands. If command ammount excedes 20, create and send a new embed.
			EmbedBuilder HelpEmbed = new EmbedBuilder();
			HelpEmbed.WithTitle($"A list of commands for {user.Username}:");
			HelpEmbed.AddField(CreateEmbedField(false, "//Help:", "Displays a list of commands such as this one."));
			HelpEmbed.AddField(CreateEmbedField(false, "//Drop", "Usage: //Drop <@user#0000>"));
			HelpEmbed.AddField(CreateEmbedField(false, "//Land", "Usage: //Land <@user#0000>"));
			HelpEmbed.AddField(CreateEmbedField(false, "//AddPhrase", "Usage: //AddPhrase <phrase>"));
			HelpEmbed.AddField(CreateEmbedField(false, "//BannedPhrases", "Usage: //BannedPhrases"));
			HelpEmbed.AddField(CreateEmbedField(false, "//Secret", "Usage: //Secret (Does nothing)"));
			HelpEmbed.AddField(CreateEmbedField(false, "//Ping", "Usage: //Ping"));
			HelpEmbed.WithColor(0, 255, 0);
			HelpEmbed.WithDescription("Helibot is in active development! More features to come soon™ \n\n");
			HelpEmbed.WithCurrentTimestamp();
			HelpEmbed.WithFooter("List of Helibot commands", HelibotImageURL);
			HelpEmbed.WithThumbnailUrl(HelibotImageURL);
			return HelpEmbed;
		}
	}
}
