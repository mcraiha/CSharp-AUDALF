using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Numerics;

namespace CSharp_AUDALF
{
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
		public static T[] Deserialize<T>(byte[] payload, bool doSafetyChecks = true)
		{
			return Deserialize<T>(new MemoryStream(payload, writable: false), doSafetyChecks);
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
		public static T DeserializeSingleElement<T>(byte[] payload, ulong index, bool doSafetyChecks = true, DeserializationSettings settings = null)
		{
			return DeserializeSingleElement<T>(new MemoryStream(payload, writable: false), index, doSafetyChecks, settings);
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
		public static Dictionary<T, V> Deserialize<T, V>(byte[] payload, bool doSafetyChecks = true, DeserializationSettings settings = null)
		{
			return Deserialize<T,V>(new MemoryStream(payload, writable: false), doSafetyChecks, settings);
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
			byte[] typeIdOfKeys = ReadKeyType(inputStream);

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
		public static V DeserializeSingleValue<T, V>(byte[] payload, T keyToSeek, bool doSafetyChecks = true, DeserializationSettings settings = null)
		{
			return DeserializeSingleValue<T, V>(new MemoryStream(payload, writable: false), keyToSeek, doSafetyChecks, settings);
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
			byte[] typeIdOfKeys = ReadKeyType(inputStream);

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
		public static bool IsAUDALF(byte[] payload)
		{
			return IsAUDALF(new MemoryStream(payload, writable: false));
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
				byte[] fourCC = reader.ReadBytes(4);
				return Definitions.ByteArrayCompare(Definitions.fourCC, fourCC);
			}
		}

		/// <summary>
		/// Get AUDALF version number from byte array
		/// </summary>
		/// <param name="payload">Byte array</param>
		/// <returns>Uint that contains version number</returns>
		public static uint GetVersionNumber(byte[] payload)
		{
			return GetVersionNumber(new MemoryStream(payload, writable: false));
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
		/// <param name="payload">Byte array</param>
		/// <returns>Ulong</returns>
		public static ulong GetByteSize(byte[] payload)
		{
			return GetByteSize(new MemoryStream(payload, writable: false));
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
		/// <param name="payload">Byte array</param>
		/// <returns>True if Dictionary; False if list</returns>
		public static bool IsDictionary(byte[] payload)
		{
			return IsDictionary(new MemoryStream(payload, writable: false));
		}

		/// <summary>
		/// Does AUDALF input stream contain a dictionary (or a list)
		/// </summary>
		/// <param name="inputStream">Input stream</param>
		/// <returns>True if Dictionary; False if list</returns>
		public static bool IsDictionary(Stream inputStream)
		{
			using (BinaryReader reader = new BinaryReader(inputStream))
			{
				reader.BaseStream.Seek(Definitions.keyTypeOffset, SeekOrigin.Begin);
				byte[] keyType = reader.ReadBytes(8);
				return !Definitions.ByteArrayCompare(Definitions.specialType, keyType);
			}
		}

		/// <summary>
		/// Read a key type from AUDALF byte array
		/// </summary>
		/// <param name="payload">Byte array</param>
		/// <returns>Byte array that contains AUDALF type ID</returns>
		public static byte[] ReadKeyType(byte[] payload)
		{
			return ReadKeyType(new MemoryStream(payload, writable: false));
		}

		/// <summary>
		/// Read a key type from AUDALF input stream
		/// </summary>
		/// <param name="inputStream">Input stream</param>
		/// <returns>Byte array that contains AUDALF type ID</returns>
		public static byte[] ReadKeyType(Stream inputStream)
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
		/// <param name="payload">Byte array</param>
		/// <returns>Type</returns>
		public static Type ParseKeyType(byte[] payload)
		{
			return ParseKeyType(new MemoryStream(payload, writable: false));
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
		/// <param name="payload">Byte array</param>
		/// <returns>Ulong count</returns>
		public static ulong GetIndexCount(byte[] payload)
		{
			return GetIndexCount(new MemoryStream(payload, writable: false));
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
		/// <param name="payload">Byte array</param>
		/// <returns>Array of ulong offsets</returns>
		public static ulong[] GetEntryDefinitionOffsets(byte[] payload)
		{
			return GetEntryDefinitionOffsets(new MemoryStream(payload));
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
		public static (ulong key, object value) ReadListKeyAndValueFromOffset(byte[] payload, ulong offset, Type wantedType)
		{
			return ReadListKeyAndValueFromOffset(new MemoryStream(payload, writable: false), offset, wantedType);
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
				object value = null;
				byte[] typeIdAsBytes = reader.ReadBytes(8);

				value = Read(reader, typeIdAsBytes, wantedType);

				return (key, value);
			}
		}

		/// <summary>
		/// Read dictionary key and value from offset.
		/// </summary>
		/// <param name="inputStream">Input stream</param>
		/// <param name="offset">Offset bytes</param>
		/// <param name="typeIdOfKeyAsBytes">AUDALF type Id of key as byte array</param>
		/// <param name="keyType">Wanted type of key</param>
		/// <param name="valueType">Wanted type of value</param>
		/// <param name="settings">Optional deserialization settings</param>
		/// <returns>Tuple that has key object and value object associated to it</returns>
		public static (object key, object value) ReadDictionaryKeyAndValueFromOffset(Stream inputStream, ulong offset, byte[] typeIdOfKeyAsBytes, Type keyType, Type valueType, DeserializationSettings settings = null)
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

		private static object Read(BinaryReader reader, byte[] typeIdAsBytes, Type wantedType, DeserializationSettings settings = null)
		{
			if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_8_bit_integerType))
			{
				return reader.ReadByte();
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_8_bit_integerArrayType))
			{
				ulong byteArrayLengthInBytes = reader.ReadUInt64();
				return reader.ReadBytes((int)byteArrayLengthInBytes);
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_16_bit_integerType))
			{
				return reader.ReadUInt16();
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_32_bit_integerType))
			{
				return reader.ReadUInt32();
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_64_bit_integerType))
			{
				return reader.ReadUInt64();
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_16_bit_integerArrayType))
			{
				ulong byteArrayLengthInBytes = reader.ReadUInt64();
				byte[] bytes = reader.ReadBytes((int)byteArrayLengthInBytes);
				ushort[] returnArray = new ushort[byteArrayLengthInBytes / 2];
				Buffer.BlockCopy(bytes, 0, returnArray, 0, (int)byteArrayLengthInBytes);
				return returnArray;
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_32_bit_integerArrayType))
			{
				ulong byteArrayLengthInBytes = reader.ReadUInt64();
				byte[] bytes = reader.ReadBytes((int)byteArrayLengthInBytes);
				uint[] returnArray = new uint[byteArrayLengthInBytes / 4];
				Buffer.BlockCopy(bytes, 0, returnArray, 0, (int)byteArrayLengthInBytes);
				return returnArray;
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_8_bit_integerType))
			{
				return reader.ReadSByte();
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_16_bit_integerType))
			{
				return reader.ReadInt16();
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_32_bit_integerType))
			{
				return reader.ReadInt32();
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_32_bit_integerArrayType))
			{
				ulong byteArrayLengthInBytes = reader.ReadUInt64();
				byte[] bytes = reader.ReadBytes((int)byteArrayLengthInBytes);
				int[] returnArray = new int[byteArrayLengthInBytes / 4];
				Buffer.BlockCopy(bytes, 0, returnArray, 0, (int)byteArrayLengthInBytes);
				return returnArray;
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_64_bit_integerType))
			{
				return reader.ReadInt64();
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.signed_64_bit_integerArrayType))
			{
				ulong byteArrayLengthInBytes = reader.ReadUInt64();
				byte[] bytes = reader.ReadBytes((int)byteArrayLengthInBytes);
				long[] returnArray = new long[byteArrayLengthInBytes / 8];
				Buffer.BlockCopy(bytes, 0, returnArray, 0, (int)byteArrayLengthInBytes);
				return returnArray;
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.floating_point_32_bit))
			{
				return reader.ReadSingle();
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.floating_point_64_bit))
			{
				return reader.ReadDouble();
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.string_utf8))
			{
				ulong stringLengthInBytes = reader.ReadUInt64();
				return Encoding.UTF8.GetString(reader.ReadBytes((int)stringLengthInBytes));
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.booleans))
			{
				return reader.ReadBoolean();
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.datetime_unix_seconds))
			{
				long timeStamp = reader.ReadInt64();
				DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timeStamp);

				if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
				{
					return dateTimeOffset;
				}
				
				return dateTimeOffset.UtcDateTime;// .DateTime;
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.datetime_unix_milliseconds))
			{
				long timeStamp = reader.ReadInt64();
				DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timeStamp);

				if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
				{
					return dateTimeOffset;
				}

				return dateTimeOffset.UtcDateTime;// .DateTime;
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.datetime_iso_8601))
			{
				ulong stringLengthInBytes = reader.ReadUInt64();
				string iso8601 = Encoding.UTF8.GetString(reader.ReadBytes((int)stringLengthInBytes));

				if (wantedType == typeof(DateTimeOffset) || settings?.wantedDateTimeType == typeof(DateTimeOffset))
				{
					return DateTimeOffset.Parse(iso8601, null, DateTimeStyles.RoundtripKind);
				}

				return DateTime.Parse(iso8601, null, DateTimeStyles.RoundtripKind);
			}
			else if (Definitions.ByteArrayCompare(typeIdAsBytes, Definitions.bigIntegerType))
			{
				ulong bigIntegerLengthInBytes = reader.ReadUInt64();
				byte[] tempBytes = reader.ReadBytes((int)bigIntegerLengthInBytes);
				return new BigInteger(tempBytes);
			}

			return null;
		}

		
	}
}
