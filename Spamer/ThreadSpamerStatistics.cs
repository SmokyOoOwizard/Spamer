namespace Spamer
{
	public class ThreadSpamerStatistics : IThreadSpamerStatistics
	{
		public bool Connected { get; set; }
		public uint ErrorCount { get; set; }

		public uint MessagesSended { get;  set; }

		IReadonlyPropertyStatistics IThreadSpamerStatistics.MessageTime => MessageTime;
		public PropertyStatistics MessageTime { get; private set; } = new PropertyStatistics();

		IReadonlyPropertyStatistics IThreadSpamerStatistics.SendTime => SendTime;
		public PropertyStatistics SendTime { get; private set; } = new PropertyStatistics();

		IReadonlyPropertyStatistics IThreadSpamerStatistics.RoundTime => RoundTime;
		public PropertyStatistics RoundTime { get; private set; } = new PropertyStatistics();

		IReadonlyPropertyStatistics IThreadSpamerStatistics.SleepTime => SleepTime;
		public PropertyStatistics SleepTime { get; private set; } = new PropertyStatistics();
	}
}