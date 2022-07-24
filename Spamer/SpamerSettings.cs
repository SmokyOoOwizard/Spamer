using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Spamer.MessageProvider;
using System.Net;
using System.Reflection;

namespace Spamer
{
	public readonly struct SpamerSettings
	{
		public readonly float MaxMessagesPerSeconds;
		public readonly float MessageSendTime;
		public readonly int ThreadsCount;

		public readonly ProtocolType Protocol;
		public readonly string TargetHostname;
		public readonly int TargetPort;

		public readonly float ConnectTimeout;

		public readonly string MessageFilePath;

		public readonly bool SendRawMessageFile;

		public readonly bool EnableRandomThreadStartOffset;
		public readonly bool EnableNewConnectionPerMessage;

		public readonly CustomMessageProviderCompileStuff? CustomMessageProvider;

		public SpamerSettings()
		{
			MaxMessagesPerSeconds = -1;
			MessageSendTime = 0;
			ThreadsCount = 1;

			Protocol = ProtocolType.TCP;
			TargetHostname = "loopback";
			TargetPort = 80;

			ConnectTimeout = 10;

			MessageFilePath = string.Empty;

			SendRawMessageFile = true;

			EnableRandomThreadStartOffset = false;
			EnableNewConnectionPerMessage = false;

			CustomMessageProvider = null;
		}

		public SpamerSettings(EntryPoint args)
		{
			MaxMessagesPerSeconds = args.MaxMessagesPerSeconds;
			MessageSendTime = 1 / args.MaxMessagesPerSeconds;
			ThreadsCount = args.ThreadsCount;

			Protocol = args.Protocol.Value;
			TargetHostname = args.TargetAddress;
			TargetPort = args.TargetPort.Value;

			ConnectTimeout = args.ConnectTimeout;

			MessageFilePath = args.MessageFilePath;

			SendRawMessageFile = args.SendRawMessage;

			EnableRandomThreadStartOffset = args.EnableRandomThreadStartOffset;
			EnableNewConnectionPerMessage = args.EnableNewConnectionPerMessage;

			CustomMessageProvider = null;
			if (!SendRawMessageFile)
			{
				CustomMessageProvider = LoadCustomMessageProvider(this);
				Console.WriteLine($"Message provider: {CustomMessageProvider?.MessageProviderType.Name}");
			}
		}

		private static CustomMessageProviderCompileStuff? LoadCustomMessageProvider(SpamerSettings settings)
		{
			Console.Write("Compile custom message provider... ");

			try
			{
				SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(settings.MessageFilePath));
				var errors = syntaxTree.GetDiagnostics().Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error).ToArray();
				if (errors.Length > 0)
				{
					throw new Exception(String.Join('\n', errors.Select(e => e.GetMessage())));
				}

				string assemblyName = Path.GetRandomFileName();
				MetadataReference[] references = new MetadataReference[]
				{
					MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
					MetadataReference.CreateFromFile(typeof(Program).Assembly.Location)
				};

				CSharpCompilation compilation = CSharpCompilation.Create(
					assemblyName,
					syntaxTrees: new[] { syntaxTree },
					references: references,
					options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

				using (var ms = new MemoryStream())
				{
					EmitResult result = compilation.Emit(ms);

					if (!result.Success)
					{
						errors = result.Diagnostics.Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error).ToArray();

						if (errors.Length > 0)
						{
							throw new Exception(String.Join('\n', errors.Select(e => e.GetMessage())));
						}
					}
					else
					{
						ms.Seek(0, SeekOrigin.Begin);
						Assembly assembly = Assembly.Load(ms.ToArray());

						var type = assembly.DefinedTypes.Where(t => t.IsAssignableTo(typeof(IMessageProvider))).FirstOrDefault();
						if (type == null)
						{
							throw new Exception("Message provider not found");
						}

						return new CustomMessageProviderCompileStuff(type, assembly);
					}
				}
			}
			catch
			{
				Console.WriteLine("Error!!!");

				throw;
			}
			finally
			{
				Console.WriteLine("Done!!!");
			}

			return null;
		}
	}
}