using Spamer.MessageProvider;
using System.Net;

namespace Spamer
{
	public readonly struct SpamerSettings
	{
		public readonly float MaxMessagesPerSeconds;
		public readonly int ThreadsCount;

		public readonly ProtocolType Protocol;
		public readonly string TargetHostname;
		public readonly int TargetPort;

		public readonly float ConnectTimeout;

		public readonly IMessageProvider? MessageProvider;

		public readonly bool EnableRandomThreadStartOffset;

		public SpamerSettings()
		{
			MaxMessagesPerSeconds = -1;
			ThreadsCount = 1;

			Protocol = ProtocolType.TCP;
			TargetHostname = "loopback";
			TargetPort = 80;

			ConnectTimeout = 10;

			MessageProvider = null;

			EnableRandomThreadStartOffset = false;
		}

		public SpamerSettings(EntryPoint args, IMessageProvider MessageProvider)
		{
			MaxMessagesPerSeconds = args.MaxMessagesPerSeconds;
			ThreadsCount = args.ThreadsCount;

			Protocol = args.Protocol.Value;
			TargetHostname = args.TargetAddress;
			TargetPort = args.TargetPort.Value;

			ConnectTimeout = args.ConnectTimeout;

			this.MessageProvider = MessageProvider;

			EnableRandomThreadStartOffset = args.EnableRandomThreadStartOffset;
		}
	}
}