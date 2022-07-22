using Spamer.NetworkWrapper;
using System.Diagnostics;

namespace Spamer
{
	public class ThreadSpamer : IDisposable, ISpamerThreadInfo
	{
		private readonly SpamerSettings settings;

		private Thread thread;
		private volatile bool running;

		public bool Connected { get; private set; }
		public uint ErrorCount { get; private set; }

		public double MessageTime { get; private set; }
		public double SendTime { get; private set; }
		public double RoundTime { get; private set; }
		public double SleepTime { get; private set; }
		public uint MessagesSended { get; private set; }

		public ThreadSpamer(SpamerSettings settings)
		{
			this.settings = settings;
		}

		public void Start()
		{
			if (!running)
			{
				running = true;
				thread = new Thread(threadWork);
				thread.Start();
			}
		}

		public void Stop()
		{
			if (running)
			{
				running = false;
				thread.Join();
			}
		}

		private void threadWork()
		{
			var network = NetworkWrapperFactory.CreateWrapper(settings);
			var actionTimer = new Stopwatch();
			var roundTimer = new Stopwatch();

			var maxSendTime = 1 / settings.MaxMessagesPerSeconds;

			if (settings.EnableRandomThreadStartOffset)
			{
				Thread.Sleep(Random.Shared.Next(5 * 1000));
			}

			while (running)
			{
				try
				{
					if (!network.Connected)
					{
						network.Connect();
						Connected = network.Connected;
					}

					roundTimer.Restart();

					actionTimer.Restart();
					var data = settings.MessageProvider.GenerateMessage();
					actionTimer.Stop();

					MessageTime = actionTimer.Elapsed.TotalSeconds;

					actionTimer.Restart();
					network.Send(data);
					MessagesSended++;
					actionTimer.Stop();

					SendTime = actionTimer.Elapsed.TotalSeconds;

					roundTimer.Stop();
					RoundTime = roundTimer.Elapsed.TotalSeconds;

					SleepTime = 0;
					if (settings.MaxMessagesPerSeconds > 0)
					{
						var sleepTime = maxSendTime - roundTimer.Elapsed.TotalSeconds;
						if (sleepTime > 0)
						{
							SleepTime = sleepTime;
							Thread.Sleep((int)(sleepTime * 1000));
						}
					}
				}
				catch
				{
					ErrorCount++;
					Connected = network.Connected;
				}
			}
		}

		public void Dispose()
		{

		}
	}
}