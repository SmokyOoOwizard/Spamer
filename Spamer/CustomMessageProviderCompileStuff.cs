using System.Reflection;

namespace Spamer
{
	public readonly struct CustomMessageProviderCompileStuff
	{
		public readonly Type MessageProviderType;
		public readonly Assembly CompiledAssembly;

		public CustomMessageProviderCompileStuff(Type messageProviderType, Assembly compiledAssembly)
		{
			MessageProviderType = messageProviderType;
			CompiledAssembly = compiledAssembly;
		}
	}
}