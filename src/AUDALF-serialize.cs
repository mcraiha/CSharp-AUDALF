using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_AUDALF
{
	public static class AUDALF_Serialize
	{
		private static readonly string KeyCannotBeNullError = "Key cannot be null!";

		public static byte[] Serialize(IEnumerable<byte> bytes)
		{
			IEnumerable<object> objects = bytes.Cast<object>();
			// Generate Key and value pairs section
			var generateResult = GenerateListKeyValuePairs(objects, typeof(byte));
			return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.specialType);
		}

		public static byte[] Serialize(IEnumerable<int> ints)
		{
			IEnumerable<object> objects = ints.Cast<object>();
			// Generate Key and value pairs section
			var generateResult = GenerateListKeyValuePairs(objects, typeof(int));
			return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.specialType);
		}

		public static byte[] Serialize(IEnumerable<string> strings)
		{
			IEnumerable<object> objects = strings.Cast<object>();
			// Generate Key and value pairs section
			var generateResult = GenerateListKeyValuePairs(objects, typeof(string));
			return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.specialType);
		}

		public static byte[] Serialize(IEnumerable<float> floats)
		{
			IEnumerable<object> objects = floats.Cast<object>();
			// Generate Key and value pairs section
			var generateResult = GenerateListKeyValuePairs(objects, typeof(float));
			return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.specialType);
		}

		public static byte[] Serialize(Dictionary<string, string> dictionary)
		{
			var valueTypes = dictionary.ToDictionary(pair => pair.Key, pair => typeof(string));
			// Generate Key and value pairs section
			var generateResult = GenerateDictionaryKeyValuePairs(dictionary, valueTypes);

			return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.GetAUDALFtypeWithDotnetType(typeof(string)));
		}

		public static byte[] Serialize(Dictionary<string, object> dictionary, Dictionary<string, Type> valueTypes = null)
		{
			// Generate Key and value pairs section
			var generateResult = GenerateDictionaryKeyValuePairs(dictionary, valueTypes);

			return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.GetAUDALFtypeWithDotnetType(typeof(string)));
		}

		private static byte[] GenericSerialize(byte[] keyValuePairsBytes, List<ulong> keyValuePairsOffsets, byte[] keyTypeAsBytes)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(stream))
				{
					// Write header
					WriteHeader(writer);

					// Write index section
					WriteIndexSection(writer, keyTypeAsBytes, keyValuePairsOffsets);

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

		private static (byte[] bytes, List<ulong> positions) GenerateDictionaryKeyValuePairs<T,V>(Dictionary<T, V> pairs, Dictionary<T, Type> valueTypes = null)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				// Use UTF-8 because it has best support in different environments
				using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8))
				{
					List<ulong> offsets = new List<ulong>();;
					foreach (var pair in pairs)
					{
						Type typeOfValue = valueTypes != null && valueTypes.ContainsKey(pair.Key) ? valueTypes[pair.Key] : null;
						offsets.Add(WriteOneDictionaryKeyValuePair(writer, pair.Key, pair.Value, typeOfValue));
					}
					return (stream.ToArray(), offsets);
				}
			}
		}

		/*private static (byte[] bytes, List<ulong> positions) GenerateDictionaryKeyValuePairs(Dictionary<string, object> pairs, Dictionary<string, Type> valueTypes = null)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				// Use UTF-8 because it has best support in different environments
				using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8))
				{
					List<ulong> offsets = new List<ulong>();;
					foreach (var pair in pairs)
					{
						Type typeOfValue = valueTypes != null && valueTypes.ContainsKey(pair.Key) ? valueTypes[pair.Key] : null;
						offsets.Add(WriteOneDictionaryKeyValuePair(writer, pair.Key, pair.Value, typeOfValue));
					}
					return (stream.ToArray(), offsets);
				}
			}
		}*/

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

		private static ulong WriteOneDictionaryKeyValuePair(BinaryWriter writer, object key, object value, Type originalType)
		{
			// Store current offset, because different types can take different amount of space
			ulong returnValue = (ulong)writer.BaseStream.Position;

			GenericWrite(writer, key, originalType, isKey: true);
			GenericWrite(writer, value, originalType, isKey: false);

			return returnValue;
		}

		private static ulong WriteOneListKeyValuePair(BinaryWriter writer, ulong index, object value, Type originalType)
		{
			// Store current offset, because different types can take different amount of space
			ulong returnValue = (ulong)writer.BaseStream.Position;

			// Write Index number which is always 8 bytes
			writer.Write(index);

			GenericWrite(writer, value, originalType, isKey: false);

			return returnValue;
		}

		private static void GenericWrite(BinaryWriter writer, Object variableToWrite, Type originalType, bool isKey)
		{
			if (typeof(byte) == originalType)
			{
				WriteByte(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(int) == originalType)
			{
				WriteInt(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(float) == originalType)
			{
				WriteFloat(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(string) == originalType)
			{
				WriteString(writer, variableToWrite, originalType, isKey: isKey);
			}
		}

		private static void WriteByte(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single byte takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}
			
			// Write byte as 1 byte
			writer.Write((byte)valueToWrite);
			// Write 7 bytes of padding
			PadWithZeros(writer, 7);
		}

		private static void WriteInt(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single int takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}
			
			// Write int as 4 bytes
			writer.Write((int)valueToWrite);
			// Write 4 bytes of padding
			PadWithZeros(writer, 4);
		}

		private static void WriteFloat(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single float takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}
			
			// Write float as 4 bytes
			writer.Write((float)valueToWrite);
			// Write 4 bytes of padding
			PadWithZeros(writer, 4);
		}

		private static void WriteString(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single string has variable length
			string stringToWrite = (string)valueToWrite;
			if (stringToWrite == null)
			{
				if (isKey)
				{
					throw new ArgumentNullException(KeyCannotBeNullError);
				}

				// Write special null, this is always 16 bytes
				WriteSpecialNullType(writer, originalType);
			}
			else
			{
				if (!isKey)
				{
					// Write value type ID (8 bytes)
					writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
				}

				// Get bytes that will be written, (UTF-8 as default)
				byte[] bytesToWrite = Encoding.UTF8.GetBytes(stringToWrite);

				// Write length as 8 bytes
				ulong stringLengthAsBytes = (ulong)bytesToWrite.LongLength;
				writer.Write(stringLengthAsBytes);

				// Write string content 
				writer.Write(bytesToWrite);
				
				// Pad with zeroes if needed
				ulong currentPos = (ulong)writer.BaseStream.Position;
				ulong nextDivisableBy8 = Definitions.NextDivisableBy8(currentPos);
				PadWithZeros(writer, nextDivisableBy8 - currentPos);
			}
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

		
	}
}
