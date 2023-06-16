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
		public static Thread ProcessGroupMsg = new Thread(new ThreadStart(SRBotEngine.ProcessGroupMsg));
		public static Thread Tick10m = new Thread(new ThreadStart(BotManagement.tenminutetick));
		static void Main(string[] args)
		{
			Console.WriteLine("SRbot is starting");
			BotManagement.ReadUserConfig();
			ReadMsg.Start();
			ProcessGroupMsg.Start();
			Tick10m.Start();
		}
		~MainThread()
		{
			ReadMsg.Abort();
			ProcessGroupMsg.Abort();
			Tick10m.Abort();
			SRBotEngine.listener.Close();
		}
	}
}
