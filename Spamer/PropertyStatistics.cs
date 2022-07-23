namespace Spamer
{
	public class PropertyStatistics : IReadonlyPropertyStatistics
	{
		public double Value { get; private set; }

		public double Avg10Value { get; private set; }
		public double Avg100Value { get; private set; }

		private List<double> values = new List<double>(100);

		public void Put(double value)
		{
			if (values.Count >= 100)
			{
				values.RemoveAt(0);
			}

			values.Add(value);

			Value = value;

			Avg10Value = values.TakeLast(10).Sum() / (values.Count >= 10 ? 10 : values.Count);
			Avg100Value = values.TakeLast(100).Sum() / (values.Count >= 100 ? 100 : values.Count);
		}
	}
}