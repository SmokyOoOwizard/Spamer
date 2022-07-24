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

			if (settings.CustomMessageProvider != null)
			{
				return Activator.CreateInstance(settings.CustomMessageProvider.Value.MessageProviderType) as IMessageProvider;
			}

			return new HelloWorldMessageProvider();
		}
	}
}