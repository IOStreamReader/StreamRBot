using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpConfig;

namespace SRbot
{
	public static class BotManagement
	{
		static Dictionary<long,int> userenergy = new Dictionary<long,int>();
		static Configuration userconfig=new Configuration();
		static Dictionary<Int64, Permission> userpermission = new Dictionary<Int64, Permission>();
		public static void WriteUserConfig()
		{
			Configuration config = new Configuration();
			try
			{
				foreach (KeyValuePair<Int64, Permission> item in userpermission)
				{
					config[item.Key.ToString()]["MaxLimit"].IntValue = item.Value.MaxLimit;
					config[item.Key.ToString()]["IgnoreLimit"].BoolValue = item.Value.IgnoreLimit;
					config[item.Key.ToString()]["AllowDebug"].BoolValue = item.Value.AllowDebug;
					config[item.Key.ToString()]["Blacklisted"].BoolValue = item.Value.Blacklisted;
					config[item.Key.ToString()]["AllowHeavyload"].BoolValue = item.Value.AllowHeavyload;
					config[item.Key.ToString()]["Restricted"].BoolValue = item.Value.Restricted;
				}
				userconfig = config;
				config.SaveToFile("userpermission.inf");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
		public static void ReadUserConfig()
		{
			try
			{
				userconfig = Configuration.LoadFromFile("userpermission.inf");
			}
			catch (IOException e)
			{
				File.WriteAllText("userpermission.inf", "");
				Console.WriteLine(e.ToString());
			}
		}
		public static bool UserCfgExist()
		{
			return File.Exists("userpermission.inf");
		}
		public static Permission GetPermission(Int64 id)
		{
			Permission permission = new Permission();
			try
			{
				permission.MaxLimit = userconfig[(int)id]["MaxLimit"].IntValue;
				permission.IgnoreLimit = userconfig[(int)id]["IgnoreLimit"].BoolValue;
				permission.AllowDebug = userconfig[(int)id]["AllowDebug"].BoolValue;
				permission.Blacklisted = userconfig[(int)id]["BlackListed"].BoolValue;
				permission.AllowHeavyload = userconfig[(int)id]["AllowHeavyload"].BoolValue;
				permission.Restricted = userconfig[(int)id]["Restricted"].BoolValue;
				return permission;
			}
			catch
			{
				CreateDefault(id);
				return permission;
			}
		}
		public static void CreateDefault(long uid)
		{
			Permission permission=new Permission();
			try
			{
				userpermission.Add(uid, permission);
			}
			catch (Exception)
			{ }
		}
		public static bool EnergyCast(long uid,int cost)
		{
			lock (SRBotEngine.EnergyLock)
			{
				if (userenergy.ContainsKey(uid))
				{
					if (userenergy[uid] + cost <= GetPermission(uid).MaxLimit)
					{
						userenergy[uid] += cost;
						return true;
					}
					else
						return false;
				}
				else
				{
					userenergy.Add(uid, cost);
					return true;
				}
			}
		}
		public static void tenminutetick()
		{
			while (true)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(ResetEnergy));
				Thread.Sleep(600000);

			}
		}
		public static void ResetEnergy(object obj)
		{
			lock (SRBotEngine.EnergyLock)
			{

				foreach (KeyValuePair<long, int> kvp in userenergy)
				{
					userenergy[kvp.Key] = 0;

				}
			}

		}
	}
}
