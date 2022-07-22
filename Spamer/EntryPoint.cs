using McMaster.Extensions.CommandLineUtils;
using Spamer.MessageProvider;
using System.ComponentModel.DataAnnotations;

namespace Spamer
{
	public class EntryPoint
	{
		[Option(ShortName = "mps", Description = "Max messages per seconds for each thread")]
		public float MaxMessagesPerSeconds { get; set; } = -1;

		[Required]
		[Option(ShortName = "p", Description = "Protocol Type")]
		public ProtocolType? Protocol { get; set; }

		[Required]
		[IpValidation]
		[Option(ShortName = "t", LongName = "target", Description = "Target ip")]
		public string TargetAddress { get; set; }

		[Required]
		[Option(ShortName = "tp", LongName = "targetPort", Description = "Target port")]
		public int? TargetPort { get; set; }

		[Range(1, int.MaxValue)]
		[Option(ShortName = "n", LongName = "threads", Description = "Threads count")]
		public int ThreadsCount { get; set; } = 1;

		[Required]
		[FileMustExist]
		[Option(ShortName = "m", LongName = "message", Description = "Message file")]
		public string MessageFilePath { get; set; }

		[Option(ShortName = "r", LongName = "rawMessage", Description = "Don't parse message file/ send raw")]
		public bool SendRawMessage { get; set; } = false;

		[Option(ShortName = "to", LongName = "timeout", Description = "Connect timeout in seconds")]
		public float ConnectTimeout { get; set; } = 10;

		[Option(ShortName = "ro", LongName = "randomThreadOffset", Description = "Enable random thread start offset from 0 to 5 seconds")]
		public bool EnableRandomThreadStartOffset { get; set; } = false;



		private bool exit = false;

		private void OnExecute()
		{
			Spamer spamer = new Spamer(new SpamerSettings(this, new HelloWorldMessageProvider()));
			var info = spamer as ISpamerInfo;

			spamer.Start();

			Console.CancelKeyPress += (_, args) =>
			{
				args.Cancel = true;
				exit = true;
			};

			while (!exit)
			{
				Thread.Sleep(10);
				Console.Clear();

				for (int i = 0; i < info.ThreadsCount; i++)
				{
					var threadInfo = info[i];
					Console.WriteLine($"Thread {i}\t Round: {threadInfo.RoundTime.ToString("N4")}s\t " +
						$"Message: {threadInfo.MessageTime.ToString("N4")}s\t " +
						$"Send: {threadInfo.SendTime.ToString("N4")}s\t " +
						$"Sleep: {threadInfo.SleepTime.ToString("N4")}s\t " +
						$"Sended: {threadInfo.MessagesSended}\t " +
						$"Connected: {threadInfo.Connected}\t " +
						$"Errors: {threadInfo.ErrorCount}");
				}

				Console.WriteLine($"Time: {new TimeSpan(0, 0, 0, (int)info.Seconds, 0)}\t Total messages sended: {info.TotalSendedMessages}\t Avg messages per second: {info.AvgMessagesPerSeconds}");
			}

			Console.WriteLine("Clean up...");
			spamer.Stop();
		}
	}
}