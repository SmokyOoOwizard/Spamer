namespace Spamer
{
	public interface ISpamerInfo
	{
		int ThreadsCount { get; }

		uint TotalSendedMessages { get; }
		float AvgMessagesPerSeconds { get; }
		double Seconds { get; }

		ISpamerThreadInfo this[int threadIndex] { get; }
	}
}