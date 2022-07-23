using System.Net.Sockets;

namespace Spamer.NetworkWrapper
{
	public class UdpNetworkWrapper : INetworkWrapper
	{
		private readonly SpamerSettings settings;

		private UdpClient client = new UdpClient();

		public bool Connected => true;

		public UdpNetworkWrapper(SpamerSettings settings)
		{
			this.settings = settings;
		}

		public void Connect()
		{
			client.Close();

			client.Connect(settings.TargetHostname, settings.TargetPort);
		}

		public void Disconnect()
		{
			client.Close();
		}

		public void Send(byte[] data)
		{
			client.Send(data);
		}

		public void Dispose()
		{
			client.Close();
		}
	}
}