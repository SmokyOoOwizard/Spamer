namespace Spamer.NetworkWrapper
{

	public interface INetworkWrapper : IDisposable
	{
		bool Connected { get; }

		void Connect();
		void Send(byte[] data);
	}
}