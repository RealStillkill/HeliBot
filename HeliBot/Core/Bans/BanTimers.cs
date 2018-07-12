using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliBot.Core.Bans
{
	class BanTimers
	{
		public static List<BanTimer> TimerList;
		private static string timersFile = "resources/timers.json";
		private static long banStartTime = DateTime.Now.Ticks;


		static BanTimers()
		{
			if (DataStorage.SaveExists(timersFile))
			{
				TimerList = DataStorage.LoadTimers(timersFile).ToList();
			}
			else
			{
				TimerList = new List<BanTimer>();
				SaveTimers();
			}
		}

		private static BanTimer GetOrCreateBanTimer(ulong id, long banStartTime)
		{
			var result = from a in TimerList where a.BanTimerID == id select a;
			var BanTimer = result.FirstOrDefault();
			if (BanTimer == null) BanTimer = CreateBanTimer(id, banStartTime);
			return BanTimer;
		}

		public static BanTimer CreateBanTimer(ulong id, long banStartTime)
		{
			var newBanTimer = new BanTimer()
			{
				BanTimerID = id,
				BanStartTime = banStartTime
			};
			TimerList.Add(newBanTimer);
			SaveTimers();
			return newBanTimer;
		}

		public static BanTimer GetBanTimer(BanTimer timer)
		{
			return GetOrCreateBanTimer(timer.BanTimerID, timer.BanStartTime);
		}

		public static void SaveTimers()
		{
			DataStorage.SaveTimers(TimerList, timersFile);
		}
	}
}
