using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_AUDALF
{
	public static class AUDALF_Serialize
	{
		public static byte[] Serialize(IEnumerable<int> ints)
		{
			IEnumerable<object> objects = ints.Cast<object>();
			// Generate Key and value pairs section
			var generateResult = GenerateListKeyValuePairs(objects, typeof(int));
			return GenericSerialize(generateResult.bytes, generateResult.positions);
		}

		public static byte[] Serialize(IEnumerable<string> strings)
		{
			IEnumerable<object> objects = strings.Cast<object>();
			// Generate Key and value pairs section
			var generateResult = GenerateListKeyValuePairs(objects, typeof(string));
			return GenericSerialize(generateResult.bytes, generateResult.positions);
		}

		public static byte[] Serialize(IEnumerable<float> floats)
		{
			IEnumerable<object> objects = floats.Cast<object>();
			// Generate Key and value pairs section
			var generateResult = GenerateListKeyValuePairs(objects, typeof(float));
			return GenericSerialize(generateResult.bytes, generateResult.positions);
		}

		private static byte[] GenericSerialize(byte[] keyValuePairsBytes, List<ulong> keyValuePairsOffsets)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(stream))
				{
					// Write header
					WriteHeader(writer);

					// Write index section
					WriteIndexSection(writer, Definitions.specialType, keyValuePairsOffsets);

					// Write Key and value pairs section
					writer.Write(keyValuePairsBytes);

					// Get total length
					ulong totalLengthInBytes = (ulong)writer.BaseStream.Length;
					writer.Seek(Definitions.payloadSizeOffset, SeekOrigin.Begin);
					writer.Write(totalLengthInBytes);
				}

				return stream.ToArray();
			}
		}

		private static void WriteHeader(BinaryWriter writer)
		{
			// First write FourCC
			writer.Write(Definitions.fourCC);

			// Then version number
			writer.Write(Definitions.versionNumber);

			// Then some zeroes for payload size since this will be fixed later
			writer.Write(Definitions.payloadSizePlaceholder);
		}

		private static void WriteIndexSection(BinaryWriter writer, byte[] keyTypeAsBytes, List<ulong> positions)
		{
			ulong indexCount = (ulong)positions.Count;

			ulong totalByteCountOfHeaderAndIndexSection = indexCount * 8 + 16 + 16;

			// Write index count as 8 bytes
			writer.Write(indexCount);

			// Write Key type as 8 bytes
			writer.Write(keyTypeAsBytes);

			// Write each position separately (index count * 8 bytes)
			foreach (ulong pos in positions)
			{
				writer.Write(pos + totalByteCountOfHeaderAndIndexSection);
			}
		}

		private static (byte[] bytes, List<ulong> positions) GenerateListKeyValuePairs(IEnumerable<object> values, Type originalType)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				// Use UTF-8 because it has best support in different environments
				using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8))
				{
					List<ulong> offsets = new List<ulong>();
					ulong index = 0;
					foreach (object o in values)
					{
						offsets.Add(WriteOneListKeyValuePair(writer, index, o, originalType));
						index++;
					}
					return (stream.ToArray(), offsets);
				}
			}
		}

		private static ulong WriteOneListKeyValuePair(BinaryWriter writer, ulong index, object value, Type originalType)
		{
			// Store current offset, because different types can take different amount of space
			ulong returnValue = (ulong)writer.BaseStream.Position;

			// Write Index number which is always 8 bytes
			writer.Write(index);

			if (typeof(int) == originalType)
			{
				// Single int value is 16 bytes

				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
				// Write int as 4 bytes
				writer.Write((int)value);
				// Write 4 bytes of padding
				PadWithZeros(writer, 4);
			}
			else if (typeof(float) == originalType)
			{
				// Single float value is 16 bytes

				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
				// Write float as 4 bytes
				writer.Write((float)value);
				// Write 4 bytes of padding
				PadWithZeros(writer, 4);
			}
			else if (typeof(string) == originalType)
			{
				// Single string has variable length

				string stringToWrite = (string)value;
				if (stringToWrite == null)
				{
					// Write special null, this is always 16 bytes
					WriteSpecialNullType(writer, originalType);
				}
				else
				{
					// Write value type ID (8 bytes)
					writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));

					// Get bytes that will be written, (UTF-8 as default)
					byte[] bytesToWrite = Encoding.UTF8.GetBytes(stringToWrite);

					// Write length as 8 bytes
					ulong stringLengthAsBytes = (ulong)bytesToWrite.LongLength;
					writer.Write(stringLengthAsBytes);

					// Write string content 
					writer.Write(bytesToWrite);
					
					// Pad with zeroes if needed
					ulong currentPos = (ulong)writer.BaseStream.Position;
					ulong nextDivisableBy8 = NextDivisableBy8(currentPos);
					PadWithZeros(writer, nextDivisableBy8 - currentPos);
				}
			}

			return returnValue;
		}

		private static void WriteSpecialNullType(BinaryWriter writer, Type typeToWrite)
		{
			writer.Write(Definitions.specialType);
			writer.Write(Definitions.GetAUDALFtypeWithDotnetType(typeToWrite));
		}

		private static readonly byte zeroByte = 0;
		private static void PadWithZeros(BinaryWriter writer, int howManyZeros)
		{
			for (int i = 0; i < howManyZeros; i++)
			{
				writer.Write(zeroByte);
			}
		}

		private static void PadWithZeros(BinaryWriter writer, ulong howManyZeros)
		{
			for (ulong u = 0; u < howManyZeros; u++)
			{
				writer.Write(zeroByte);
			}
		}

		private static ulong NextDivisableBy8(ulong current)
		{
			ulong bits = current & 7;
    		if (bits == 0) return current;
    		return current + (8-bits);
		}
	}
}
