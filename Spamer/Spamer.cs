using System.Diagnostics;

namespace Spamer
{
	public class Spamer : ISpamerInfo
	{
		private readonly SpamerSettings settings;
		private ThreadSpamer[] threads = Array.Empty<ThreadSpamer>();
		private bool running = false;
		private Stopwatch timer = new Stopwatch();

		public int ThreadsCount { get { return threads.Length; } }

		public uint TotalSendedMessages
		{
			get
			{
				var total = 0u;
				for (int i = 0; i < threads.Length; i++)
				{
					total += threads[i].MessagesSended;
				}
				return total;
			}
		}

		public float AvgMessagesPerSeconds => (float)(TotalSendedMessages / Seconds);

		public double Seconds => timer.Elapsed.TotalSeconds;

		public ISpamerThreadInfo this[int threadIndex]
		{
			get
			{
				return threads[threadIndex];
			}
		}

		public Spamer(SpamerSettings settings)
		{
			this.settings = settings;

			threads = new ThreadSpamer[settings.ThreadsCount];
		}

		public void Start()
		{
			if (!running)
			{
				timer.Restart();
				running = true;
				for (int i = 0; i < threads.Length; i++)
				{
					threads[i] = new ThreadSpamer(settings);
					threads[i].Start();
				}
			}
		}

		public void Stop()
		{
			if (running)
			{
				for (int i = 0; i < threads.Length; i++)
				{
					threads[i].Stop();
					threads[i].Dispose();
				}
				timer.Stop();
				running = false;
			}
		}
	}
}