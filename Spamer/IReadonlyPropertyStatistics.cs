namespace Spamer
{
	public interface IReadonlyPropertyStatistics
	{
		double Value { get; }

		double Avg10Value { get; }
		double Avg100Value { get; }
	}
}