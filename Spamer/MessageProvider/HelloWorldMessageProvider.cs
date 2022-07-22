using System.Text;

namespace Spamer.MessageProvider
{
	public class HelloWorldMessageProvider : IMessageProvider
	{
		public byte[] GenerateMessage()
		{
			return Encoding.UTF8.GetBytes("Hello World!!!");
		}
	}
}