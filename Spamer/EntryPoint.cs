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
		[Option(ShortName = "t", LongName = "target", Description = "Target host")]
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

		[Option(ShortName = "ne", LongName = "newConnectionPerMessage", Description = "Enable new connection for each message")]
		public bool EnableNewConnectionPerMessage { get; set; } = false;



		private bool exit = false;

		private void OnExecute()
		{
			Spamer spamer = new Spamer(new SpamerSettings(this));
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

				const string TIME_FORMAT = "N5";

				for (int i = 0; i < spamer.ThreadsCount; i++)
				{
					var threadInfo = spamer[i];
					Console.WriteLine($"Thread {i}\t Round: {threadInfo.Statistics.RoundTime.Avg100Value.ToString(TIME_FORMAT)}s\t " +
						$"Message: {threadInfo.Statistics.MessageTime.Avg100Value.ToString(TIME_FORMAT)}s\t " +
						$"Send: {threadInfo.Statistics.SendTime.Avg100Value.ToString(TIME_FORMAT)}s\t " +
						$"Sleep: {threadInfo.Statistics.SleepTime.Avg100Value.ToString(TIME_FORMAT)}s\t " +
						$"Sended: {threadInfo.Statistics.MessagesSended}\t " +
						$"Connected: {threadInfo.Statistics.Connected}\t " +
						$"Errors: {threadInfo.Statistics.ErrorCount}");
				}

				Console.WriteLine($"Time: {new TimeSpan(0, 0, 0, (int)spamer.Seconds, 0)}\t " +
					$"Total messages sended: {spamer.TotalSendedMessages}\t " +
					$"Avg messages per second: {spamer.AvgMessagesPerSeconds.ToString(TIME_FORMAT)}");
			}

			Console.Write("Clean up...");
			spamer.Stop();
			Console.WriteLine(" Done!");
		}
	}
}