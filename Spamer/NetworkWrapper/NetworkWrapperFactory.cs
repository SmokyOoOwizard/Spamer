namespace Spamer.NetworkWrapper
{
	public static class NetworkWrapperFactory
	{
		public static INetworkWrapper CreateWrapper(SpamerSettings settings)
		{
			switch (settings.Protocol)
			{
				case ProtocolType.TCP:
					return new TcpNetworkWrapper(settings);
				case ProtocolType.UDP:
					return new UdpNetworkWrapper(settings);
			}
			return null;
		}
	}
}