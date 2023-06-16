using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRbot
{
	public class Permission
	{
		public bool AllowDebug { get; set; }
		public bool IgnoreLimit { get; set; }
		public bool AllowHeavyload { get; set; }
		public int MaxLimit { get; set; }
		public bool Blacklisted { get; set; }
		public bool Restricted { get; set; }
		public Permission()
		{
			AllowDebug = false;
			IgnoreLimit = false;
			AllowHeavyload = true;
			MaxLimit = 30;
			Blacklisted = false;
			Restricted = false;
		}
	}
}
