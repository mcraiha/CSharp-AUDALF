using System;
using System.IO;
using System.Text;
using System.Collections;

namespace CSharp_AUDALF
{
	public enum AUDALF_ValidationResult
	{
		Ok = 0,
		WrongFourCC,
		VersionTooBig,
		UnknownKeyType,
		UnknownValueType
	}

	public static class AUDALF_Deserialize
	{
		public static int[] Deserialize(byte[] payload, bool doSafetyChecks = true)
		{
			return Deserialize(new MemoryStream(payload, writable: false), doSafetyChecks);
		}

		public static int[] Deserialize(Stream inputStream, bool doSafetyChecks = true)
		{
			ulong[] entryOffsets = GetEntryDefinitionOffsets(inputStream);
			int[] returnValues = new int[entryOffsets.Length];
			for (int i = 0; i < returnValues.Length; i++)
			{
				returnValues[i] = (int)ReadListKeyAndValueFromOffset(inputStream, entryOffsets[i]).value;
			}

			return returnValues;
		}

		public static string[] DeserializeStringArray(byte[] payload, bool doSafetyChecks = true)
		{
			return DeserializeStringArray(new MemoryStream(payload, writable: false), doSafetyChecks);
		}

		public static string[] DeserializeStringArray(Stream inputStream, bool doSafetyChecks = true)
		{
			ulong[] entryOffsets = GetEntryDefinitionOffsets(inputStream);
			string[] returnValues = new string[entryOffsets.Length];
			for (int i = 0; i < returnValues.Length; i++)
			{
				returnValues[i] = (string)ReadListKeyAndValueFromOffset(inputStream, entryOffsets[i]).value;
			}

			return returnValues;
		}

		public static bool IsAUDALF(byte[] payload)
		{
			return IsAUDALF(new MemoryStream(payload, writable: false));
		}

		public static bool IsAUDALF(Stream inputStream)
		{
			using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
			{
				byte[] fourCC = reader.ReadBytes(4);
				return ByteArrayCompare(Definitions.fourCC, fourCC);
			}
		}

		public static uint GetVersionNumber(byte[] payload)
		{
			return GetVersionNumber(new MemoryStream(payload, writable: false));
		}

		public static uint GetVersionNumber(Stream inputStream)
		{
			using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
			{
				reader.BaseStream.Seek(Definitions.versionOffset, SeekOrigin.Begin);
				return reader.ReadUInt32();
			}
		}

		public static ulong GetByteSize(byte[] payload)
		{
			return GetByteSize(new MemoryStream(payload, writable: false));
		}

		public static ulong GetByteSize(Stream inputStream)
		{
			using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
			{
				reader.BaseStream.Seek(Definitions.payloadSizeOffset, SeekOrigin.Begin);
				return reader.ReadUInt64();
			}
		}

		public static bool IsDictionary(byte[] payload)
		{
			return IsDictionary(new MemoryStream(payload, writable: false));
		}

		public static bool IsDictionary(Stream inputStream)
		{
			using (BinaryReader reader = new BinaryReader(inputStream))
			{
				reader.BaseStream.Seek(Definitions.keyTypeOffset, SeekOrigin.Begin);
				byte[] keyType = reader.ReadBytes(8);
				return !ByteArrayCompare(Definitions.specialType, keyType);
			}
		}

		public static Type ParseKeyType(byte[] payload)
		{
			return ParseKeyType(new MemoryStream(payload, writable: false));
		}

		public static Type ParseKeyType(Stream inputStream)
		{
			using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
			{
				reader.BaseStream.Seek(Definitions.keyTypeOffset, SeekOrigin.Begin);
				byte[] keyType = reader.ReadBytes(8);
				return typeof(int);
			}
		}

		public static ulong GetIndexCount(byte[] payload)
		{
			return GetIndexCount(new MemoryStream(payload, writable: false));
		}

		public static ulong GetIndexCount(Stream inputStream)
		{
			using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
			{
				reader.BaseStream.Seek(Definitions.indexCountOffset, SeekOrigin.Begin);
				return reader.ReadUInt64();
			}
		}

		public static ulong[] GetEntryDefinitionOffsets(byte[] payload)
		{
			return GetEntryDefinitionOffsets(new MemoryStream(payload));
		}

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

		public static (ulong key, object value) ReadListKeyAndValueFromOffset(byte[] payload, ulong offset)
		{
			return ReadListKeyAndValueFromOffset(new MemoryStream(payload, writable: false), offset);
		}

		public static (ulong key, object value) ReadListKeyAndValueFromOffset(Stream inputStream, ulong offset)
		{
			using (BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true))
			{
				reader.BaseStream.Seek((long)offset, SeekOrigin.Begin);
				ulong key = reader.ReadUInt64();
				object value = null;
				byte[] typeIdAsBytes = reader.ReadBytes(8);

				if (ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_8_bit_integerType))
				{

				}
				else if (ByteArrayCompare(typeIdAsBytes, Definitions.unsigned_32_bit_integerType))
				{
					value = reader.ReadUInt32();
				}
				else if (ByteArrayCompare(typeIdAsBytes, Definitions.signed_32_bit_integerType))
				{
					value = reader.ReadInt32();
				}
				else if (ByteArrayCompare(typeIdAsBytes, Definitions.string_utf8))
				{
					ulong stringLengthInBytes = reader.ReadUInt64();
					value = Encoding.UTF8.GetString(reader.ReadBytes((int)stringLengthInBytes));
				}

				return (key, value);
			}
		}

		private static bool ByteArrayCompare(byte[] a1, byte[] a2) 
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(a1, a2);
		}
	}
}
