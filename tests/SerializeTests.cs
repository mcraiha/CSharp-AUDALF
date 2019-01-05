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
	}
}