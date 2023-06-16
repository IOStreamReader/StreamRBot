using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRbot
{
	public static class BotCommands
	{
		public static long LastMeow=0;
		public static void Rand(object gid)
		{
			Random rd = new Random();
			int num = rd.Next() % 100;
			SRBotEngine.SendGroupMsg((long)gid, num.ToString());
		}
		public static void jrz(object gid)
		{
			Random random = new Random();
			long jrz = random.Next() % 1919810;
			SRBotEngine.SendGroupMsg((long)gid, "[CQ:image,file=https://dev.zapic.moe/RandomPic/yys/?seed="+jrz.ToString()+"]");
		}
		public static void meow(object gid)
		{
			Random random = new Random();
			int choice = random.Next() % 5;
			switch (choice)
			{
				case 0:
					{
						SRBotEngine.SendGroupMsg((long)gid, "喵~");
						break;
					}
				case 1:
					{
						SRBotEngine.SendGroupMsg((long)gid, "喵喵喵");
						break;
					}
				case 2:
					{
						SRBotEngine.SendGroupMsg((long)gid, "nya");
						break;
					}
				case 3:
					{
						SRBotEngine.SendGroupMsg((long)gid, "(蹭蹭手)");
						break;
					}
				case 4:
					{
						SRBotEngine.SendGroupMsg((long)gid, "喵呜");
						break;
					}

			}
			LastMeow = DateTimeOffset.Now.ToUnixTimeSeconds();
		}
		public static void tyt(object gid)
		{
			SRBotEngine.SendGroupMsg((long)gid, "[CQ:face,id=218]");
		}
	}
}
