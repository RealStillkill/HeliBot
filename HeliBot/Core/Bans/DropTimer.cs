using Discord.WebSocket;
using System;
using System.Linq;
using System.Timers;

namespace HeliBot.Core.Bans
{
	public delegate void TimerEventHandler(object source, TimerEventArgs e);

	public class DropTimer : Timer
	{
		public event TimerEventHandler TimerElapsed;
		//public event TimerEventHandler ClockElapsed;

		public void InitTimer()
		{
			Timer BanClock = new Timer();
			
			BanClock.Interval = BanTime;
			BanClock.Elapsed += OnTimerElapsed;
			BanClock.AutoReset = false;
			Timer ChatClock = new Timer();
			ChatClock.Elapsed += OnTimerElapsed;
			ChatClock.AutoReset = true;
			ChatClock.Interval = BanTime;
			BanClock.Enabled = true;
			ChatClock.Enabled = true;
		}

		public void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			TimerElapsed(this, new TimerEventArgs(Author, Msg));
		}

		public void SetComponents(SocketGuildUser author, SocketUserMessage msg, ulong banTime)
		{
			Author = author;
			Msg = msg;
			BanTime = banTime;
		}

		public static SocketGuildUser Author { get; set; }

		public static ulong BanTime { get; set; }
		public static SocketUserMessage Msg { get; set; }
	}


	public class TimerEventArgs
	{
		private SocketUserMessage EventInfoMessage;
		private SocketGuildUser EventInfoUser;

		public TimerEventArgs(SocketGuildUser User, SocketUserMessage message)
		{
			EventInfoUser = User;
			EventInfoMessage = message;
		}

		public SocketUserMessage GetEventInfoMessage()
		{
			return EventInfoMessage;
		}

		public SocketGuildUser GetEventInfoUser()
		{
			return EventInfoUser;
		}
	}
}
