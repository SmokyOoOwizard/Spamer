using System.Net.Sockets;

namespace Spamer.NetworkWrapper
{
	public class TcpNetworkWrapper : INetworkWrapper
	{
		private readonly SpamerSettings settings;

		private TcpClient? client;
		private NetworkStream? stream;

		public bool Connected => client?.Connected ?? false;

		public TcpNetworkWrapper(SpamerSettings settings)
		{
			this.settings = settings;
		}

		public void Connect()
		{
			if (client != null)
			{
				return;
			}

			client = new TcpClient();

			if (!client.ConnectAsync(settings.TargetHostname, settings.TargetPort).Wait((int)(settings.ConnectTimeout * 1000)))
			{
				client = null;
				throw new TimeoutException();
			}
			stream = client.GetStream();
		}

		public void Disconnect()
		{
			stream?.Close();
			client?.Close();

			client = null;
			stream = null;
		}

		public void Send(byte[] data)
		{
			stream?.Write(data, 0, data.Length);
		}

		public void Dispose()
		{
			stream?.Close();
			client?.Close();
		}
	}
}