namespace Spamer
{
	public interface ISpamerThreadInfo
	{
		bool Connected { get; }
		uint ErrorCount { get; }


		double MessageTime { get; }
		double SendTime { get; }
		double RoundTime { get; }
		double SleepTime { get; }

		uint MessagesSended { get; }
	}
}