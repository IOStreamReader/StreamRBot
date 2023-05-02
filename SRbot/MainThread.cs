using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SRbot
{
	internal class MainThread
	{

		public static Thread ReadMsg = new Thread(new ThreadStart(SRBotEngine.RequestGetMsg));
		public static Thread ProcessGroupMsg=new Thread(new ThreadStart(SRBotEngine.ProcessGroupMsg));
		static void Main(string[] args)
		{
			Console.WriteLine("SRbot is Launching, press any key to continue");
			Console.ReadKey();
			ReadMsg.Start();
			ProcessGroupMsg.Start();
		}
	}
}
