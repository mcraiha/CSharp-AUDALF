using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Numerics;

namespace CSharp_AUDALF
{
	/// <summary>
	/// Static class for AUDALF serialization 
	/// </summary>
	public static class AUDALF_Serialize
	{
		private static readonly string KeyCannotBeNullError = "Key cannot be null!";
		private static readonly string ValueCannotBeNullWithoutKnownValueTypeError = "You cannot use null value without known value type!";

		/// <summary>
		/// Serialize a IEnumerable structure to AUDALF bytes
		/// </summary>
		/// <param name="ienumerableStructure">IEnumerable structure</param>
		/// <param name="serializationSettings">Optional serialization settings</param>
		/// <typeparam name="T">Type to serialize</typeparam>
		/// <returns>Byte array</returns>
		public static byte[] Serialize<T>(IEnumerable<T> ienumerableStructure, SerializationSettings serializationSettings = null)
		{
			IEnumerable<object> objects = ienumerableStructure.Cast<object>();
			// Generate Key and value pairs section
			var generateResult = GenerateListKeyValuePairs(objects, typeof(T), serializationSettings);
			return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.specialType);
		}

		/// <summary>
		/// Serialize a byte to byte dictionary to AUDALF bytes
		/// </summary>
		/// <param name="dictionary">Byte to byte Dictionary to serialize</param>
		/// <param name="serializationSettings">Optional serialization settings</param>
		/// <returns>Byte array</returns>
		public static byte[] Serialize(IDictionary<byte, byte> dictionary, SerializationSettings serializationSettings = null)
		{
			var valueTypes = dictionary.ToDictionary(pair => pair.Key, pair => typeof(byte));
			// Generate Key and value pairs section
			var generateResult = GenerateDictionaryKeyValuePairs(dictionary, valueTypes, serializationSettings);

			return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.GetAUDALFtypeWithDotnetType(typeof(byte)));
		}

		/// <summary>
		/// Serialize a int to int dictionary to AUDALF bytes
		/// </summary>
		/// <param name="dictionary">Int to int Dictionary to serialize</param>
		/// <param name="serializationSettings">Optional serialization settings</param>
		/// <returns>Byte array</returns>
		public static byte[] Serialize(IDictionary<int, int> dictionary, SerializationSettings serializationSettings = null)
		{
			var valueTypes = dictionary.ToDictionary(pair => pair.Key, pair => typeof(int));
			// Generate Key and value pairs section
			var generateResult = GenerateDictionaryKeyValuePairs(dictionary, valueTypes, serializationSettings);

			return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.GetAUDALFtypeWithDotnetType(typeof(int)));
		}

		/// <summary>
		/// Serialize a string to string dictionary to AUDALF bytes
		/// </summary>
		/// <param name="dictionary">String to string Dictionary to serialize</param>
		/// <param name="serializationSettings">Optional serialization settings</param>
		/// <returns>Byte array</returns>
		public static byte[] Serialize(IDictionary<string, string> dictionary, SerializationSettings serializationSettings = null)
		{
			var valueTypes = dictionary.ToDictionary(pair => pair.Key, pair => typeof(string));
			// Generate Key and value pairs section
			var generateResult = GenerateDictionaryKeyValuePairs(dictionary, valueTypes, serializationSettings);

			return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.GetAUDALFtypeWithDotnetType(typeof(string)));
		}

		/// <summary>
		/// Serialize a string to object dictionary to AUDALF bytes
		/// </summary>
		/// <param name="dictionary">String to object dictionary to serialize</param>
		/// <param name="valueTypes">What kind of values does the dictionary have</param>
		/// <param name="serializationSettings">Optional serialization settings</param>
		/// <returns>Byte array</returns>
		public static byte[] Serialize(IDictionary<string, object> dictionary, IDictionary<string, Type> valueTypes = null, SerializationSettings serializationSettings = null)
		{
			// Generate Key and value pairs section
			var generateResult = GenerateDictionaryKeyValuePairs(dictionary, valueTypes, serializationSettings);

			return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.GetAUDALFtypeWithDotnetType(typeof(string)));
		}

		/// <summary>
		/// Serialize a string to byte[] dictionary to AUDALF bytes
		/// </summary>
		/// <param name="dictionary">String to byte[] dictionary to serialize</param>
		/// <param name="valueTypes">What kind of values does the dictionary have</param>
		/// <param name="serializationSettings">Optional serialization settings</param>
		/// <returns>Byte array</returns>
		public static byte[] Serialize(IDictionary<string, byte[]> dictionary, IDictionary<string, Type> valueTypes = null, SerializationSettings serializationSettings = null)
		{
			if (valueTypes == null)
			{
				valueTypes = dictionary.ToDictionary(pair => pair.Key, pair => typeof(byte[]));
			}

			// Generate Key and value pairs section
			var generateResult = GenerateDictionaryKeyValuePairs(dictionary, valueTypes, serializationSettings);

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

		private static (byte[] bytes, List<ulong> positions) GenerateDictionaryKeyValuePairs<T,V>(IDictionary<T, V> pairs, IDictionary<T, Type> valueTypes = null, SerializationSettings serializationSettings = null)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				// Use UTF-8 because it has best support in different environments
				using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8))
				{
					List<ulong> offsets = new List<ulong>();
					foreach (var pair in pairs)
					{
						Type typeOfValue = FigureOutTypeOfValue(pair.Key, pair.Value, valueTypes);

						if (typeOfValue == null)
						{
							throw new ArgumentNullException(ValueCannotBeNullWithoutKnownValueTypeError);
						}

						offsets.Add(WriteOneDictionaryKeyValuePair(writer, pair.Key, pair.Value, typeof(T), typeOfValue, serializationSettings));
					}
					return (stream.ToArray(), offsets);
				}
			}
		}

		private static Type FigureOutTypeOfValue<T>(T key, object value, IDictionary<T, Type> valueTypes = null)
		{
			// ValueTypes will override everything else
			if (valueTypes != null && valueTypes.ContainsKey(key))
			{
				return valueTypes[key];
			}
			else if (value != null)
			{
				return value.GetType();
			}

			// Not good, return null
			return null;
		}

		private static (byte[] bytes, List<ulong> positions) GenerateListKeyValuePairs(IEnumerable<object> values, Type originalType, SerializationSettings serializationSettings)
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
						offsets.Add(WriteOneListKeyValuePair(writer, index, o, originalType, serializationSettings));
						index++;
					}
					return (stream.ToArray(), offsets);
				}
			}
		}

		private static ulong WriteOneDictionaryKeyValuePair(BinaryWriter writer, object key, object value, Type keyType, Type valueType, SerializationSettings serializationSettings)
		{
			// Store current offset, because different types can take different amount of space
			ulong returnValue = (ulong)writer.BaseStream.Position;

			GenericWrite(writer, key, keyType, isKey: true, serializationSettings: serializationSettings);
			GenericWrite(writer, value, valueType, isKey: false, serializationSettings: serializationSettings);

			return returnValue;
		}

		private static ulong WriteOneListKeyValuePair(BinaryWriter writer, ulong index, object value, Type originalType, SerializationSettings serializationSettings)
		{
			// Store current offset, because different types can take different amount of space
			ulong returnValue = (ulong)writer.BaseStream.Position;

			// Write Index number which is always 8 bytes
			writer.Write(index);

			GenericWrite(writer, value, originalType, isKey: false, serializationSettings: serializationSettings);

			return returnValue;
		}

		private static void GenericWrite(BinaryWriter writer, Object variableToWrite, Type originalType, bool isKey, SerializationSettings serializationSettings)
		{
			if (variableToWrite == null)
			{
				if (isKey)
				{
					throw new ArgumentNullException(KeyCannotBeNullError);
				}

				// Write special null, this is always 16 bytes
				WriteSpecialNullType(writer, originalType);
				return;
			}

			if (typeof(byte) == originalType)
			{
				WriteByte(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(byte[]) == originalType)
			{
				WriteByteArray(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(ushort) == originalType)
			{
				WriteUShort(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(ushort[]) == originalType)
			{
				WriteUShortArray(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(uint) == originalType)
			{
				WriteUInt(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(uint[]) == originalType)
			{
				WriteUIntArray(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(ulong) == originalType)
			{
				WriteULong(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(ulong[]) == originalType)
			{
				WriteULongArray(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(sbyte) == originalType)
			{
				WriteSByte(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(short) == originalType)
			{
				WriteShort(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(short[]) == originalType)
			{
				WriteShortArray(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(int) == originalType)
			{
				WriteInt(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(int[]) == originalType)
			{
				WriteIntArray(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(long) == originalType)
			{
				WriteLong(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(long[]) == originalType)
			{
				WriteLongArray(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(float) == originalType)
			{
				WriteFloat(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(double) == originalType)
			{
				WriteDouble(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(string) == originalType)
			{
				WriteString(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(bool) == originalType)
			{
				WriteBoolean(writer, variableToWrite, originalType, isKey: isKey);
			}
			else if (typeof(DateTime) == originalType)
			{
				WriteDateTime(writer, variableToWrite, originalType, isKey: isKey, dateTimeFormat: serializationSettings != null ? serializationSettings.dateTimeFormat : default(DateTimeFormat));
			}
			else if (typeof(DateTimeOffset) == originalType)
			{
				WriteDateTimeOffset(writer, variableToWrite, originalType, isKey: isKey, dateTimeFormat: serializationSettings != null ? serializationSettings.dateTimeFormat : default(DateTimeFormat));
			}
			else if (typeof(BigInteger) == originalType)
			{
				WriteBigInteger(writer, variableToWrite, originalType, isKey: isKey);
			}
		}

		#region Single values

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

		private static void WriteUShort(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single ushort takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}
			
			// Write ushort as 2 bytes
			writer.Write((ushort)valueToWrite);
			// Write 6 bytes of padding
			PadWithZeros(writer, 6);
		}

		private static void WriteUInt(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single uint takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}
			
			// Write ushort as 4 bytes
			writer.Write((uint)valueToWrite);
			// Write 4 bytes of padding
			PadWithZeros(writer, 4);
		}

		private static void WriteULong(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single ulong takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}
			
			// Write ulong as 8 bytes
			writer.Write((ulong)valueToWrite);
			// No padding needed
		}

		private static void WriteSByte(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single sbyte takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}
			
			// Write sbyte as 1 byte
			writer.Write((sbyte)valueToWrite);
			// Write 7 bytes of padding
			PadWithZeros(writer, 7);
		}

		private static void WriteShort(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single short takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}
			
			// Write short as 2 bytes
			writer.Write((short)valueToWrite);
			// Write 6 bytes of padding
			PadWithZeros(writer, 6);
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

		private static void WriteLong(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single long takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}
			
			// Write ulong as 8 bytes
			writer.Write((long)valueToWrite);
			// No padding needed
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

		private static void WriteDouble(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single double takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}
			
			// Write double as 8 bytes
			writer.Write((double)valueToWrite);
			// No padding needed
		}

		private static void WriteString(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single string has variable length
			string stringToWrite = (string)valueToWrite;
	
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

		private static void WriteBoolean(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Single boolean takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}
			
			// Write boolean as 1 byte
			writer.Write((bool)valueToWrite);
			// Write 7 bytes of padding
			PadWithZeros(writer, 7);
		}

		private static void WriteDateTime(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey, DateTimeFormat dateTimeFormat)
		{
			// Single datetime might take either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given), or variable amount
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				switch (dateTimeFormat)
				{
					case DateTimeFormat.UnixInSeconds:
						writer.Write(Definitions.datetime_unix_seconds);
						break;

					case DateTimeFormat.UnixInMilliseconds:
						writer.Write(Definitions.datetime_unix_milliseconds);
						break;

					case DateTimeFormat.ISO8601:
						writer.Write(Definitions.datetime_iso_8601);
						break;

				}
			}

			DateTime dt = (DateTime)valueToWrite;

			switch (dateTimeFormat)
			{
				case DateTimeFormat.UnixInSeconds:				
					long unixTimeSeconds = new DateTimeOffset(dt, TimeZoneInfo.Utc.GetUtcOffset(dt)).ToUnixTimeSeconds();
					writer.Write(unixTimeSeconds);
					break;
				
				case DateTimeFormat.UnixInMilliseconds:
					long unixTimeMilliSeconds = new DateTimeOffset(dt, TimeZoneInfo.Utc.GetUtcOffset(dt)).ToUnixTimeMilliseconds();
					writer.Write(unixTimeMilliSeconds);
					break;

				case DateTimeFormat.ISO8601:
					string iso8601Time = dt.ToString("o", CultureInfo.InvariantCulture);
					// Use existing string writing for this
					WriteString(writer, iso8601Time, originalType, isKey: true);
					break;
			}
		}

		private static void WriteDateTimeOffset(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey, DateTimeFormat dateTimeFormat)
		{
			// Single datetimeoffset might take either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given), or variable amount
			if (!isKey)
			{
				// Write value type ID (8 bytes)
				switch (dateTimeFormat)
				{
					case DateTimeFormat.UnixInSeconds:
						writer.Write(Definitions.datetime_unix_seconds);
						break;

					case DateTimeFormat.UnixInMilliseconds:
						writer.Write(Definitions.datetime_unix_milliseconds);
						break;

					case DateTimeFormat.ISO8601:
						writer.Write(Definitions.datetime_iso_8601);
						break;

				}
			}

			DateTimeOffset dto = (DateTimeOffset)valueToWrite;

			switch (dateTimeFormat)
			{
				case DateTimeFormat.UnixInSeconds:
					long unixTimeSeconds = dto.ToUnixTimeSeconds();
					writer.Write(unixTimeSeconds);
					break;
				
				case DateTimeFormat.UnixInMilliseconds:
					long unixTimeMilliSeconds = dto.ToUnixTimeMilliseconds();
					writer.Write(unixTimeMilliSeconds);
					break;

				case DateTimeFormat.ISO8601:
					string iso8601Time = dto.ToString("o", CultureInfo.InvariantCulture);
					// Use existing string writing for this
					WriteString(writer, iso8601Time, originalType, isKey: true);
					break;
			}
		}

		private static void WriteBigInteger(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Big integer takes at least 9 bytes, most likely more
			byte[] arrayToWrite = ((BigInteger)valueToWrite).ToByteArray();

			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}		
			
			ulong countOfBytes = (ulong)arrayToWrite.LongLength;

			// Write how many bytes will follow as unsigned 64 bit integer
			writer.Write(countOfBytes);

			// Write actual bytes
			writer.Write(arrayToWrite);

			// Write needed amount of padding
			PadWithZeros(writer, Definitions.NextDivisableBy8(countOfBytes) - countOfBytes);
		}

		#endregion // Single values


		#region Arrays

		private static void WriteByteArray(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Byte array takes at least 8 bytes, most likely more
			byte[] arrayToWrite = (byte[])valueToWrite;

			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}		
			
			ulong countOfBytes = (ulong)arrayToWrite.LongLength;

			// Write how many bytes will follow as unsigned 64 bit integer
			writer.Write(countOfBytes);

			// Write actual bytes
			writer.Write(arrayToWrite);

			// Write needed amount of padding
			PadWithZeros(writer, Definitions.NextDivisableBy8(countOfBytes) - countOfBytes);
		}

		private static void WriteUShortArray(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Ushort array takes at least 8 bytes, most likely more
			ushort[] ushortArray = (ushort[])valueToWrite;
			byte[] arrayToWrite = new byte[ushortArray.Length * 2];
			Buffer.BlockCopy(ushortArray, 0, arrayToWrite, 0, arrayToWrite.Length);

			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}		
			
			ulong countOfBytes = (ulong)arrayToWrite.LongLength;

			// Write how many bytes will follow as unsigned 64 bit integer
			writer.Write(countOfBytes);

			// Write actual bytes
			writer.Write(arrayToWrite);

			// Write needed amount of padding
			PadWithZeros(writer, Definitions.NextDivisableBy8(countOfBytes) - countOfBytes);
		}

		private static void WriteUIntArray(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// UInt array takes at least 8 bytes, most likely more
			uint[] uintArray = (uint[])valueToWrite;
			byte[] arrayToWrite = new byte[uintArray.Length * 4];
			Buffer.BlockCopy(uintArray, 0, arrayToWrite, 0, arrayToWrite.Length);

			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}		
			
			ulong countOfBytes = (ulong)arrayToWrite.LongLength;

			// Write how many bytes will follow as unsigned 64 bit integer
			writer.Write(countOfBytes);

			// Write actual bytes
			writer.Write(arrayToWrite);

			// Write needed amount of padding
			PadWithZeros(writer, Definitions.NextDivisableBy8(countOfBytes) - countOfBytes);
		}

		private static void WriteULongArray(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// ULong array takes at least 8 bytes, most likely more
			ulong[] ulongArray = (ulong[])valueToWrite;
			byte[] arrayToWrite = new byte[ulongArray.Length * 8];
			Buffer.BlockCopy(ulongArray, 0, arrayToWrite, 0, arrayToWrite.Length);

			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}		
			
			ulong countOfBytes = (ulong)arrayToWrite.LongLength;

			// Write how many bytes will follow as unsigned 64 bit integer
			writer.Write(countOfBytes);

			// Write actual bytes
			writer.Write(arrayToWrite);

			// Write needed amount of padding
			PadWithZeros(writer, Definitions.NextDivisableBy8(countOfBytes) - countOfBytes);
		}

		private static void WriteShortArray(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Short array takes at least 8 bytes, most likely more
			short[] shortArray = (short[])valueToWrite;
			byte[] arrayToWrite = new byte[shortArray.Length * 2];
			Buffer.BlockCopy(shortArray, 0, arrayToWrite, 0, arrayToWrite.Length);

			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}		
			
			ulong countOfBytes = (ulong)arrayToWrite.LongLength;

			// Write how many bytes will follow as unsigned 64 bit integer
			writer.Write(countOfBytes);

			// Write actual bytes
			writer.Write(arrayToWrite);

			// Write needed amount of padding
			PadWithZeros(writer, Definitions.NextDivisableBy8(countOfBytes) - countOfBytes);
		}

		private static void WriteIntArray(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Int array takes at least 8 bytes, most likely more
			int[] intArray = (int[])valueToWrite;
			byte[] arrayToWrite = new byte[intArray.Length * 4];
			Buffer.BlockCopy(intArray, 0, arrayToWrite, 0, arrayToWrite.Length);

			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}		
			
			ulong countOfBytes = (ulong)arrayToWrite.LongLength;

			// Write how many bytes will follow as unsigned 64 bit integer
			writer.Write(countOfBytes);

			// Write actual bytes
			writer.Write(arrayToWrite);

			// Write needed amount of padding
			PadWithZeros(writer, Definitions.NextDivisableBy8(countOfBytes) - countOfBytes);
		}

		private static void WriteLongArray(BinaryWriter writer, Object valueToWrite, Type originalType, bool isKey)
		{
			// Long array takes at least 8 bytes, most likely more
			long[] longArray = (long[])valueToWrite;
			byte[] arrayToWrite = new byte[longArray.Length * 8];
			Buffer.BlockCopy(longArray, 0, arrayToWrite, 0, arrayToWrite.Length);

			if (!isKey)
			{
				// Write value type ID (8 bytes)
				writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
			}		
			
			ulong countOfBytes = (ulong)arrayToWrite.LongLength;

			// Write how many bytes will follow as unsigned 64 bit integer
			writer.Write(countOfBytes);

			// Write actual bytes
			writer.Write(arrayToWrite);

			// Write needed amount of padding
			PadWithZeros(writer, Definitions.NextDivisableBy8(countOfBytes) - countOfBytes);
		}

		#endregion // Arrays

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
