using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Numerics;
using System.Buffers.Binary;

namespace CSharp_AUDALF;

/// <summary>
/// AUDALF validation result
/// </summary>
public enum AUDALF_ValidationResult
{
	/// <summary>
	/// Ok
	/// </summary>
	Ok = 0,
	
	/// <summary>
	/// Wrong FourCC
	/// </summary>
	WrongFourCC,

	/// <summary>
	/// Version number too big
	/// </summary>
	VersionTooBig,

	/// <summary>
	/// Unknown key type
	/// </summary>
	UnknownKeyType,

	/// <summary>
	/// Unknown value type
	/// </summary>
	UnknownValueType
}

/// <summary>
/// Static class for deserializing AUDALF bytes into something more useful
/// </summary>
public static class AUDALF_Deserialize
{
	/// <summary>
	/// Deserialize AUDALF bytes to array
	/// </summary>
	/// <param name="payload">AUDALF bytes</param>
	/// <param name="doSafetyChecks">Do safety checks</param>
	/// <typeparam name="T">Type of array variables</typeparam>
	/// <returns>Array of variables</returns>
	public static T[] Deserialize<T>(ReadOnlySpan<byte> payload, bool doSafetyChecks = true)
	{
		return Deserialize<T>(new MemoryStream(payload.ToArray(), writable: false), doSafetyChecks);
	}

	/// <summary>
	/// Deserialize AUDALF bytes from stream to array
	/// </summary>
	/// <param name="inputStream">Input stream that contains bytes</param>
	/// <param name="doSafetyChecks">Do safety checks</param>
	/// <typeparam name="T">Type of array variables</typeparam>
	/// <returns>Array of variables</returns>
	public static T[] Deserialize<T>(Stream inputStream, bool doSafetyChecks = true)
	{
		ulong[] entryOffsets = GetEntryDefinitionOffsets(inputStream);
		T[] returnValues = new T[entryOffsets.Length];
		for (int i = 0; i < returnValues.Length; i++)
		{
			returnValues[i] = (T)ReadListKeyAndValueFromOffset(inputStream, entryOffsets[i], typeof(T)).value;
		}

		return returnValues;
	}

	/// <summary>
	/// Deserialize single array element from AUDALF bytes with given array index
	/// </summary>
	/// <param name="payload">AUDALF bytes</param>
	/// <param name="index">Zero based index to seek</param>
	/// <param name="doSafetyChecks">Do safety checks</param>
	/// <param name="settings">Optional Deserialization Settings</param>
	/// <typeparam name="T">Type of array variables</typeparam>
	/// <returns>Value of type T</returns>
	public static T DeserializeSingleElement<T>(ReadOnlySpan<byte> payload, ulong index, bool doSafetyChecks = true, DeserializationSettings settings = null)
	{
		return DeserializeSingleElement<T>(new MemoryStream(payload.ToArray(), writable: false), index, doSafetyChecks, settings);
	}

	/// <summary>
	/// Deserialize single array element from AUDALF stream with given array index
	/// </summary>
	/// <param name="inputStream">Input stream that contains bytes</param>
	/// <param name="index">Zero based index to seek</param>
	/// <param name="doSafetyChecks">Do safety checks</param>
	/// <param name="settings">Optional Deserialization Settings</param>
	/// <typeparam name="T">Type of array variables</typeparam>
	/// <returns>Value of type T</returns>
	public static T DeserializeSingleElement<T>(Stream inputStream, ulong index, bool doSafetyChecks = true, DeserializationSettings settings = null)
	{
		ulong[] entryOffsets = GetEntryDefinitionOffsets(inputStream);

		ulong wantedOffset = entryOffsets[index];

		(_, object value) = ReadListKeyAndValueFromOffset(inputStream, wantedOffset, typeof(T));

		return (T)value;
	}

