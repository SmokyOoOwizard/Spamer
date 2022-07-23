namespace Spamer.MessageProvider
{
	public static class MessageProviderFactory
	{
		public static IMessageProvider CreateProvider(SpamerSettings settings)
		{
			if (settings.SendRawMessageFile)
			{
				return new RawMessageProvider(settings);
			}

			return new HelloWorldMessageProvider();
		}
	}
}