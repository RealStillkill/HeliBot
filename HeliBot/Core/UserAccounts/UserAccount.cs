using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliBot.Core.UserAccounts
{
	public class UserAccount
	{
		public ulong ID { get; set; }

		public uint BanTime{ get; set; }

		public uint BanCount{ get; set; }
	}
}