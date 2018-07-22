using System.Timers;
namespace HeliBot.Core.Bans
{
	public class Ban : Timer
	{
		public ulong UserID { get; set; }

		public string BanID { get; set; }

		public ulong BanTimeRemaining { get; set; }

		public Timer BanTimer { get; set; }
	}
}