	/// <summary>
	/// Deserialize AUDALF bytes to Dictionary
	/// </summary>
	/// <param name="payload">AUDALF bytes</param>
	/// <param name="doSafetyChecks">Do safety checks</param>
	/// <param name="settings">Optional Deserialization Settings</param>
	/// <typeparam name="T">Key type of Dictionary</typeparam>
	/// <typeparam name="V">Value type of Dictionary</typeparam>
	/// <returns>Dictionary</returns>
	public static Dictionary<T, V> Deserialize<T, V>(ReadOnlySpan<byte> payload, bool doSafetyChecks = true, DeserializationSettings settings = null)
	{
		return Deserialize<T,V>(new MemoryStream(payload.ToArray(), writable: false), doSafetyChecks, settings);
	}

	/// <summary>
	/// Deserialize AUDALF bytes from stream to Dictionary
	/// </summary>
	/// <param name="inputStream">Input stream that contains bytes</param>
	/// <param name="doSafetyChecks">Do safety checks</param>
	/// <param name="settings">Optional Deserialization Settings</param>
	/// <typeparam name="T">Key type of Dictionary</typeparam>
	/// <typeparam name="V">Value type of Dictionary</typeparam>
	/// <returns>Dictionary</returns>
	public static Dictionary<T, V> Deserialize<T, V>(Stream inputStream, bool doSafetyChecks = true, DeserializationSettings settings = null)
	{
		ReadOnlySpan<byte> typeIdOfKeys = ReadKeyType(inputStream);

		ulong[] entryOffsets = GetEntryDefinitionOffsets(inputStream);
		Dictionary<T, V> returnDictionary = new Dictionary<T, V>(entryOffsets.Length);
		for (int i = 0; i < entryOffsets.Length; i++)
		{
			(object key, object value) = ReadDictionaryKeyAndValueFromOffset(inputStream, entryOffsets[i], typeIdOfKeys, typeof(T), typeof(V), settings);
			returnDictionary.Add((T)key, (V)value);
		}

		return returnDictionary;
	}

	/// <summary>
	/// Deserialize single value from AUDALF bytes with given dictionary key
	/// </summary>
	/// <param name="payload">AUDALF bytes</param>
	/// <param name="keyToSeek">Key to seek</param>
	/// <param name="doSafetyChecks">Do safety checks</param>
	/// <param name="settings">Optional Deserialization Settings</param>
	/// <typeparam name="T">Key type of Dictionary</typeparam>
	/// <typeparam name="V">Value type of Dictionary</typeparam>
	/// <returns>Value of type V</returns>
	public static V DeserializeSingleValue<T, V>(ReadOnlySpan<byte> payload, T keyToSeek, bool doSafetyChecks = true, DeserializationSettings settings = null)
	{
		return DeserializeSingleValue<T, V>(new MemoryStream(payload.ToArray(), writable: false), keyToSeek, doSafetyChecks, settings);
	}

	/// <summary>
	/// Deserialize single value from AUDALF stream with given dictionary key
	/// </summary>
	/// <param name="inputStream">Input stream that contains bytes</param>
	/// <param name="keyToSeek">Key to seek</param>
	/// <param name="doSafetyChecks">Do safety checks</param>
	/// <param name="settings">Optional Deserialization Settings</param>
	/// <typeparam name="T">Key type of Dictionary</typeparam>
	/// <typeparam name="V">Value type of Dictionary</typeparam>
	/// <returns>Value of type V</returns>
	public static V DeserializeSingleValue<T, V>(Stream inputStream, T keyToSeek, bool doSafetyChecks = true, DeserializationSettings settings = null)
	{
		ReadOnlySpan<byte> typeIdOfKeys = ReadKeyType(inputStream);

		ulong[] entryOffsets = GetEntryDefinitionOffsets(inputStream);

		for (int i = 0; i < entryOffsets.Length; i++)
		{
			(object key, object value) = ReadDictionaryKeyAndValueFromOffset(inputStream, entryOffsets[i], typeIdOfKeys, typeof(T), typeof(V), settings);
			if (key.Equals(keyToSeek))
			{
				return (V)value;
			}
		}

		return default;
	}

