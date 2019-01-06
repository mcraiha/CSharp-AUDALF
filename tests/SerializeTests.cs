using NUnit.Framework;
using CSharp_AUDALF;
using System;
using System.Collections.Generic;

namespace Tests
{
	public class SerializeTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void SerializeByteArray()
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

		[Test]
		public void SerializeUShortArray()
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

		[Test]
		public void SerializeUIntArray()
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

		[Test]
		public void SerializeULongArray()
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

		[Test]
		public void SerializeSByteArray()
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

		[Test]
		public void SerializeShortArray()
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

		[Test]
		public void SerializeIntArray()
		{
			// Arrange
			int[] intArray = new int[] { 1, 10, 100, 1000 };

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

		[Test]
		public void SerializeLongArray()
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

		[Test]
		public void SerializeFloatArray()
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

		[Test]
		public void SerializeDoubleArray()
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

		[Test]
		public void SerializeStringArray()
		{
			// Arrange
			string[] stringArray = new string[] { "something", null, "🐱" };

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

		[Test]
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

		[Test]
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
	}
}