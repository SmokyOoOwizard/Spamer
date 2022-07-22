using System.Net.Sockets;

namespace Spamer.NetworkWrapper
{
	public class TcpNetworkWrapper : INetworkWrapper
	{
		private readonly SpamerSettings settings;

		private TcpClient client = new TcpClient();
		private NetworkStream? stream;

		public bool Connected => client.Connected;

		public TcpNetworkWrapper(SpamerSettings settings)
		{
			this.settings = settings;
		}

		public void Connect()
		{
			if (stream != null)
			{
				stream.Close();
				client.Close();
			}

			if (!client.ConnectAsync(settings.TargetHostname, settings.TargetPort).Wait((int)(settings.ConnectTimeout * 1000)))
			{
				throw new TimeoutException();
			}
			stream = client.GetStream();
		}

		public void Send(byte[] data)
		{
			stream?.Write(data, 0, data.Length);
		}

		public void Dispose()
		{
			stream?.Close();
			client.Close();
		}
	}
}