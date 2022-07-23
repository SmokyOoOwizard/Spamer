using Spamer.MessageProvider;
using System.Net;

namespace Spamer
{
	public readonly struct SpamerSettings
	{
		public readonly float MaxMessagesPerSeconds;
		public readonly float MessageSendTime;
		public readonly int ThreadsCount;

		public readonly ProtocolType Protocol;
		public readonly string TargetHostname;
		public readonly int TargetPort;

		public readonly float ConnectTimeout;

		public readonly IMessageProvider? MessageProvider;

		public readonly string MessageFilePath;

		public readonly bool SendRawMessageFile;

		public readonly bool EnableRandomThreadStartOffset;
		public readonly bool EnableNewConnectionPerMessage;

		public SpamerSettings()
		{
			MaxMessagesPerSeconds = -1;
			MessageSendTime = 0;
			ThreadsCount = 1;

			Protocol = ProtocolType.TCP;
			TargetHostname = "loopback";
			TargetPort = 80;

			ConnectTimeout = 10;

			MessageFilePath = string.Empty;

			SendRawMessageFile = true;

			EnableRandomThreadStartOffset = false;
			EnableNewConnectionPerMessage = false;

			MessageProvider = null;
		}

		public SpamerSettings(EntryPoint args)
		{
			MaxMessagesPerSeconds = args.MaxMessagesPerSeconds;
			MessageSendTime = 1 / args.MaxMessagesPerSeconds;
			ThreadsCount = args.ThreadsCount;

			Protocol = args.Protocol.Value;
			TargetHostname = args.TargetAddress;
			TargetPort = args.TargetPort.Value;

			ConnectTimeout = args.ConnectTimeout;

			MessageFilePath = args.MessageFilePath;

			SendRawMessageFile = args.SendRawMessage;

			EnableRandomThreadStartOffset = args.EnableRandomThreadStartOffset;
			EnableNewConnectionPerMessage = args.EnableNewConnectionPerMessage;

			// HACK
			MessageProvider = null;
			this.MessageProvider = MessageProviderFactory.CreateProvider(this);
		}
	}
}