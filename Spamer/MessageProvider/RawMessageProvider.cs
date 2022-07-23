namespace Spamer.MessageProvider
{
	public class RawMessageProvider : IMessageProvider
	{
		private readonly SpamerSettings settings;

		private byte[] data;

		public RawMessageProvider(SpamerSettings settings)
		{
			this.settings = settings;

			data = File.ReadAllBytes(settings.MessageFilePath);
		}

		public byte[] GenerateMessage()
		{
			return data;
		}
	}
}