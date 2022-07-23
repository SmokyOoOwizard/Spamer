namespace Spamer.NetworkWrapper
{

	public interface INetworkWrapper : IDisposable
	{
		bool Connected { get; }

		void Connect();
		void Disconnect();

		void Send(byte[] data);
	}
}