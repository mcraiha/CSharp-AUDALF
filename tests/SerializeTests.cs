using NUnit.Framework;
using CSharp_AUDALF;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Tests
{
	public class SerializeTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test, Description("Serialize byte array to AUDALF list")]
		public void SerializeByteArrayToAUDALFList()
		{
			// Arrange
			byte[] byteArray = new byte[] { 0, 1, 10, 100, 255 };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(byteArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)byteArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize ushort array to AUDALF list")]
		public void SerializeUShortArrayToAUDALFList()
		{
			// Arrange
			ushort[] ushortArray = new ushort[] { 0, 1, 10, 100, 1000, ushort.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(ushortArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)ushortArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize uint array to AUDALF list")]
		public void SerializeUIntArrayToAUDALFList()
		{
			// Arrange
			uint[] uintArray = new uint[] { 0, 1, 10, 100, 1000, 1000000, uint.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(uintArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)uintArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize ulong array to AUDALF list")]
		public void SerializeULongArrayToAUDALFList()
		{
			// Arrange
			ulong[] ulongArray = new ulong[] { 0, 1, 10, 100, 1000, 1000000, 1000000000, ulong.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(ulongArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)ulongArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize sbyte array to AUDALF list")]
		public void SerializeSByteArrayToAUDALFList()
		{
			// Arrange
			sbyte[] sbyteArray = new sbyte[] { sbyte.MinValue, 0, 1, 10, 100, sbyte.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(sbyteArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)sbyteArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize short array to AUDALF list")]
		public void SerializeShortArrayToAUDALFList()
		{
			// Arrange
			short[] shortArray = new short[] { short.MinValue, 0, 1, 10, 100, 1000, short.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(shortArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)shortArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize int array to AUDALF list")]
		public void SerializeIntArrayToAUDALFList()
		{
			// Arrange
			int[] intArray = new int[] { int.MinValue, 1, 10, 100, 1000, int.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(intArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)intArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize long array to AUDALF list")]
		public void SerializeLongArrayToAUDALFList()
		{
			// Arrange
			long[] longArray = new long[] { long.MinValue, 0, 1, 10, 100, 1000, 1000000, 1000000000, long.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(longArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)longArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize float array to AUDALF list")]
		public void SerializeFloatArrayToAUDALFList()
		{
			// Arrange
			float[] floatArray = new float[] { float.MinValue, -1, 3.14f, float.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(floatArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)floatArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize double array to AUDALF list")]
		public void SerializeDoubleArrayToAUDALFList()
		{
			// Arrange
			double[] doubleArray = new double[] { double.MinValue, -1, 0.0, 3.14, double.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(doubleArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)doubleArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize string array to AUDALF list")]
		public void SerializeStringArrayToAUDALFList()
		{
			// Arrange
			string[] stringArray = new string[] { "something", null, "🐱", "null" };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)stringArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize string-string dictionary to AUDALF dictionary")]
		public void SerializeStringStringDictionary()
		{
			// Arrange
			Dictionary<string, string> stringStringDictionary = new Dictionary<string, string>() 
			{
				{ "1", "is one" },
				{ "second", null },
				{ "emojis", "🐶🍦"}
			};

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringStringDictionary);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsTrue(isDictionary, "Result should contain a dictionary, not an array");
			Assert.AreEqual((ulong)stringStringDictionary.Count, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize datetime array to AUDALF list")]
		public void SerializeDateTimeArray()
		{
			// Arrange
			DateTime[] dateTimeArray = new DateTime[] {new DateTime(1966, 1, 1), new DateTime(2000, 2, 28), new DateTime(2022, 6, 6)};
			SerializationSettings settings1 = new SerializationSettings() { dateTimeFormat = DateTimeFormat.ISO8601 };
			SerializationSettings settings2 = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInMilliseconds };
			SerializationSettings settings3 = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInSeconds };

			// Act
			byte[] result1 = AUDALF_Serialize.Serialize(dateTimeArray, settings1);
			bool isAUDALF1 = AUDALF_Deserialize.IsAUDALF(result1);
			uint versionNumber1 = AUDALF_Deserialize.GetVersionNumber(result1);
			ulong byteSize1 = AUDALF_Deserialize.GetByteSize(result1);
			bool isDictionary1 = AUDALF_Deserialize.IsDictionary(result1);
			ulong indexCount1 = AUDALF_Deserialize.GetIndexCount(result1);
			ulong[] entryDefinitionOffsets1 = AUDALF_Deserialize.GetEntryDefinitionOffsets(result1);

			byte[] result2 = AUDALF_Serialize.Serialize(dateTimeArray, settings2);
			bool isAUDALF2 = AUDALF_Deserialize.IsAUDALF(result2);
			uint versionNumber2 = AUDALF_Deserialize.GetVersionNumber(result2);
			ulong byteSize2 = AUDALF_Deserialize.GetByteSize(result2);
			bool isDictionary2 = AUDALF_Deserialize.IsDictionary(result2);
			ulong indexCount2 = AUDALF_Deserialize.GetIndexCount(result2);
			ulong[] entryDefinitionOffsets2 = AUDALF_Deserialize.GetEntryDefinitionOffsets(result2);

			byte[] result3 = AUDALF_Serialize.Serialize(dateTimeArray, settings3);
			bool isAUDALF3 = AUDALF_Deserialize.IsAUDALF(result3);
			uint versionNumber3 = AUDALF_Deserialize.GetVersionNumber(result3);
			ulong byteSize3 = AUDALF_Deserialize.GetByteSize(result3);
			bool isDictionary3 = AUDALF_Deserialize.IsDictionary(result3);
			ulong indexCount3 = AUDALF_Deserialize.GetIndexCount(result3);
			ulong[] entryDefinitionOffsets3 = AUDALF_Deserialize.GetEntryDefinitionOffsets(result3);

			// Assert
			Assert.IsNotNull(result1, "Result should NOT be null");
			Assert.IsTrue(isAUDALF1, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber1, "Result should have correct version number");
			Assert.AreEqual(result1.LongLength, byteSize1, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary1, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)dateTimeArray.LongLength, indexCount1, "Result should contain certain number of items");
			Assert.AreEqual(indexCount1, (ulong)entryDefinitionOffsets1.LongLength, "Result should have certain number of entry definitions");

			Assert.IsNotNull(result2, "Result should NOT be null");
			Assert.IsTrue(isAUDALF2, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber2, "Result should have correct version number");
			Assert.AreEqual(result2.LongLength, byteSize2, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary2, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)dateTimeArray.LongLength, indexCount2, "Result should contain certain number of items");
			Assert.AreEqual(indexCount2, (ulong)entryDefinitionOffsets2.LongLength, "Result should have certain number of entry definitions");

			Assert.IsNotNull(result3, "Result should NOT be null");
			Assert.IsTrue(isAUDALF3, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber3, "Result should have correct version number");
			Assert.AreEqual(result3.LongLength, byteSize3, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary3, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)dateTimeArray.LongLength, indexCount3, "Result should contain certain number of items");
			Assert.AreEqual(indexCount3, (ulong)entryDefinitionOffsets3.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets1)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize1, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}

			foreach (ulong u in entryDefinitionOffsets2)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize2, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}

			foreach (ulong u in entryDefinitionOffsets3)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize3, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}

			CollectionAssert.AreNotEqual(result1, result2);
			CollectionAssert.AreNotEqual(result2, result3);
		}

		[Test, Description("Serialize bool array to AUDALF list")]
		public void SerializeBoolArrayToAUDALFList()
		{
			// Arrange
			bool[] boolArray = new bool[] { true, true, true, false, true, false, false, true, false };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(boolArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)boolArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize big integer array to AUDALF list")]
		public void SerializeBigIntegerArrayToAUDALFList()
		{
			// Arrange
			BigInteger[] bigIntegerArray = new BigInteger[] { BigInteger.MinusOne, BigInteger.One, 
                               BigInteger.Zero, 120, 128, 255, 1024, 
                               Int64.MinValue, Int64.MaxValue, 
                               BigInteger.Parse("90123123981293054321") };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(bigIntegerArray);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)bigIntegerArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize datetimeoffset array to AUDALF list")]
		public void SerializeDateTimeOffsetArrayToAUDALFList()
		{
			// Arrange
			DateTimeOffset[] dateTimeOffsetArray = new DateTimeOffset[] { new DateTimeOffset(new DateTime(1966, 1, 1)), new DateTimeOffset(new DateTime(2000, 2, 28)), new DateTimeOffset(new DateTime(2022, 6, 6)) };
			SerializationSettings settings1 = new SerializationSettings() { dateTimeFormat = DateTimeFormat.ISO8601 };
			SerializationSettings settings2 = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInMilliseconds };
			SerializationSettings settings3 = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInSeconds };

			// Act
			byte[] result1 = AUDALF_Serialize.Serialize(dateTimeOffsetArray, settings1);
			bool isAUDALF1 = AUDALF_Deserialize.IsAUDALF(result1);
			uint versionNumber1 = AUDALF_Deserialize.GetVersionNumber(result1);
			ulong byteSize1 = AUDALF_Deserialize.GetByteSize(result1);
			bool isDictionary1 = AUDALF_Deserialize.IsDictionary(result1);
			ulong indexCount1 = AUDALF_Deserialize.GetIndexCount(result1);
			ulong[] entryDefinitionOffsets1 = AUDALF_Deserialize.GetEntryDefinitionOffsets(result1);

			byte[] result2 = AUDALF_Serialize.Serialize(dateTimeOffsetArray, settings2);
			bool isAUDALF2 = AUDALF_Deserialize.IsAUDALF(result2);
			uint versionNumber2 = AUDALF_Deserialize.GetVersionNumber(result2);
			ulong byteSize2 = AUDALF_Deserialize.GetByteSize(result2);
			bool isDictionary2 = AUDALF_Deserialize.IsDictionary(result2);
			ulong indexCount2 = AUDALF_Deserialize.GetIndexCount(result2);
			ulong[] entryDefinitionOffsets2 = AUDALF_Deserialize.GetEntryDefinitionOffsets(result2);

			byte[] result3 = AUDALF_Serialize.Serialize(dateTimeOffsetArray, settings3);
			bool isAUDALF3 = AUDALF_Deserialize.IsAUDALF(result3);
			uint versionNumber3 = AUDALF_Deserialize.GetVersionNumber(result3);
			ulong byteSize3 = AUDALF_Deserialize.GetByteSize(result3);
			bool isDictionary3 = AUDALF_Deserialize.IsDictionary(result3);
			ulong indexCount3 = AUDALF_Deserialize.GetIndexCount(result3);
			ulong[] entryDefinitionOffsets3 = AUDALF_Deserialize.GetEntryDefinitionOffsets(result3);

			// Assert
			Assert.IsNotNull(result1, "Result should NOT be null");
			Assert.IsTrue(isAUDALF1, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber1, "Result should have correct version number");
			Assert.AreEqual(result1.LongLength, byteSize1, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary1, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)dateTimeOffsetArray.LongLength, indexCount1, "Result should contain certain number of items");
			Assert.AreEqual(indexCount1, (ulong)entryDefinitionOffsets1.LongLength, "Result should have certain number of entry definitions");

			Assert.IsNotNull(result2, "Result should NOT be null");
			Assert.IsTrue(isAUDALF2, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber2, "Result should have correct version number");
			Assert.AreEqual(result2.LongLength, byteSize2, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary2, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)dateTimeOffsetArray.LongLength, indexCount2, "Result should contain certain number of items");
			Assert.AreEqual(indexCount2, (ulong)entryDefinitionOffsets2.LongLength, "Result should have certain number of entry definitions");

			Assert.IsNotNull(result3, "Result should NOT be null");
			Assert.IsTrue(isAUDALF3, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber3, "Result should have correct version number");
			Assert.AreEqual(result3.LongLength, byteSize3, "Result payload should have correct amount lenght info");
			Assert.IsFalse(isDictionary3, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)dateTimeOffsetArray.LongLength, indexCount3, "Result should contain certain number of items");
			Assert.AreEqual(indexCount3, (ulong)entryDefinitionOffsets3.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets1)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize1, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}

			foreach (ulong u in entryDefinitionOffsets2)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize2, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}

			foreach (ulong u in entryDefinitionOffsets3)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize3, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}

			CollectionAssert.AreNotEqual(result1, result2);
			CollectionAssert.AreNotEqual(result2, result3);
		}

		[Test, Description("Serialize string-object dictionary to AUDALF dictionary")]
		public void SerializeStringObjectDictionary()
		{
			// Arrange
			Dictionary<string, object> stringObjectDictionary = new Dictionary<string, object>() 
			{
				{ "1", "is one" },
				{ "second", 137f },
				{ "emojis", "🐶🍦"},
				{ "nicebool", true },
				{ "ain", new DateTimeOffset(2011, 11, 17, 4, 45, 32, new TimeSpan(7, 0, 0)) }
			};

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringObjectDictionary);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsTrue(isDictionary, "Result should contain a dictionary, not an array");
			Assert.AreEqual((ulong)stringObjectDictionary.Count, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			CollectionAssert.AllItemsAreUnique(entryDefinitionOffsets);

			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}

		[Test, Description("Serialize string-bytearray dictionary to AUDALF dictionary")]
		public void SerializeStringByteArrayDictionary()
		{
			// Arrange
			Dictionary<string, byte[]> stringByteArrayDictionary = new Dictionary<string, byte[]>() 
			{
				{ "1", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, byte.MaxValue } },
				{ "second", null },
				{ "threes", new byte[] { 127, 128, 111 } },
				{ "four", new byte[] {  } },
				{ "fiv", new byte[] { 42 } },
			};

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringByteArrayDictionary);
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(result);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(result);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(result);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(result);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(result);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(result);

			// Assert
			Assert.IsNotNull(result, "Result should NOT be null");
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.AreEqual(result.LongLength, byteSize, "Result payload should have correct amount lenght info");
			Assert.IsTrue(isDictionary, "Result should contain a dictionary, not an array");
			Assert.AreEqual((ulong)stringByteArrayDictionary.Count, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}
		}
	}
}