	/// <summary>
	/// Is byte array a AUDALF one. Only checks FourCC
	/// </summary>
	/// <param name="payload">Byte array</param>
	/// <returns>True if it is; False otherwise</returns>
	public static bool IsAUDALF(ReadOnlySpan<byte> payload)
	{
		return Definitions.ByteArrayCompare(Definitions.fourCC.AsSpan(), payload.Slice(0, 4));
	}

	/// <summary>
	/// Does stream contain AUDALF byte array. Only checks FourCC
	/// </summary>
	/// <param name="inputStream">Input stream</param>
	/// <returns>True if it is; False otherwise</returns>
	public static bool IsAUDALF(Stream inputStream)
	{
		using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
		{
			byte[] fourCC = reader.ReadBytes(Definitions.fourCCSize);
			return Definitions.ByteArrayCompare(Definitions.fourCC.AsSpan(), fourCC);
		}
	}

	/// <summary>
	/// Get AUDALF version number from AUDALF byte array
	/// </summary>
	/// <param name="payload">AUDALF byte array</param>
	/// <returns>Uint that contains version number</returns>
	public static uint GetVersionNumber(ReadOnlySpan<byte> payload)
	{
		return BinaryPrimitives.ReadUInt32LittleEndian(payload.Slice(Definitions.versionOffset, 4));
	}

	/// <summary>
	/// Get AUDALF version number from stream
	/// </summary>
	/// <param name="inputStream">Input stream</param>
	/// <returns>Uint that contains version number</returns>
	public static uint GetVersionNumber(Stream inputStream)
	{
		using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
		{
			reader.BaseStream.Seek(Definitions.versionOffset, SeekOrigin.Begin);
			return reader.ReadUInt32();
		}
	}

	/// <summary>
	/// Get the header stored payload size in bytes
	/// </summary>
	/// <param name="payload">AUDALF byte array</param>
	/// <returns>Ulong</returns>
	public static ulong GetByteSize(ReadOnlySpan<byte> payload)
	{
		return BinaryPrimitives.ReadUInt64LittleEndian(payload.Slice(Definitions.payloadSizeOffset, 8));
	}

	/// <summary>
	/// Get the header stored payload size in bytes from input stream
	/// </summary>
	/// <param name="inputStream">Input stream</param>
	/// <returns>Ulong</returns>
	public static ulong GetByteSize(Stream inputStream)
	{
		using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
		{
			reader.BaseStream.Seek(Definitions.payloadSizeOffset, SeekOrigin.Begin);
			return reader.ReadUInt64();
		}
	}

	/// <summary>
	/// Is AUDALF byte array a dictionary (or a list)
	/// </summary>
	/// <param name="payload">AUDALF byte array</param>
	/// <returns>True if Dictionary; False if list</returns>
	public static bool IsDictionary(ReadOnlySpan<byte> payload)
	{
		return !Definitions.ByteArrayCompare(Definitions.specialType.AsSpan(), payload.Slice(Definitions.keyTypeOffset, 8));
	}

	/// <summary>
	/// Does AUDALF input stream contain a dictionary (or a list)
	/// </summary>
	/// <param name="inputStream">Input stream</param>
	/// <returns>True if Dictionary; False if list</returns>
	public static bool IsDictionary(Stream inputStream)
	{
		using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
		{
			reader.BaseStream.Seek(Definitions.keyTypeOffset, SeekOrigin.Begin);
			byte[] keyType = reader.ReadBytes(8);
			return !Definitions.ByteArrayCompare(Definitions.specialType.AsSpan(), keyType);
		}
	}

