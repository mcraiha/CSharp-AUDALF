using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.Frozen;
using System.Linq;
using System.Globalization;
using System.Numerics;

namespace CSharp_AUDALF;

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
	public static byte[] Serialize<T>(IEnumerable<T> ienumerableStructure, SerializationSettings? serializationSettings = null)
	{
		IEnumerable<object> objects = ienumerableStructure.Cast<object>();
		// Generate Key and value pairs section
		var generateResult = GenerateListKeyValuePairs(objects, typeof(T), serializationSettings);
		return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.specialType.AsSpan());
	}

	/// <summary>
	/// Serialize a IEnumerable structure of objects to AUDALF bytes
	/// </summary>
	/// <param name="ienumerableStructure">IEnumerable structure of objects</param>
	/// <param name="typeForNullValues">Type info for null values</param>
	/// <param name="serializationSettings">Optional serialization settings</param>
	/// <returns>Byte array</returns>
	public static byte[] Serialize(IEnumerable<object?> ienumerableStructure, Type? typeForNullValues = null, SerializationSettings? serializationSettings = null)
	{
		var generateResult = GenerateListKeyValuePairsWithPossibleNull(ienumerableStructure, typeForNullValues, serializationSettings);
		return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.specialType.AsSpan());
	}

	/// <summary>
	/// Serialize a byte to byte dictionary to AUDALF bytes
	/// </summary>
	/// <param name="dictionary">Byte to byte Dictionary to serialize</param>
	/// <param name="serializationSettings">Optional serialization settings</param>
	/// <returns>Byte array</returns>
	public static byte[] Serialize(IDictionary<byte, byte> dictionary, SerializationSettings? serializationSettings = null)
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
	public static byte[] Serialize(IDictionary<int, int> dictionary, SerializationSettings? serializationSettings = null)
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
	public static byte[] Serialize(IDictionary<string, string?> dictionary, SerializationSettings? serializationSettings = null)
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
	public static byte[] Serialize(IDictionary<string, object?> dictionary, IDictionary<string, Type>? valueTypes = null, SerializationSettings? serializationSettings = null)
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
	public static byte[] Serialize(IDictionary<string, byte[]?> dictionary, IDictionary<string, Type>? valueTypes = null, SerializationSettings? serializationSettings = null)
	{
		if (valueTypes == null)
		{
			valueTypes = dictionary.ToDictionary(pair => pair.Key, pair => typeof(byte[]));
		}

		// Generate Key and value pairs section
		var generateResult = GenerateDictionaryKeyValuePairs(dictionary, valueTypes, serializationSettings);

		return GenericSerialize(generateResult.bytes, generateResult.positions, Definitions.GetAUDALFtypeWithDotnetType(typeof(string)));
	}

	private static byte[] GenericSerialize(ReadOnlySpan<byte> keyValuePairsBytes, List<ulong> keyValuePairsOffsets, ReadOnlySpan<byte> keyTypeAsBytes)
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
		writer.Write(Definitions.fourCC.AsSpan());

		// Then version number
		writer.Write(Definitions.versionNumber.AsSpan());

		// Then some zeroes for payload size since this will be fixed later
		writer.Write(Definitions.payloadSizePlaceholder.AsSpan());
	}

	private static void WriteIndexSection(BinaryWriter writer, ReadOnlySpan<byte> keyTypeAsBytes, List<ulong> positions)
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

	private static (byte[] bytes, List<ulong> positions) GenerateDictionaryKeyValuePairs<T,V>(IDictionary<T, V> pairs, IDictionary<T, Type>? valueTypes = null, SerializationSettings? serializationSettings = null) where T : notnull
	{
		using (MemoryStream stream = new MemoryStream())
		{
			// Use UTF-8 because it has best support in different environments
			using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8))
			{
				List<ulong> offsets = new List<ulong>();
				foreach (var pair in pairs)
				{
					Type? typeOfValue = FigureOutTypeOfValue(pair.Key, pair.Value, valueTypes);
					//Console.WriteLine($"Type: {typeOfValue}");

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

	private static Type? FigureOutTypeOfValue<T>(T key, object? value, IDictionary<T, Type>? valueTypes = null)
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

	private static (byte[] bytes, List<ulong> positions) GenerateListKeyValuePairsWithPossibleNull(IEnumerable<object?> values, Type? typeForNulls, SerializationSettings? serializationSettings)
	{
		using (MemoryStream stream = new MemoryStream())
		{
			// Use UTF-8 because it has best support in different environments
			using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8))
			{
				List<ulong> offsets = new List<ulong>();
				ulong index = 0;
				foreach (object? o in values)
				{
					offsets.Add(WriteOneListKeyValuePair(writer, index, o, o != null ? o.GetType() : typeForNulls!, serializationSettings));
					index++;
				}
				return (stream.ToArray(), offsets);
			}
		}
	}

	private static (byte[] bytes, List<ulong> positions) GenerateListKeyValuePairs(IEnumerable<object> values, Type originalType, SerializationSettings? serializationSettings)
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

	private static ulong WriteOneDictionaryKeyValuePair(BinaryWriter writer, object key, object? value, Type keyType, Type valueType, SerializationSettings? serializationSettings)
	{
		// Store current offset, because different types can take different amount of space
		ulong returnValue = (ulong)writer.BaseStream.Position;

		GenericWrite(writer, key, keyType, isKey: true, serializationSettings: serializationSettings);
		GenericWrite(writer, value, valueType, isKey: false, serializationSettings: serializationSettings);

		return returnValue;
	}

	private static ulong WriteOneListKeyValuePair(BinaryWriter writer, ulong index, object? value, Type originalType, SerializationSettings? serializationSettings)
	{
		// Store current offset, because different types can take different amount of space
		ulong returnValue = (ulong)writer.BaseStream.Position;

		// Write Index number which is always 8 bytes
		writer.Write(index);

		GenericWrite(writer, value, originalType, isKey: false, serializationSettings: serializationSettings);

		return returnValue;
	}

	private static readonly FrozenSet<Type> needCustomValueTypeWriting = new HashSet<Type>()
	{
		typeof(DateTime),
		typeof(DateTimeOffset),
		typeof(object[]),
	}.ToFrozenSet();

	// Most types should use this
	private static void CommonValueTypeWriter(BinaryWriter writer, Type originalType)
	{
		// Write value type ID (8 bytes)
		writer.Write(Definitions.GetAUDALFtypeWithDotnetType(originalType));
	}

	private static void GenericWrite(BinaryWriter writer, object? variableToWrite, Type originalType, bool isKey, SerializationSettings? serializationSettings)
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

		// Write value type if needed and it is possible (some types have custom writers)
		if (!isKey)
		{
			if (needCustomValueTypeWriting.Contains(originalType))
			{
				// Special cases
			}
			else
			{
				// Common case
				CommonValueTypeWriter(writer, originalType);
			}
		}

		if (typeof(byte) == originalType)
		{
			WriteByte(writer, variableToWrite);
		}
		else if (typeof(byte[]) == originalType)
		{
			WriteArray<byte>(writer, variableToWrite, sizeof(byte));
		}
		else if (typeof(ushort) == originalType)
		{
			WriteUShort(writer, variableToWrite);
		}
		else if (typeof(ushort[]) == originalType)
		{
			WriteArray<ushort>(writer, variableToWrite, sizeof(ushort));
		}
		else if (typeof(uint) == originalType)
		{
			WriteUInt(writer, variableToWrite);
		}
		else if (typeof(uint[]) == originalType)
		{
			WriteArray<uint>(writer, variableToWrite, sizeof(uint));
		}
		else if (typeof(ulong) == originalType)
		{
			WriteULong(writer, variableToWrite);
		}
		else if (typeof(ulong[]) == originalType)
		{
			WriteArray<ulong>(writer, variableToWrite, sizeof(ulong));
		}
		else if (typeof(sbyte) == originalType)
		{
			WriteSByte(writer, variableToWrite);
		}
		else if (typeof(sbyte[]) == originalType)
		{
			WriteArray<sbyte>(writer, variableToWrite, sizeof(sbyte));
		}
		else if (typeof(short) == originalType)
		{
			WriteShort(writer, variableToWrite);
		}
		else if (typeof(short[]) == originalType)
		{
			WriteArray<short>(writer, variableToWrite, sizeof(short));
		}
		else if (typeof(int) == originalType)
		{
			WriteInt(writer, variableToWrite);
		}
		else if (typeof(int[]) == originalType)
		{
			WriteArray<int>(writer, variableToWrite, sizeof(int));
		}
		else if (typeof(long) == originalType)
		{
			WriteLong(writer, variableToWrite);
		}
		else if (typeof(long[]) == originalType)
		{
			WriteArray<long>(writer, variableToWrite, sizeof(long));
		}
		else if (typeof(float) == originalType)
		{
			WriteFloat(writer, variableToWrite);
		}
		else if (typeof(float[]) == originalType)
		{
			WriteArray<float>(writer, variableToWrite, sizeof(float));
		}
		else if (typeof(Half) == originalType)
		{
			WriteHalf(writer, variableToWrite);
		}
		else if (typeof(Half[]) == originalType)
		{
			WriteArray<Half>(writer, variableToWrite, 2 /* sizeof(Half) */);
		}
		else if (typeof(double) == originalType)
		{
			WriteDouble(writer, variableToWrite);
		}
		else if (typeof(double[]) == originalType)
		{
			WriteArray<double>(writer, variableToWrite, sizeof(double));
		}
		else if (typeof(string) == originalType)
		{
			WriteString(writer, variableToWrite);
		}
		else if (typeof(bool) == originalType)
		{
			WriteBoolean(writer, variableToWrite);
		}
		else if (typeof(DateTime) == originalType)
		{
			WriteDateTime(writer, variableToWrite, isKey: isKey, dateTimeFormat: serializationSettings != null ? serializationSettings.dateTimeFormat : default(DateTimeFormat));
		}
		else if (typeof(DateTimeOffset) == originalType)
		{
			WriteDateTimeOffset(writer, variableToWrite, isKey: isKey, dateTimeFormat: serializationSettings != null ? serializationSettings.dateTimeFormat : default(DateTimeFormat));
		}
		else if (typeof(BigInteger) == originalType)
		{
			WriteBigInteger(writer, variableToWrite);
		}
		else if (typeof(object[]) == originalType)
		{
			object[] objects = (object[])variableToWrite;
			MemoryStream ms = new MemoryStream();
			using (BinaryWriter tempWriter = new BinaryWriter(ms))
			{
				foreach (object o in objects)
				{
					GenericWrite(tempWriter, o, o.GetType(), isKey: false, serializationSettings);
				}
			}
			ArrayWriter(writer, ms.ToArray());
		}
	}

	#region Single values

	private static void WriteByte(BinaryWriter writer, object valueToWrite)
	{
		// Single byte takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
		
		// Write byte as 1 byte
		writer.Write((byte)valueToWrite);
		// Write 7 bytes of padding
		PadWithZeros(writer, 7);
	}

	private static void WriteUShort(BinaryWriter writer, object valueToWrite)
	{
		// Single ushort takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
		
		// Write ushort as 2 bytes
		writer.Write((ushort)valueToWrite);
		// Write 6 bytes of padding
		PadWithZeros(writer, 6);
	}

	private static void WriteUInt(BinaryWriter writer, object valueToWrite)
	{
		// Single uint takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
		
		// Write ushort as 4 bytes
		writer.Write((uint)valueToWrite);
		// Write 4 bytes of padding
		PadWithZeros(writer, 4);
	}

	private static void WriteULong(BinaryWriter writer, object valueToWrite)
	{
		// Single ulong takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
		
		// Write ulong as 8 bytes
		writer.Write((ulong)valueToWrite);
		// No padding needed
	}

	private static void WriteSByte(BinaryWriter writer, object valueToWrite)
	{
		// Single sbyte takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
		
		// Write sbyte as 1 byte
		writer.Write((sbyte)valueToWrite);
		// Write 7 bytes of padding
		PadWithZeros(writer, 7);
	}

	private static void WriteShort(BinaryWriter writer, object valueToWrite)
	{
		// Single short takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
		
		// Write short as 2 bytes
		writer.Write((short)valueToWrite);
		// Write 6 bytes of padding
		PadWithZeros(writer, 6);
	}

	private static void WriteInt(BinaryWriter writer, object valueToWrite)
	{
		// Single int takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
		
		// Write int as 4 bytes
		writer.Write((int)valueToWrite);
		// Write 4 bytes of padding
		PadWithZeros(writer, 4);
	}

	private static void WriteLong(BinaryWriter writer, object valueToWrite)
	{
		// Single long takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
		
		// Write ulong as 8 bytes
		writer.Write((long)valueToWrite);
		// No padding needed
	}
	
	private static void WriteHalf(BinaryWriter writer, object valueToWrite)
	{
		// Single half takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
		
		// Write half as 2 bytes
		writer.Write((Half)valueToWrite);
		// Write 6 bytes of padding
		PadWithZeros(writer, 6);
	}

	private static void WriteFloat(BinaryWriter writer, object valueToWrite)
	{
		// Single float takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)

		// Write float as 4 bytes
		writer.Write((float)valueToWrite);
		// Write 4 bytes of padding
		PadWithZeros(writer, 4);
	}

	private static void WriteDouble(BinaryWriter writer, object valueToWrite)
	{
		// Single double takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
		
		// Write double as 8 bytes
		writer.Write((double)valueToWrite);
		// No padding needed
	}

	private static void WriteString(BinaryWriter writer, object valueToWrite)
	{
		// Single string has variable length
		string stringToWrite = (string)valueToWrite;

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

	private static void WriteBoolean(BinaryWriter writer, object valueToWrite)
	{
		// Single boolean takes either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given)
		
		// Write boolean as 1 byte
		writer.Write((bool)valueToWrite);
		// Write 7 bytes of padding
		PadWithZeros(writer, 7);
	}

	private static void WriteDateTime(BinaryWriter writer, object valueToWrite, bool isKey, DateTimeFormat dateTimeFormat)
	{
		// Single datetime might take either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given), or variable amount
		if (!isKey)
		{
			// Write value type ID (8 bytes)
			switch (dateTimeFormat)
			{
				case DateTimeFormat.UnixInSeconds:
					writer.Write(Definitions.datetime_unix_seconds.AsSpan());
					break;

				case DateTimeFormat.UnixInMilliseconds:
					writer.Write(Definitions.datetime_unix_milliseconds.AsSpan());
					break;

				case DateTimeFormat.ISO8601:
					writer.Write(Definitions.datetime_iso_8601.AsSpan());
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
				WriteString(writer, iso8601Time);
				break;
		}
	}

	private static void WriteDateTimeOffset(BinaryWriter writer, object valueToWrite, bool isKey, DateTimeFormat dateTimeFormat)
	{
		// Single datetimeoffset might take either 8 bytes (as key since type ID is given earlier) or 16 bytes (as value since type ID must be given), or variable amount
		if (!isKey)
		{
			// Write value type ID (8 bytes)
			switch (dateTimeFormat)
			{
				case DateTimeFormat.UnixInSeconds:
					writer.Write(Definitions.datetime_unix_seconds.AsSpan());
					break;

				case DateTimeFormat.UnixInMilliseconds:
					writer.Write(Definitions.datetime_unix_milliseconds.AsSpan());
					break;

				case DateTimeFormat.ISO8601:
					writer.Write(Definitions.datetime_iso_8601.AsSpan());
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
				WriteString(writer, iso8601Time);
				break;
		}
	}

	private static void WriteBigInteger(BinaryWriter writer, object valueToWrite)
	{
		// Big integer takes at least 9 bytes, most likely more
		byte[] arrayToWrite = ((BigInteger)valueToWrite).ToByteArray();	
		
		ulong countOfBytes = (ulong)arrayToWrite.LongLength;

		// Write how many bytes will follow as unsigned 64 bit integer
		writer.Write(countOfBytes);

		// Write actual bytes
		writer.Write(arrayToWrite);

		// Write needed amount of padding
		PadWithZeros(writer, Definitions.NextDivisableBy8(countOfBytes) - countOfBytes);
	}

	#endregion // Single values

	private static void WriteArray<T>(BinaryWriter writer, object valueToWrite, int bytesPerItem)
	{
		if (typeof(T).IsPrimitive)
		{
			T[] array = (T[])valueToWrite;
			byte[] arrayToWrite = new byte[array.Length * bytesPerItem];
			Buffer.BlockCopy(array, 0, arrayToWrite, 0, arrayToWrite.Length);

			// Write actual array
			ArrayWriter(writer, arrayToWrite);
		}
		else if (typeof(T) == typeof(Half))
		{
			Half[] array = (Half[])valueToWrite;
			byte[] arrayToWrite = new byte[array.Length * bytesPerItem];
			for (int i = 0; i < array.Length; i++)
			{
				BitConverter.TryWriteBytes(new Span<byte>(arrayToWrite, i * 2, bytesPerItem), array[i]);
			}

			// Write actual array
			ArrayWriter(writer, arrayToWrite);
		}
		else
		{
			throw new NotImplementedException($"Support for {typeof(T)} array serialization is not yet implemented!");
		}
	}

	private static void ArrayWriter(BinaryWriter writer, byte[] arrayToWrite)
	{
		ulong countOfBytes = (ulong)arrayToWrite.LongLength;

		// Write how many bytes will follow as unsigned 64 bit integer
		writer.Write(countOfBytes);

		// Write actual bytes
		writer.Write(arrayToWrite);

		// Write needed amount of padding
		PadWithZeros(writer, Definitions.NextDivisableBy8(countOfBytes) - countOfBytes);
	}

	private static void WriteSpecialNullType(BinaryWriter writer, Type typeToWrite)
	{
		writer.Write(Definitions.specialType.AsSpan());
		writer.Write(Definitions.GetAUDALFtypeWithDotnetType(typeToWrite));
	}

	private const byte zeroByte = 0;
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
