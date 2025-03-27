using NUnit.Framework;
using CSharp_AUDALF;
using System;
using System.IO;
using System.Collections.Generic;

using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CollectionAssert = NUnit.Framework.Legacy.CollectionAssert;

namespace Tests
{
	public class DeserializeTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test, Description("Deserialize byte array from AUDALF byte array")]
		public void DeserializeAUDALFBytesToByteArray()
		{
			// Arrange
			byte[] inputArray = new byte[] { /* FOURCC*/ 0x41, 0x55, 0x44, 0x41, 
											/* VERSION NUMBER */ 0x01, 0x00, 0x00, 0x00, 
											/* SIZE OF WHOLE ARRAY */ 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* INDEX COUNT */ 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											
											/* KEY TYPE */ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* ADDRESS OF INDEX #1 */ 0x48, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* ADDRESS OF INDEX #2 */ 0x60, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* ADDRESS OF INDEX #3 */ 0x78, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* ADDRESS OF INDEX #4 */ 0x90, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* ADDRESS OF INDEX #5 */ 0xA8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											
											/* KEY #1 */ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* VALUE TYPE ID #1 */ 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* ACTUAL VALUE #1 */ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											
											/* KEY #2 */ 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* VALUE TYPE ID #2 */ 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* ACTUAL VALUE #2 */ 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											
											/* KEY #3 */ 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* VALUE TYPE ID #3 */ 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* ACTUAL VALUE #3 */ 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											
											/* KEY #4 */ 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* VALUE TYPE ID #4 */ 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* ACTUAL VALUE #4 */ 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											
											/* KEY #5 */ 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* VALUE TYPE ID #5 */ 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
											/* ACTUAL VALUE #5 */ 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 
											};
			byte[] expected = new byte[] { 0, 1, 10, 100, 255 };

			// Act
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(inputArray);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(inputArray);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(inputArray);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(inputArray);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(inputArray);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(inputArray);
			byte[] byteArray = AUDALF_Deserialize.Deserialize<byte>(inputArray);

			// Assert
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)byteArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}

			Assert.AreEqual(byteSize, inputArray.LongLength);
			CollectionAssert.AreEqual(expected, byteArray);
		}

		[Test, Description("Deserialize byte array from AUDALF file")]
		public void DeserializeAUDALFFileToByteArray()
		{
			// Arrange
			byte[] inputArray = File.ReadAllBytes("samples/bytes_0_1_10_100_255.audalf");
			byte[] expected = new byte[] { 0, 1, 10, 100, 255 };

			// Act
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(inputArray);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(inputArray);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(inputArray);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(inputArray);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(inputArray);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(inputArray);
			byte[] byteArray = AUDALF_Deserialize.Deserialize<byte>(inputArray);

			byte[] byteArrayReadOneByOne = new byte[indexCount];
			for (int i = 0; i < entryDefinitionOffsets.Length; i++)
			{
				(ulong index, object value) = AUDALF_Deserialize.ReadListKeyAndValueFromOffset(inputArray, entryDefinitionOffsets[i], typeof(byte));
				byteArrayReadOneByOne[index] = (byte)value;
			}

			// Assert
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)byteArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}

			Assert.AreEqual(byteSize, inputArray.LongLength);
			CollectionAssert.AreEqual(expected, byteArray);
			CollectionAssert.AreEqual(expected, byteArrayReadOneByOne);
		}

		[Test, Description("Deserialize int array from AUDALF file")]
		public void DeserializeAUDALFFileToIntArray()
		{
			// Arrange
			byte[] inputArray = File.ReadAllBytes("samples/ints_0_1_10_100_255_16777216_2147483647.audalf");
			int[] expected = new int[] { 0, 1, 10, 100, 255, 16777216, 2147483647 };

			// Act
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(inputArray);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(inputArray);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(inputArray);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(inputArray);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(inputArray);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(inputArray);
			int[] intArray = AUDALF_Deserialize.Deserialize<int>(inputArray);

			// Assert
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.IsFalse(isDictionary, "Result should contain an array, not a dictionary");
			Assert.AreEqual((ulong)intArray.LongLength, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}

			Assert.AreEqual(byteSize, inputArray.LongLength);
			CollectionAssert.AreEqual(expected, intArray);
		}

		[Test, Description("Deserialize string-string dictionary from AUDALF file")]
		public void DeserializeAUDALFFileToStringStringDictionary()
		{
			// Arrange
			byte[] inputArray = File.ReadAllBytes("samples/string_dictionary.audalf");
			Dictionary<string, string> expected = new Dictionary<string, string>() 
			{
				{ "1", "is one" },
				{ "second", null },
				{ "emojis", "🐶🍦"}
			};

			// Act
			bool isAUDALF = AUDALF_Deserialize.IsAUDALF(inputArray);
			uint versionNumber = AUDALF_Deserialize.GetVersionNumber(inputArray);
			ulong byteSize = AUDALF_Deserialize.GetByteSize(inputArray);
			bool isDictionary = AUDALF_Deserialize.IsDictionary(inputArray);
			ulong indexCount = AUDALF_Deserialize.GetIndexCount(inputArray);
			ReadOnlySpan<byte> keyType = AUDALF_Deserialize.ReadKeyType(inputArray);
			ulong[] entryDefinitionOffsets = AUDALF_Deserialize.GetEntryDefinitionOffsets(inputArray);
			Dictionary<string, string> stringStringDictionary = AUDALF_Deserialize.Deserialize<string, string>(inputArray);

			// Assert
			Assert.IsTrue(isAUDALF, "Result should be AUDALF payload");
			Assert.AreEqual(BitConverter.ToUInt32(Definitions.versionNumber, 0), versionNumber, "Result should have correct version number");
			Assert.IsTrue(isDictionary, "Result should contain an array, not a dictionary");
			CollectionAssert.AreEqual(Definitions.string_utf8, keyType.ToArray(), "KeyType should be string UTF-8");
			Assert.AreEqual((ulong)expected.Count, indexCount, "Result should contain certain number of items");
			Assert.AreEqual(indexCount, (ulong)entryDefinitionOffsets.LongLength, "Result should have certain number of entry definitions");
			
			foreach (ulong u in entryDefinitionOffsets)
			{
				Assert.GreaterOrEqual(u, (ulong)Definitions.entryDefinitionsOffset, "Each entry definition should point to valid address inside the payload");
				Assert.LessOrEqual(u, byteSize, "Each entry definition should point to valid address inside the payload");
				Assert.IsTrue(u % 8 == 0, "Every offset should align to 8 bytes (64 bits)");
			}

			Assert.AreEqual(byteSize, inputArray.LongLength);
			CollectionAssert.AreEqual(expected, stringStringDictionary);
		}
	}
}