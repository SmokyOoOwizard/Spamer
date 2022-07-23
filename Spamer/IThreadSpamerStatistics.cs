namespace Spamer
{
	public interface IThreadSpamerStatistics
	{
		bool Connected { get; }
		uint ErrorCount { get; }

		uint MessagesSended { get; }

		IReadonlyPropertyStatistics MessageTime { get; }
		IReadonlyPropertyStatistics SendTime { get; }
		IReadonlyPropertyStatistics RoundTime { get; }
		IReadonlyPropertyStatistics SleepTime { get; }
	}
}