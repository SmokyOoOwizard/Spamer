using Spamer.NetworkWrapper;
using System.Diagnostics;

namespace Spamer
{
	public class ThreadSpamer : IDisposable
	{
		private readonly SpamerSettings settings;

		private Thread thread;
		private volatile bool running;

		private readonly ThreadSpamerStatistics statistics = new ThreadSpamerStatistics();
		public IThreadSpamerStatistics Statistics => statistics;

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
						statistics.Connected = network.Connected;
					}

					roundTimer.Restart();

					actionTimer.Restart();
					var data = settings.MessageProvider.GenerateMessage();
					actionTimer.Stop();

					statistics.MessageTime.Put(actionTimer.Elapsed.TotalSeconds);

					actionTimer.Restart();
					network.Send(data);
					actionTimer.Stop();

					statistics.MessagesSended++;

					statistics.SendTime.Put(actionTimer.Elapsed.TotalSeconds);

					roundTimer.Stop();
					statistics.RoundTime.Put(roundTimer.Elapsed.TotalSeconds);

					if (settings.EnableNewConnectionPerMessage)
					{
						network.Disconnect();
						statistics.Connected = network.Connected;
					}

					if (settings.MaxMessagesPerSeconds > 0)
					{
						var sleepTime = settings.MessageSendTime - roundTimer.Elapsed.TotalSeconds;
						if (sleepTime > 0)
						{
							statistics.SleepTime.Put(sleepTime);
							Thread.Sleep((int)(sleepTime * 1000));
						}
						else
						{
							statistics.SleepTime.Put(0);
						}
					}
				}
				catch
				{
					statistics.ErrorCount++;
					statistics.Connected = network.Connected;
				}
			}

			network.Dispose();
		}

		public void Dispose()
		{

		}
	}
}