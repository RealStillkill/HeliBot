using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliBot.Core.Bans
{
	public class Ban
	{
		public ulong UserID { get; set; }

		public string BanID { get; set; }

		public ulong BanTimeRemaining { get; set; }
	}
}
