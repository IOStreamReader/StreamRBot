﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SRbot
{
	public static class SRBotEngine
	{
		public static object EnergyLock=new object();
		static Queue<Message> RecvGroupMsgQueue =new Queue<Message>();
		static string url = @"http://127.0.0.1";
		public static HttpListener listener = new HttpListener();
		public static void RequestGetMsg()
		{
			listener.Prefixes.Add(url + @":5701/");
			listener.Start();
			while (true)
			{
				try
				{
					HttpListenerContext context = listener.GetContext();
					if (context.Request.HttpMethod == "POST") // 判断是否为POST请求
					{
						byte[] data = new byte[context.Request.ContentLength64]; // 读取HTTP请求正文
						context.Request.InputStream.Read(data, 0, data.Length);
						string rawMessage = Encoding.UTF8.GetString(data);
						string responseString = "200";
						context.Response.ContentType = "text/plain";
						context.Response.ContentEncoding = Encoding.UTF8; byte[] buffer = Encoding.UTF8.GetBytes(responseString);
						context.Response.OutputStream.Write(buffer, 0, buffer.Length);
						context.Response.Close();
						Message msg = JsonConvert.DeserializeObject<Message>(rawMessage);
						if (msg != null && msg.message_type == "group")
						{
							RecvGroupMsgQueue.Enqueue(msg);
						}
					}
					Thread.Sleep(100);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.StackTrace);
					Thread.Sleep(3000);
				}
			}
		}
		public static void ProcessGroupMsg()
		{
			while(true)
			{
				if(RecvGroupMsgQueue.Count > 0)
				{
					Message tmp=RecvGroupMsgQueue.Dequeue();
					bool Reply=true;
					if(tmp.sender.user_id==1418780411)
						Reply=false;
					if(BotManagement.GetPermission(tmp.sender.user_id)!=null)
						if(BotManagement.GetPermission(tmp.sender.user_id).Blacklisted)
							Reply=false;
					BotManagement.WriteUserConfig();
					if (BotManagement.GetPermission(tmp.sender.user_id) == null)
						BotManagement.CreateDefault(tmp.sender.user_id);
					if(Reply)
					{
						if(tmp.message==".rand")
						{
							if (BotManagement.EnergyCast(tmp.sender.user_id, 1))
								ThreadPool.QueueUserWorkItem(new WaitCallback(BotCommands.Rand), tmp.group_id);
						}
						else if(tmp.message.Contains("ljyys"))
						{
							if (BotManagement.EnergyCast(tmp.sender.user_id, 3))
								ThreadPool.QueueUserWorkItem(new WaitCallback(BotCommands.jrz), tmp.group_id);
						}
						else if(tmp.message.Contains("🐱") && (DateTimeOffset.Now.ToUnixTimeSeconds() - BotCommands.LastMeow >= 30))
						{
							if(BotManagement.EnergyCast(tmp.sender.user_id,1))
								ThreadPool.QueueUserWorkItem(new WaitCallback(BotCommands.meow), tmp.group_id);
						}
						else if(tmp.message=="tyt"&&tmp.sender.user_id!= 2135798420)
						{
							if (BotManagement.EnergyCast(tmp.sender.user_id, 1))
								ThreadPool.QueueUserWorkItem(new WaitCallback(BotCommands.tyt), tmp.group_id);
						}
					}
				}
				Thread.Sleep(100);
			}
		}
		public static void SendGroupMsg(Int64 gid,string message)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + @":5700/send_group_msg");
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				byte[] postBytes = Encoding.UTF8.GetBytes("group_id=" + gid.ToString() + "&message=" + message);
				request.ContentLength = postBytes.Length;
				Stream requestStream = request.GetRequestStream();
				requestStream.Write(postBytes, 0, postBytes.Length);
				requestStream.Close();
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream responseStream = response.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
				string responseString = streamReader.ReadToEnd();
				streamReader.Close();
				responseStream.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Thread.Sleep(3000);
			}
		}
		public class Sender
		{
			public string nickname { get; set; }
			public Int64 user_id { get; set; }
		}
		public class Message
		{
			public bool group { get; set; }
			public Int64 group_id { get; set; }
			public Int32 message_id { get; set; }
			public Int32 real_id { get; set; }
			public string message_type { get; set; }
			public Sender sender { get; set; }
			public Int32 type { get; set; }
			public string message { get; set; }
			public string raw_message { get; set; }

		}
	}
}