	/// <summary>
	/// Read a key type from AUDALF byte array
	/// </summary>
	/// <param name="payload">AUDALF byte array</param>
	/// <returns>Byte array that contains AUDALF type ID</returns>
	public static ReadOnlySpan<byte> ReadKeyType(ReadOnlySpan<byte> payload)
	{
		return payload.Slice(Definitions.keyTypeOffset, 8);
	}

	/// <summary>
	/// Read a key type from AUDALF input stream
	/// </summary>
	/// <param name="inputStream">Input stream</param>
	/// <returns>Byte array that contains AUDALF type ID</returns>
	public static ReadOnlySpan<byte> ReadKeyType(Stream inputStream)
	{
		using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
		{
			reader.BaseStream.Seek(Definitions.keyTypeOffset, SeekOrigin.Begin);
			return reader.ReadBytes(8);
		}
	}

	/// <summary>
	/// Parse a key type from AUDALF byte array
	/// </summary>
	/// <param name="payload">AUDALF byte array</param>
	/// <returns>Type</returns>
	public static Type ParseKeyType(ReadOnlySpan<byte> payload)
	{
		return Definitions.GetDotnetTypeWithAUDALFtype(payload.Slice(Definitions.keyTypeOffset, 8));
	}

	/// <summary>
	/// Parse a key type from AUDALF input stream
	/// </summary>
	/// <param name="inputStream">Input stream</param>
	/// <returns>Type</returns>
	public static Type ParseKeyType(Stream inputStream)
	{
		using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
		{
			reader.BaseStream.Seek(Definitions.keyTypeOffset, SeekOrigin.Begin);
			byte[] keyType = reader.ReadBytes(8);
			return Definitions.GetDotnetTypeWithAUDALFtype(keyType);
		}
	}

	/// <summary>
	/// Get index count from AUDALF byte array
	/// </summary>
	/// <param name="payload">AUDALF byte array</param>
	/// <returns>Ulong count</returns>
	public static ulong GetIndexCount(ReadOnlySpan<byte> payload)
	{
		return BinaryPrimitives.ReadUInt64LittleEndian(payload.Slice(Definitions.indexCountOffset, 8));
	}

	/// <summary>
	/// Get index count from AUDALF input stream
	/// </summary>
	/// <param name="inputStream">Input stream</param>
	/// <returns>Ulong count</returns>
	public static ulong GetIndexCount(Stream inputStream)
	{
		using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
		{
			reader.BaseStream.Seek(Definitions.indexCountOffset, SeekOrigin.Begin);
			return reader.ReadUInt64();
		}
	}

	/// <summary>
	/// Get entry definition offsets from AUDALF byte array
	/// </summary>
	/// <param name="payload">AUDALF byte array</param>
	/// <returns>Array of ulong offsets</returns>
	public static ulong[] GetEntryDefinitionOffsets(ReadOnlySpan<byte> payload)
	{
		ulong indexCount = BinaryPrimitives.ReadUInt64LittleEndian(payload.Slice(Definitions.indexCountOffset, 8));

		ulong[] returnValues = new ulong[indexCount];
		for (ulong u = 0; u < indexCount; u++)
		{
			int offset = Definitions.entryDefinitionsOffset + (int)(u * 8);
			returnValues[u] = BinaryPrimitives.ReadUInt64LittleEndian(payload.Slice(offset, 8));
		}

		return returnValues;
	}

	/// <summary>
	/// Get entry definition offsets from AUDALF input stream
	/// </summary>
	/// <param name="inputStream">Input stream</param>
	/// <returns>Array of ulong offsets</returns>
	public static ulong[] GetEntryDefinitionOffsets(Stream inputStream)
	{
		using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
		{
			reader.BaseStream.Seek(Definitions.indexCountOffset, SeekOrigin.Begin);
			ulong indexCount = reader.ReadUInt64();

			reader.BaseStream.Seek(Definitions.entryDefinitionsOffset, SeekOrigin.Begin);

			ulong[] returnValues = new ulong[indexCount];
			for (ulong u = 0; u < indexCount; u++)
			{
				returnValues[u] = reader.ReadUInt64();
			}

			return returnValues;
		}
	}

