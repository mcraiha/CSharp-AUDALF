using System.IO;

using CSharp_AUDALF;

namespace AudalfCli;


class Program
{
	static void Main(string[] args)
	{
		if (args.Length != 1)
		{
			Console.WriteLine("Give filename (that has AUDALF content) or hex string as only input!");
			return;
		}

		if (File.Exists(args[0]))
		{
			Console.WriteLine($"Loading file: {args[0]}");
			using (FileStream fs = File.OpenRead(args[0]))
			{
				if (!AUDALF_Deserialize.IsAUDALF(fs))
				{
					Console.WriteLine("Not an AUDALF input (incorrect FourCC)");
					return;
				}

				uint version = AUDALF_Deserialize.GetVersionNumber(fs);
				Console.WriteLine($"AUDALF input with version: {version}");

				ulong byteSize = AUDALF_Deserialize.GetByteSize(fs);
				Console.WriteLine($"Byte amount: {byteSize}");

				bool isDictionary = AUDALF_Deserialize.IsDictionary(fs);
				Console.WriteLine($"Is dictionary: {isDictionary}");
			}
		}
	}
}