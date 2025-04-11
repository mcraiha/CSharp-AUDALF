using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Frozen;
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

delegate object ReadFromStream(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null);

delegate object ReadFromReadOnlySpan(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null);

sealed class DeserializeDefinition
{
	public readonly Type type;
	public readonly ReadFromStream readFromStream;
	public readonly ReadFromReadOnlySpan readFromReadOnlySpan;
	public readonly bool isConstantSized;
	public readonly int constantSizeInBytes;

	public DeserializeDefinition(Type dotnetType, ReadFromStream streamRead, ReadFromReadOnlySpan readOnlySpanRead)
	{
		type = dotnetType;

		readFromStream = streamRead;
		readFromReadOnlySpan = readOnlySpanRead;

		isConstantSized = false;
		constantSizeInBytes = -1;
	}

	public DeserializeDefinition(Type dotnetType, ReadFromStream streamRead, ReadFromReadOnlySpan readOnlySpanRead, int sizeInBytes)
	{
		type = dotnetType;

		readFromStream = streamRead;
		readFromReadOnlySpan = readOnlySpanRead;

		isConstantSized = true;
		constantSizeInBytes = sizeInBytes;
	}
}

/// <summary>
/// Static class for deserializing AUDALF bytes into something more useful
/// </summary>
public static class AUDALF_Deserialize
{
	private static readonly FrozenDictionary<long, DeserializeDefinition> definitions = new Dictionary<long, DeserializeDefinition>()
	{
		// Null values
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.specialType.AsSpan()),
			new DeserializeDefinition(typeof(object), ReadNull, ReadNull)
		},

		// Single values
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.unsigned_8_bit_integerType.AsSpan()),
			new DeserializeDefinition(typeof(byte), ReadByte, ReadByte, sizeof(byte))
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.unsigned_16_bit_integerType.AsSpan()),
			new DeserializeDefinition(typeof(ushort), ReadUShort, ReadUShort, sizeof(ushort))
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.unsigned_32_bit_integerType.AsSpan()),
			new DeserializeDefinition(typeof(uint), ReadUInt, ReadUInt, sizeof(uint))
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.unsigned_64_bit_integerType.AsSpan()),
			new DeserializeDefinition(typeof(ulong), ReadULong, ReadULong, sizeof(ulong))
		},

		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.signed_8_bit_integerType.AsSpan()),
			new DeserializeDefinition(typeof(sbyte), ReadSByte, ReadSByte, sizeof(sbyte))
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.signed_16_bit_integerType.AsSpan()),
			new DeserializeDefinition(typeof(short), ReadShort, ReadShort, sizeof(short))
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.signed_32_bit_integerType.AsSpan()),
			new DeserializeDefinition(typeof(int), ReadInt, ReadInt, sizeof(int))
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.signed_64_bit_integerType.AsSpan()),
			new DeserializeDefinition(typeof(long), ReadLong, ReadLong, sizeof(long))
		},

		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.floating_point_32_bit.AsSpan()),
			new DeserializeDefinition(typeof(float), ReadSingle, ReadSingle, sizeof(float))
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.floating_point_64_bit.AsSpan()),
			new DeserializeDefinition(typeof(double), ReadDouble, ReadDouble, sizeof(double))
		},

		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.string_utf8.AsSpan()),
			new DeserializeDefinition(typeof(string), ReadUTF8String, ReadUTF8String)
		},

		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.booleans.AsSpan()),
			new DeserializeDefinition(typeof(bool), ReadBool, ReadBool, sizeof(bool))
		},

		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.datetime_unix_seconds.AsSpan()),
			new DeserializeDefinition(typeof(DateTimeOffset), ReadUnixSeconds, ReadUnixSeconds, sizeof(long))
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.datetime_unix_milliseconds.AsSpan()),
			new DeserializeDefinition(typeof(DateTimeOffset), ReadUnixMilliSeconds, ReadUnixMilliSeconds, sizeof(long))
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.datetime_iso_8601.AsSpan()),
			new DeserializeDefinition(typeof(DateTimeOffset), ReadISO8601Timestamp, ReadISO8601Timestamp)
		},

		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.bigIntegerType.AsSpan()),
			new DeserializeDefinition(typeof(BigInteger), ReadBigInteger, ReadBigInteger)
		},


		// Array values
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.unsigned_8_bit_integerArrayType.AsSpan()),
			new DeserializeDefinition(typeof(byte[]), ReadByteArray, ReadByteArray)
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.unsigned_16_bit_integerArrayType.AsSpan()),
			new DeserializeDefinition(typeof(ushort[]), ReadUShortArray, ReadUShortArray)
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.unsigned_32_bit_integerArrayType.AsSpan()),
			new DeserializeDefinition(typeof(uint[]), ReadUIntArray, ReadUIntArray)
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.unsigned_64_bit_integerArrayType.AsSpan()),
			new DeserializeDefinition(typeof(ulong[]), ReadULongArray, ReadULongArray)
		},

		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.signed_8_bit_integerArrayType.AsSpan()),
			new DeserializeDefinition(typeof(sbyte[]), ReadSByteArray, ReadSByteArray)
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.signed_16_bit_integerArrayType.AsSpan()),
			new DeserializeDefinition(typeof(short[]), ReadShortArray, ReadShortArray)
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.signed_32_bit_integerArrayType.AsSpan()),
			new DeserializeDefinition(typeof(int[]), ReadIntArray, ReadIntArray)
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.signed_64_bit_integerArrayType.AsSpan()),
			new DeserializeDefinition(typeof(long[]), ReadLongArray, ReadLongArray)
		},

		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.floating_point_32_bitArrayType.AsSpan()),
			new DeserializeDefinition(typeof(float[]), ReadSingleArray, ReadSingleArray)
		},
		{ 
			BinaryPrimitives.ReadInt64LittleEndian(Definitions.floating_point_64_bitArrayType.AsSpan()),
			new DeserializeDefinition(typeof(double[]), ReadDoubleArray, ReadDoubleArray)
		},

	}.ToFrozenDictionary();

	/// <summary>
	/// Deserialize AUDALF bytes to array
	/// </summary>
	/// <param name="payload">AUDALF bytes</param>
	/// <param name="doSafetyChecks">Do safety checks</param>
	/// <typeparam name="T">Type of array variables</typeparam>
	/// <returns>Array of variables</returns>
	public static T[] Deserialize<T>(ReadOnlySpan<byte> payload, bool doSafetyChecks = true)
	{
		ulong[] entryOffsets = GetEntryDefinitionOffsets(payload);
		T[] returnValues = new T[entryOffsets.Length];
		for (int i = 0; i < returnValues.Length; i++)
		{
			returnValues[i] = (T)ReadListKeyAndValueFromOffset(payload, entryOffsets[i], typeof(T)).value;
		}

		return returnValues;
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
		ulong[] entryOffsets = GetEntryDefinitionOffsets(payload);

		ulong wantedOffset = entryOffsets[index];

		(_, object value) = ReadListKeyAndValueFromOffset(payload, wantedOffset, typeof(T));

		return (T)value;
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
		ReadOnlySpan<byte> typeIdOfKeys = ReadKeyType(payload);

		ulong[] entryOffsets = GetEntryDefinitionOffsets(payload);
		Dictionary<T, V> returnDictionary = new Dictionary<T, V>(entryOffsets.Length);
		for (int i = 0; i < entryOffsets.Length; i++)
		{
			(object key, object value) = ReadDictionaryKeyAndValueFromOffset(payload, entryOffsets[i], typeIdOfKeys, typeof(T), typeof(V), settings);
			returnDictionary.Add((T)key, (V)value);
		}

		return returnDictionary;
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
		ReadOnlySpan<byte> typeIdOfKeys = ReadKeyType(payload);

		ulong[] entryOffsets = GetEntryDefinitionOffsets(payload);

		for (int i = 0; i < entryOffsets.Length; i++)
		{
			(object key, object value) = ReadDictionaryKeyAndValueFromOffset(payload, entryOffsets[i], typeIdOfKeys, typeof(T), typeof(V), settings);
			if (key.Equals(keyToSeek))
			{
				return (V)value;
			}
		}

		return default;
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
		ReadOnlySpan<byte> typeIdAsBytes = payload.Slice((int)offset + 8, 8);
		object value = Read(payload.Slice((int)offset + 16), typeIdAsBytes, wantedType);
		return (key, value);
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

	private static int GetDictionaryKeySizeInBytes(ReadOnlySpan<byte> payload, ulong offset, ReadOnlySpan<byte> typeIdOfKeyAsBytes, Type keyType)
	{
		long key = BinaryPrimitives.ReadInt64LittleEndian(typeIdOfKeyAsBytes);
		if (definitions.TryGetValue(key, out var definition))
		{
			if (definition.isConstantSized)
			{
				return definition.constantSizeInBytes;
			}

			ulong byteArrayLengthInBytes = BinaryPrimitives.ReadUInt64LittleEndian(payload.Slice((int)offset));
			return sizeof(ulong) + (int)byteArrayLengthInBytes;
		}

		throw new NotImplementedException($"GetDictionaryKeySizeInBytes not implemented for {keyType}");
	}

	/// <summary>
	/// Read dictionary key and value from offset.
	/// </summary>
	/// <param name="payload">AUDALF byte array</param>
	/// <param name="offset">Offset bytes</param>
	/// <param name="typeIdOfKeyAsBytes">AUDALF type Id of key</param>
	/// <param name="keyType">Wanted type of key</param>
	/// <param name="valueType">Wanted type of value</param>
	/// <param name="settings">Optional deserialization settings</param>
	/// <returns>Tuple that has key object and value object associated to it</returns>
	public static (object key, object value) ReadDictionaryKeyAndValueFromOffset(ReadOnlySpan<byte> payload, ulong offset, ReadOnlySpan<byte> typeIdOfKeyAsBytes, Type keyType, Type valueType, DeserializationSettings settings = null)
	{
		object key = Read(payload.Slice((int)offset), typeIdOfKeyAsBytes, keyType);

		// Since key might not end in 8 byte alignment, move to it
		int keyLengthInBytes = GetDictionaryKeySizeInBytes(payload, offset, typeIdOfKeyAsBytes, keyType);
		ulong nextValid8ByteBlock = Definitions.NextDivisableBy8(offset + (ulong)keyLengthInBytes);

		ReadOnlySpan<byte> typeIdOfValueAsBytes = payload.Slice((int)nextValid8ByteBlock, 8);

		object value = Read(payload.Slice((int)nextValid8ByteBlock + 8), typeIdOfValueAsBytes, valueType, settings);

		return (key, value);
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

			byte[] typeIdOfValueAsBytes = reader.ReadBytes(8);

			object value = Read(reader, typeIdOfValueAsBytes, valueType, settings);

			return (key, value);
		}
	}


	internal static object ReadNull(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		long key = BinaryPrimitives.ReadInt64LittleEndian(bytesToProcess);

		if (definitions.TryGetValue(key, out var definition))
		{
			return Convert.ChangeType(null, definition.type);
		}

		throw new NotImplementedException($"Missing implementation");
	}

	internal static object ReadNull(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		long key = reader.ReadInt64();

		if (definitions.TryGetValue(key, out var definition))
		{
			return Convert.ChangeType(null, definition.type);
		}

		throw new NotImplementedException($"Missing implementation");
	}


	internal static object ReadByte(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return bytesToProcess[0];
	}

	internal static object ReadByte(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return reader.ReadByte();
	}

	internal static object ReadByteArray(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		ulong byteArrayLengthInBytes = BinaryPrimitives.ReadUInt64LittleEndian(bytesToProcess);
		return bytesToProcess.Slice(8, (int)byteArrayLengthInBytes).ToArray();
	}

	internal static object ReadByteArray(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		ulong byteArrayLengthInBytes = reader.ReadUInt64();
		return reader.ReadBytes((int)byteArrayLengthInBytes);
	}


	internal static object ReadUShort(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return BinaryPrimitives.ReadUInt16LittleEndian(bytesToProcess);
	}

	internal static object ReadUShort(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return reader.ReadUInt16();
	}

	internal static object ReadUShortArray(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<ushort>(bytesToProcess, sizeof(ushort));
	}

	internal static object ReadUShortArray(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<ushort>(reader, sizeof(ushort));
	}


	internal static object ReadUInt(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return BinaryPrimitives.ReadUInt32LittleEndian(bytesToProcess);
	}

	internal static object ReadUInt(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return reader.ReadUInt32();
	}

	internal static object ReadUIntArray(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<uint>(bytesToProcess, sizeof(uint));
	}

	internal static object ReadUIntArray(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<uint>(reader, sizeof(uint));
	}


	internal static object ReadULong(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return BinaryPrimitives.ReadUInt64LittleEndian(bytesToProcess);
	}

	internal static object ReadULong(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return reader.ReadUInt64();
	}

	internal static object ReadULongArray(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<ulong>(bytesToProcess, sizeof(ulong));
	}

	internal static object ReadULongArray(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<ulong>(reader, sizeof(ulong));
	}


	internal static object ReadSByte(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return (sbyte)bytesToProcess[0];
	}

	internal static object ReadSByte(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return reader.ReadSByte();
	}

	internal static object ReadSByteArray(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<sbyte>(bytesToProcess, sizeof(sbyte));
	}

	internal static object ReadSByteArray(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<sbyte>(reader, sizeof(sbyte));
	}


	internal static object ReadShort(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return BinaryPrimitives.ReadInt16LittleEndian(bytesToProcess);
	}

	internal static object ReadShort(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return reader.ReadInt16();
	}

	internal static object ReadShortArray(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<short>(bytesToProcess, sizeof(short));
	}

	internal static object ReadShortArray(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<short>(reader, sizeof(short));
	}


	internal static object ReadInt(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return BinaryPrimitives.ReadInt32LittleEndian(bytesToProcess);
	}

	internal static object ReadInt(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return reader.ReadInt32();
	}

	internal static object ReadIntArray(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<int>(bytesToProcess, sizeof(int));
	}

	internal static object ReadIntArray(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<int>(reader, sizeof(int));
	}


	internal static object ReadLong(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return BinaryPrimitives.ReadInt64LittleEndian(bytesToProcess);
	}

	internal static object ReadLong(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return reader.ReadInt64();
	}

	internal static object ReadLongArray(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<long>(bytesToProcess, sizeof(long));
	}

	internal static object ReadLongArray(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<long>(reader, sizeof(long));
	}


	internal static object ReadSingle(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return BinaryPrimitives.ReadSingleLittleEndian(bytesToProcess);
	}

	internal static object ReadSingle(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return reader.ReadSingle();
	}

	internal static object ReadSingleArray(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<float>(bytesToProcess, sizeof(float));
	}

	internal static object ReadSingleArray(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<float>(reader, sizeof(float));
	}


	internal static object ReadDouble(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return BinaryPrimitives.ReadDoubleLittleEndian(bytesToProcess);
	}

	internal static object ReadDouble(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return reader.ReadDouble();
	}

	internal static object ReadDoubleArray(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<double>(bytesToProcess, sizeof(double));
	}

	internal static object ReadDoubleArray(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return ReadArray<double>(reader, sizeof(double));
	}


	internal static object ReadUTF8String(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		ulong stringLengthInBytes = BinaryPrimitives.ReadUInt64LittleEndian(bytesToProcess);
		return Encoding.UTF8.GetString(bytesToProcess.Slice(8, (int)stringLengthInBytes));
	}

	internal static object ReadUTF8String(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		ulong stringLengthInBytes = reader.ReadUInt64();
		return Encoding.UTF8.GetString(reader.ReadBytes((int)stringLengthInBytes));
	}

	// TODO: String arrays

	internal static object ReadBool(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return bytesToProcess[0] != 0;
	}

	internal static object ReadBool(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		return reader.ReadBoolean();
	}

	// TODO: bool arrays

	internal static object ReadUnixSeconds(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		long timeStamp = BinaryPrimitives.ReadInt64LittleEndian(bytesToProcess);
		DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timeStamp);

		if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
		{
			return dateTimeOffset;
		}
		
		return dateTimeOffset.UtcDateTime;// .DateTime;
	}

	internal static object ReadUnixSeconds(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		long timeStamp = reader.ReadInt64();
		DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timeStamp);

		if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
		{
			return dateTimeOffset;
		}
		
		return dateTimeOffset.UtcDateTime;// .DateTime;
	}

	// TODO: unix seconds arrays

	internal static object ReadUnixMilliSeconds(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		long timeStamp = BinaryPrimitives.ReadInt64LittleEndian(bytesToProcess);
		DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timeStamp);

		if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
		{
			return dateTimeOffset;
		}

		return dateTimeOffset.UtcDateTime;// .DateTime;
	}

	internal static object ReadUnixMilliSeconds(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		long timeStamp = reader.ReadInt64();
		DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timeStamp);

		if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
		{
			return dateTimeOffset;
		}

		return dateTimeOffset.UtcDateTime;// .DateTime;
	}

	// TODO: unix milliseconds arrays

	internal static object ReadISO8601Timestamp(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		ulong stringLengthInBytes = BinaryPrimitives.ReadUInt64LittleEndian(bytesToProcess);
		string iso8601 = Encoding.UTF8.GetString(bytesToProcess.Slice(8, (int)stringLengthInBytes));

		if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
		{
			return DateTimeOffset.Parse(iso8601, null, DateTimeStyles.RoundtripKind);
		}

		return DateTime.Parse(iso8601, null, DateTimeStyles.RoundtripKind);
	}

	internal static object ReadISO8601Timestamp(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		ulong stringLengthInBytes = reader.ReadUInt64();
		string iso8601 = Encoding.UTF8.GetString(reader.ReadBytes((int)stringLengthInBytes));

		if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
		{
			return DateTimeOffset.Parse(iso8601, null, DateTimeStyles.RoundtripKind);
		}

		return DateTime.Parse(iso8601, null, DateTimeStyles.RoundtripKind);
	}

	// TODO: ISO 8601 timestamp arrays

	internal static object ReadBigInteger(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		ulong bigIntegerLengthInBytes = BinaryPrimitives.ReadUInt64LittleEndian(bytesToProcess);
		return new BigInteger(bytesToProcess.Slice(8, (int)bigIntegerLengthInBytes));
	}

	internal static object ReadBigInteger(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		ulong bigIntegerLengthInBytes = reader.ReadUInt64();
		byte[] tempBytes = reader.ReadBytes((int)bigIntegerLengthInBytes);
		return new BigInteger(tempBytes);
	}

	// TODO: big integer arrays

	private static object Read(ReadOnlySpan<byte> bytesToProcess, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		long key = BinaryPrimitives.ReadInt64LittleEndian(typeIdAsBytes);
		if (definitions.TryGetValue(key, out var definition))
		{
			return definition.readFromReadOnlySpan(bytesToProcess, typeIdAsBytes, wantedType, settings);
		}

		throw new NotImplementedException($"Missing implementation");
	}

	private static object Read(BinaryReader reader, ReadOnlySpan<byte> typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
	{
		long key = BinaryPrimitives.ReadInt64LittleEndian(typeIdAsBytes);
		if (definitions.TryGetValue(key, out var definition))
		{
			return definition.readFromStream(reader, typeIdAsBytes, wantedType, settings);
		}

		throw new NotImplementedException($"Missing implementation");
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