	/// <summary>
	/// Read list key and value from offset. Key means index.
	/// </summary>
	/// <param name="payload">Byte array</param>
	/// <param name="offset">Offset bytes</param>
	/// <param name="wantedType">Wanted type for object (in case there are multiple options for deserialization)</param>
	/// <returns>Tuple that has key index and object for value</returns>
	public static (ulong key, object value) ReadListKeyAndValueFromOffset(ReadOnlySpan<byte> payload, ulong offset, Type wantedType)
	{
		ulong key = BinaryPrimitives.ReadUInt64LittleEndian(payload.Slice((int)offset, 8));
		ReadOnlySpan<byte> typeIdAsBytes = payload.Slice(8, 8);
		object value = Read(payload.Slice(16), typeIdAsBytes, wantedType);
		return ReadListKeyAndValueFromOffset(new MemoryStream(payload.ToArray(), writable: false), offset, wantedType);
	}

	/// <summary>
	/// Read list key and value from offset. Key means index.
	/// </summary>
	/// <param name="inputStream">Input stream</param>
	/// <param name="offset">Offset bytes</param>
	/// <param name="wantedType">Wanted type for object (in case there are multiple options for deserialization)</param>
	/// <returns>Tuple that has key index and object for value</returns>
	public static (ulong key, object value) ReadListKeyAndValueFromOffset(Stream inputStream, ulong offset, Type wantedType)
	{
		using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
		{
			reader.BaseStream.Seek((long)offset, SeekOrigin.Begin);
			ulong key = reader.ReadUInt64();
			byte[] typeIdAsBytes = reader.ReadBytes(8);

			object value = Read(reader, typeIdAsBytes, wantedType);

			return (key, value);
		}
	}

	/// <summary>
	/// Read dictionary key and value from offset.
	/// </summary>
	/// <param name="inputStream">Input stream</param>
	/// <param name="offset">Offset bytes</param>
	/// <param name="typeIdOfKeyAsBytes">AUDALF type Id of key</param>
	/// <param name="keyType">Wanted type of key</param>
	/// <param name="valueType">Wanted type of value</param>
	/// <param name="settings">Optional deserialization settings</param>
	/// <returns>Tuple that has key object and value object associated to it</returns>
	public static (object key, object value) ReadDictionaryKeyAndValueFromOffset(Stream inputStream, ulong offset, ReadOnlySpan<byte> typeIdOfKeyAsBytes, Type keyType, Type valueType, DeserializationSettings settings = null)
	{
		using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
		{
			reader.BaseStream.Seek((long)offset, SeekOrigin.Begin);
			object key = Read(reader, typeIdOfKeyAsBytes, keyType);

			// Since key might not end in 8 byte alignment, move to it
			long nextValid8ByteBlock = (long)Definitions.NextDivisableBy8((ulong)reader.BaseStream.Position);
			reader.BaseStream.Seek(nextValid8ByteBlock, SeekOrigin.Begin);

			object value = null;
			byte[] typeIdOfValueAsBytes = reader.ReadBytes(8);

			value = Read(reader, typeIdOfValueAsBytes, valueType, settings);

			return (key, value);
		}
	}

	private static object Read(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_8_bit_integerType.AsSpan()))
		{
			return bytesToProcess[0];
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_8_bit_integerArrayType.AsSpan()))
		{
			ulong byteArrayLengthInBytes = BinaryPrimitives.ReadUInt64LittleEndian(bytesToProcess);
			return bytesToProcess.Slice(8, (int)byteArrayLengthInBytes).ToArray();
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_16_bit_integerType.AsSpan()))
		{
			return BinaryPrimitives.ReadUInt16LittleEndian(bytesToProcess);
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_32_bit_integerType.AsSpan()))
		{
			return BinaryPrimitives.ReadUInt32LittleEndian(bytesToProcess);
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_64_bit_integerType.AsSpan()))
		{
			return BinaryPrimitives.ReadUInt64LittleEndian(bytesToProcess);
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_16_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<ushort>(bytesToProcess, sizeof(ushort));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_32_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<uint>(bytesToProcess, sizeof(uint));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_64_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<ulong>(bytesToProcess, sizeof(ulong));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_8_bit_integerType.AsSpan()))
		{
			return (sbyte)bytesToProcess[0];
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_8_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<sbyte>(bytesToProcess, sizeof(sbyte));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_16_bit_integerType.AsSpan()))
		{
			return BinaryPrimitives.ReadInt16LittleEndian(bytesToProcess);
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_16_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<short>(bytesToProcess, sizeof(short));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_32_bit_integerType.AsSpan()))
		{
			return BinaryPrimitives.ReadInt32LittleEndian(bytesToProcess);
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_32_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<int>(bytesToProcess, sizeof(int));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_64_bit_integerType.AsSpan()))
		{
			return BinaryPrimitives.ReadInt64LittleEndian(bytesToProcess);
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_64_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<long>(bytesToProcess, sizeof(long));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.floating_point_32_bit.AsSpan()))
		{
			return BinaryPrimitives.ReadSingleLittleEndian(bytesToProcess);
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.floating_point_32_bitArrayType.AsSpan()))
		{
			return ReadArray<float>(bytesToProcess, sizeof(float));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.floating_point_64_bit.AsSpan()))
		{
			return BinaryPrimitives.ReadDoubleLittleEndian(bytesToProcess);
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.string_utf8.AsSpan()))
		{
			ulong stringLengthInBytes = BinaryPrimitives.ReadUInt64LittleEndian(bytesToProcess);;
			return Encoding.UTF8.GetString(bytesToProcess.Slice(8, (int)stringLengthInBytes));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.booleans.AsSpan()))
		{
			return bytesToProcess[0] != 0;
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.datetime_unix_seconds.AsSpan()))
		{
			long timeStamp = BinaryPrimitives.ReadInt64LittleEndian(bytesToProcess);
			DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timeStamp);

			if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
			{
				return dateTimeOffset;
			}
			
			return dateTimeOffset.UtcDateTime;// .DateTime;
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.datetime_unix_milliseconds.AsSpan()))
		{
			long timeStamp = BinaryPrimitives.ReadInt64LittleEndian(bytesToProcess);
			DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timeStamp);

			if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
			{
				return dateTimeOffset;
			}

			return dateTimeOffset.UtcDateTime;// .DateTime;
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.datetime_iso_8601.AsSpan()))
		{
			ulong stringLengthInBytes = BinaryPrimitives.ReadUInt64LittleEndian(bytesToProcess);
			string iso8601 = Encoding.UTF8.GetString(bytesToProcess.Slice(8, (int)stringLengthInBytes));

			if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
			{
				return DateTimeOffset.Parse(iso8601, null, DateTimeStyles.RoundtripKind);
			}

			return DateTime.Parse(iso8601, null, DateTimeStyles.RoundtripKind);
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.bigIntegerType.AsSpan()))
		{
			ulong bigIntegerLengthInBytes = BinaryPrimitives.ReadUInt64LittleEndian(bytesToProcess);
			return new BigInteger(bytesToProcess.Slice(8, (int)bigIntegerLengthInBytes));
		}

		return null;
	}

	private static object Read(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_8_bit_integerType.AsSpan()))
		{
			return reader.ReadByte();
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_8_bit_integerArrayType.AsSpan()))
		{
			ulong byteArrayLengthInBytes = reader.ReadUInt64();
			return reader.ReadBytes((int)byteArrayLengthInBytes);
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_16_bit_integerType.AsSpan()))
		{
			return reader.ReadUInt16();
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_32_bit_integerType.AsSpan()))
		{
			return reader.ReadUInt32();
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_64_bit_integerType.AsSpan()))
		{
			return reader.ReadUInt64();
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_16_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<ushort>(reader, sizeof(ushort));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_32_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<uint>(reader, sizeof(uint));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_64_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<ulong>(reader, sizeof(ulong));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_8_bit_integerType.AsSpan()))
		{
			return reader.ReadSByte();
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_8_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<sbyte>(reader, sizeof(sbyte));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_16_bit_integerType.AsSpan()))
		{
			return reader.ReadInt16();
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_16_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<short>(reader, sizeof(short));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_32_bit_integerType.AsSpan()))
		{
			return reader.ReadInt32();
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_32_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<int>(reader, sizeof(int));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_64_bit_integerType.AsSpan()))
		{
			return reader.ReadInt64();
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_64_bit_integerArrayType.AsSpan()))
		{
			return ReadArray<long>(reader, sizeof(long));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.floating_point_32_bit.AsSpan()))
		{
			return reader.ReadSingle();
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.floating_point_32_bitArrayType.AsSpan()))
		{
			return ReadArray<float>(reader, sizeof(float));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.floating_point_64_bit.AsSpan()))
		{
			return reader.ReadDouble();
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.string_utf8.AsSpan()))
		{
			ulong stringLengthInBytes = reader.ReadUInt64();
			return Encoding.UTF8.GetString(reader.ReadBytes((int)stringLengthInBytes));
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.booleans.AsSpan()))
		{
			return reader.ReadBoolean();
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.datetime_unix_seconds.AsSpan()))
		{
			long timeStamp = reader.ReadInt64();
			DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timeStamp);

			if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
			{
				return dateTimeOffset;
			}
			
			return dateTimeOffset.UtcDateTime;// .DateTime;
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.datetime_unix_milliseconds.AsSpan()))
		{
			long timeStamp = reader.ReadInt64();
			DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timeStamp);

			if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
			{
				return dateTimeOffset;
			}

			return dateTimeOffset.UtcDateTime;// .DateTime;
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.datetime_iso_8601.AsSpan()))
		{
			ulong stringLengthInBytes = reader.ReadUInt64();
			string iso8601 = Encoding.UTF8.GetString(reader.ReadBytes((int)stringLengthInBytes));

			if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
			{
				return DateTimeOffset.Parse(iso8601, null, DateTimeStyles.RoundtripKind);
			}

			return DateTime.Parse(iso8601, null, DateTimeStyles.RoundtripKind);
		}
		else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.bigIntegerType.AsSpan()))
		{
			ulong bigIntegerLengthInBytes = reader.ReadUInt64();
			byte[] tempBytes = reader.ReadBytes((int)bigIntegerLengthInBytes);
			return new BigInteger(tempBytes);
		}

		return null;
	}

	private static T[] ReadArray<T>(BinaryReader reader, ulong bytesPerItem)
	{
		ulong byteArrayLengthInBytes = reader.ReadUInt64();
		byte[] bytes = reader.ReadBytes((int)byteArrayLengthInBytes);
		T[] returnArray = new T[byteArrayLengthInBytes / bytesPerItem];
		Buffer.BlockCopy(bytes, 0, returnArray, 0, (int)byteArrayLengthInBytes);
		return returnArray;
	}

	private static T[] ReadArray<T>(ReadOnlySpan<byte> bytesToProcess, ulong bytesPerItem)
	{
		ulong byteArrayLengthInBytes = BinaryPrimitives.ReadUInt64LittleEndian(bytesToProcess);
		ReadOnlySpan<byte> source = bytesToProcess.Slice(8, (int)byteArrayLengthInBytes);
		T[] returnArray = new T[byteArrayLengthInBytes / bytesPerItem];
		Buffer.BlockCopy(source.ToArray(), 0, returnArray, 0, (int)byteArrayLengthInBytes);
		return returnArray;
	}
}
