using NUnit.Framework;
using CSharp_AUDALF;
using System;
using System.Collections.Generic;
using System.Numerics;

using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CollectionAssert = NUnit.Framework.Legacy.CollectionAssert;
using System.IO;

namespace Tests
{
	public class RoundtripTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test, Description("Byte array roundtrip test")]
		public void ByteArrayRoundtripTest()
		{
			// Arrange
			byte[] byteArray = new byte[] { 0, 1, 10, 100, 255 };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(byteArray);
			byte[] byteArrayDeserialized1 = AUDALF_Deserialize.Deserialize<byte>(result);
			byte[] byteArrayDeserialized2 = AUDALF_Deserialize.Deserialize<byte>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(byteArrayDeserialized1);
			Assert.IsNotNull(byteArrayDeserialized2);

			CollectionAssert.AreEqual(byteArray, byteArrayDeserialized1);
			CollectionAssert.AreEqual(byteArray, byteArrayDeserialized2);

			for (ulong i = 0; i < (ulong)byteArray.Length; i++)
			{
				byte elementAtIndex1 = AUDALF_Deserialize.DeserializeSingleElement<byte>(result, i);
				byte elementAtIndex2 = AUDALF_Deserialize.DeserializeSingleElement<byte>(new MemoryStream(result), i);
				Assert.AreEqual(byteArray[i], elementAtIndex1);
				Assert.AreEqual(byteArray[i], elementAtIndex2);
			}
		}

		[Test, Description("UShort array roundtrip test")]
		public void UShortArrayRoundtripTest()
		{
			// Arrange
			ushort[] ushortArray = new ushort[] { 0, 1, 10, 100, 1000, ushort.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(ushortArray);
			ushort[] uShortArrayDeserialized1 = AUDALF_Deserialize.Deserialize<ushort>(result);
			ushort[] uShortArrayDeserialized2 = AUDALF_Deserialize.Deserialize<ushort>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(uShortArrayDeserialized1);
			Assert.IsNotNull(uShortArrayDeserialized2);

			CollectionAssert.AreEqual(ushortArray, uShortArrayDeserialized1);
			CollectionAssert.AreEqual(ushortArray, uShortArrayDeserialized2);

			for (ulong i = 0; i < (ulong)ushortArray.Length; i++)
			{
				ushort elementAtIndex1 = AUDALF_Deserialize.DeserializeSingleElement<ushort>(result, i);
				ushort elementAtIndex2 = AUDALF_Deserialize.DeserializeSingleElement<ushort>(new MemoryStream(result), i);
				Assert.AreEqual(ushortArray[i], elementAtIndex1);
				Assert.AreEqual(ushortArray[i], elementAtIndex2);
			}
		}

		[Test, Description("UInt array roundtrip test")]
		public void UIntArrayRoundtripTest()
		{
			// Arrange
			uint[] uintArray = new uint[] { 0, 1, 10, 100, 1000, 1000000, uint.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(uintArray);
			uint[] uIntArrayDeserialized1 = AUDALF_Deserialize.Deserialize<uint>(result);
			uint[] uIntArrayDeserialized2 = AUDALF_Deserialize.Deserialize<uint>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(uIntArrayDeserialized1);
			Assert.IsNotNull(uIntArrayDeserialized2);

			CollectionAssert.AreEqual(uintArray, uIntArrayDeserialized1);
			CollectionAssert.AreEqual(uintArray, uIntArrayDeserialized2);

			for (ulong i = 0; i < (ulong)uintArray.Length; i++)
			{
				uint elementAtIndex1 = AUDALF_Deserialize.DeserializeSingleElement<uint>(result, i);
				uint elementAtIndex2 = AUDALF_Deserialize.DeserializeSingleElement<uint>(new MemoryStream(result), i);
				Assert.AreEqual(uintArray[i], elementAtIndex1);
				Assert.AreEqual(uintArray[i], elementAtIndex2);
			}
		}

		[Test, Description("ULong array roundtrip test")]
		public void ULongArrayRoundtripTest()
		{
			// Arrange
			ulong[] ulongArray = new ulong[] { 0, 1, 10, 100, 1000, 1000000, 1000000000, ulong.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(ulongArray);
			ulong[] uLongArrayDeserialized1 = AUDALF_Deserialize.Deserialize<ulong>(result);
			ulong[] uLongArrayDeserialized2 = AUDALF_Deserialize.Deserialize<ulong>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(uLongArrayDeserialized1);
			Assert.IsNotNull(uLongArrayDeserialized2);

			CollectionAssert.AreEqual(ulongArray, uLongArrayDeserialized1);
			CollectionAssert.AreEqual(ulongArray, uLongArrayDeserialized2);

			for (ulong i = 0; i < (ulong)ulongArray.Length; i++)
			{
				ulong elementAtIndex1 = AUDALF_Deserialize.DeserializeSingleElement<ulong>(result, i);
				ulong elementAtIndex2 = AUDALF_Deserialize.DeserializeSingleElement<ulong>(new MemoryStream(result), i);
				Assert.AreEqual(ulongArray[i], elementAtIndex1);
				Assert.AreEqual(ulongArray[i], elementAtIndex2);
			}
		}

		[Test, Description("SByte array roundtrip test")]
		public void SByteArrayRoundtripTest()
		{
			// Arrange
			sbyte[] sbyteArray = new sbyte[] { sbyte.MinValue, 0, 1, 10, 100, sbyte.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(sbyteArray);
			sbyte[] sbyteArrayDeserialized1 = AUDALF_Deserialize.Deserialize<sbyte>(result);
			sbyte[] sbyteArrayDeserialized2 = AUDALF_Deserialize.Deserialize<sbyte>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(sbyteArrayDeserialized1);
			Assert.IsNotNull(sbyteArrayDeserialized2);

			CollectionAssert.AreEqual(sbyteArray, sbyteArrayDeserialized1);
			CollectionAssert.AreEqual(sbyteArray, sbyteArrayDeserialized2);

			for (ulong i = 0; i < (ulong)sbyteArray.Length; i++)
			{
				sbyte elementAtIndex1 = AUDALF_Deserialize.DeserializeSingleElement<sbyte>(result, i);
				sbyte elementAtIndex2 = AUDALF_Deserialize.DeserializeSingleElement<sbyte>(new MemoryStream(result), i);
				Assert.AreEqual(sbyteArray[i], elementAtIndex1);
				Assert.AreEqual(sbyteArray[i], elementAtIndex2);
			}
		}

		[Test, Description("Short array roundtrip test")]
		public void ShortArrayRoundtripTest()
		{
			// Arrange
			short[] shortArray = new short[] { short.MinValue, 0, 1, 10, 100, 1000, short.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(shortArray);
			short[] shortArrayDeserialized1 = AUDALF_Deserialize.Deserialize<short>(result);
			short[] shortArrayDeserialized2 = AUDALF_Deserialize.Deserialize<short>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(shortArrayDeserialized1);
			Assert.IsNotNull(shortArrayDeserialized2);

			CollectionAssert.AreEqual(shortArray, shortArrayDeserialized1);
			CollectionAssert.AreEqual(shortArray, shortArrayDeserialized2);

			for (ulong i = 0; i < (ulong)shortArray.Length; i++)
			{
				short elementAtIndex1 = AUDALF_Deserialize.DeserializeSingleElement<short>(result, i);
				short elementAtIndex2 = AUDALF_Deserialize.DeserializeSingleElement<short>(new MemoryStream(result), i);
				Assert.AreEqual(shortArray[i], elementAtIndex1);
				Assert.AreEqual(shortArray[i], elementAtIndex2);
			}
		}

		[Test, Description("Int array roundtrip test")]
		public void IntArrayRoundtripTest()
		{
			// Arrange
			int[] intArray = new int[] { int.MinValue, 0, 1, 10, 100, 1000, 1000000, int.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(intArray);
			int[] intArrayDeserialized1 = AUDALF_Deserialize.Deserialize<int>(result);
			int[] intArrayDeserialized2 = AUDALF_Deserialize.Deserialize<int>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(intArrayDeserialized1);
			Assert.IsNotNull(intArrayDeserialized2);

			CollectionAssert.AreEqual(intArray, intArrayDeserialized1);
			CollectionAssert.AreEqual(intArray, intArrayDeserialized2);

			for (ulong i = 0; i < (ulong)intArray.Length; i++)
			{
				int elementAtIndex1 = AUDALF_Deserialize.DeserializeSingleElement<int>(result, i);
				int elementAtIndex2 = AUDALF_Deserialize.DeserializeSingleElement<int>(new MemoryStream(result), i);
				Assert.AreEqual(intArray[i], elementAtIndex1);
				Assert.AreEqual(intArray[i], elementAtIndex2);
			}
		}

		[Test, Description("Long array roundtrip test")]
		public void LongArrayRoundtripTest()
		{
			// Arrange
			long[] longArray = new long[] { long.MinValue, 0, 1, 10, 100, 1000, 1000000, 1000000000, long.MaxValue };
			
			// Act
			byte[] result = AUDALF_Serialize.Serialize(longArray);
			long[] LongArrayDeserialized1 = AUDALF_Deserialize.Deserialize<long>(result);
			long[] LongArrayDeserialized2 = AUDALF_Deserialize.Deserialize<long>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(LongArrayDeserialized1);
			Assert.IsNotNull(LongArrayDeserialized2);

			CollectionAssert.AreEqual(longArray, LongArrayDeserialized1);
			CollectionAssert.AreEqual(longArray, LongArrayDeserialized2);

			for (ulong i = 0; i < (ulong)longArray.Length; i++)
			{
				long elementAtIndex1 = AUDALF_Deserialize.DeserializeSingleElement<long>(result, i);
				long elementAtIndex2 = AUDALF_Deserialize.DeserializeSingleElement<long>(new MemoryStream(result), i);
				Assert.AreEqual(longArray[i], elementAtIndex1);
				Assert.AreEqual(longArray[i], elementAtIndex2);
			}
		}

		[Test, Description("Float array roundtrip test")]
		public void FloatArrayRoundtripTest()
		{
			// Arrange
			float[] floatArray = new float[] { float.MinValue, -1, 3.14f, float.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(floatArray);
			float[] floatArrayDeserialized1 = AUDALF_Deserialize.Deserialize<float>(result);
			float[] floatArrayDeserialized2 = AUDALF_Deserialize.Deserialize<float>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(floatArrayDeserialized1);
			Assert.IsNotNull(floatArrayDeserialized2);

			CollectionAssert.AreEqual(floatArray, floatArrayDeserialized1);
			CollectionAssert.AreEqual(floatArray, floatArrayDeserialized2);

			for (ulong i = 0; i < (ulong)floatArray.Length; i++)
			{
				float elementAtIndex1 = AUDALF_Deserialize.DeserializeSingleElement<float>(result, i);
				float elementAtIndex2 = AUDALF_Deserialize.DeserializeSingleElement<float>(new MemoryStream(result), i);
				Assert.AreEqual(floatArray[i], elementAtIndex1);
				Assert.AreEqual(floatArray[i], elementAtIndex2);
			}
		}

		[Test, Description("Double array roundtrip test")]
		public void DoubleArrayRoundtripTest()
		{
			// Arrange
			double[] doubleArray = new double[] { double.MinValue, -1, 0.0, 3.14, double.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(doubleArray);
			double[] doubleArrayDeserialized1 = AUDALF_Deserialize.Deserialize<double>(result);
			double[] doubleArrayDeserialized2 = AUDALF_Deserialize.Deserialize<double>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(doubleArrayDeserialized1);
			Assert.IsNotNull(doubleArrayDeserialized2);

			CollectionAssert.AreEqual(doubleArray, doubleArrayDeserialized1);
			CollectionAssert.AreEqual(doubleArray, doubleArrayDeserialized2);

			for (ulong i = 0; i < (ulong)doubleArray.Length; i++)
			{
				double elementAtIndex1 = AUDALF_Deserialize.DeserializeSingleElement<double>(result, i);
				double elementAtIndex2 = AUDALF_Deserialize.DeserializeSingleElement<double>(new MemoryStream(result), i);
				Assert.AreEqual(doubleArray[i], elementAtIndex1);
				Assert.AreEqual(doubleArray[i], elementAtIndex2);
			}
		}

		[Test, Description("String array roundtrip test")]
		public void StringArrayRoundtripTest()
		{
			// Arrange
			string?[] stringArray = new string?[] { "something", null, "🐱", "null" };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringArray);
			string?[] stringArrayDeserialized1 = AUDALF_Deserialize.Deserialize<string?>(result);
			string?[] stringArrayDeserialized2 = AUDALF_Deserialize.Deserialize<string?>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(stringArrayDeserialized1);
			Assert.IsNotNull(stringArrayDeserialized2);

			CollectionAssert.AreEqual(stringArray, stringArrayDeserialized1);
			CollectionAssert.AreEqual(stringArray, stringArrayDeserialized2);

			for (ulong i = 0; i < (ulong)stringArray.Length; i++)
			{
				string? elementAtIndex1 = AUDALF_Deserialize.DeserializeSingleElement<string>(result, i);
				string? elementAtIndex2 = AUDALF_Deserialize.DeserializeSingleElement<string>(new MemoryStream(result), i);
				Assert.AreEqual(stringArray[i], elementAtIndex1);
				Assert.AreEqual(stringArray[i], elementAtIndex2);
			}
		}

		[Test, Description("String-String dictionary roundtrip test")]
		public void StringStringDictionaryRoundtripTest()
		{
			// Arrange
			Dictionary<string, string?> stringStringDictionary = new Dictionary<string, string?>() 
			{
				{ "1", "is one" },
				{ "second", null },
				{ "emojis", "🐶🍦"}
			};

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringStringDictionary);

			Type keyType1 = AUDALF_Deserialize.ParseKeyType(result);
			Dictionary<string, string?> stringStringDictionaryDeserialized1 = AUDALF_Deserialize.Deserialize<string, string?>(result);

			Type keyType2 = AUDALF_Deserialize.ParseKeyType(new MemoryStream(result));
			Dictionary<string, string?> stringStringDictionaryDeserialized2 = AUDALF_Deserialize.Deserialize<string, string?>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);

			Assert.IsNotNull(stringStringDictionaryDeserialized1);
			Assert.AreEqual(typeof(string), keyType1);

			Assert.IsNotNull(stringStringDictionaryDeserialized2);
			Assert.AreEqual(typeof(string), keyType2);

			CollectionAssert.AreEqual(stringStringDictionary, stringStringDictionaryDeserialized1);
			CollectionAssert.AreEqual(stringStringDictionary, stringStringDictionaryDeserialized2);
		}

		[Test, Description("String-String dictionary roundtrip single values test")]
		public void StringStringDictionaryRoundtripSingleValuesTest()
		{
			// Arrange
			Dictionary<string, string?> stringStringDictionary = new Dictionary<string, string?>() 
			{
				{ "1", "is one" },
				{ "second", null },
				{ "emojis", "🐶🍦"}
			};

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringStringDictionary);

			Type keyType1 = AUDALF_Deserialize.ParseKeyType(result);
			Type keyType2 = AUDALF_Deserialize.ParseKeyType(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(typeof(string), keyType1);
			Assert.AreEqual(typeof(string), keyType2);

			Assert.AreEqual("is one", AUDALF_Deserialize.DeserializeSingleValue<string,string>(result, "1"));
			Assert.IsNull(AUDALF_Deserialize.DeserializeSingleValue<string,string>(result, "second"));
			Assert.AreEqual("🐶🍦", AUDALF_Deserialize.DeserializeSingleValue<string,string>(result, "emojis"));

			Assert.AreEqual("is one", AUDALF_Deserialize.DeserializeSingleValue<string,string>(new MemoryStream(result), "1"));
			Assert.IsNull(AUDALF_Deserialize.DeserializeSingleValue<string,string>(new MemoryStream(result), "second"));
			Assert.AreEqual("🐶🍦", AUDALF_Deserialize.DeserializeSingleValue<string,string>(new MemoryStream(result), "emojis"));
		}

		[Test, Description("Booleans array roundtrip test")]
		public void BooleansArrayRoundtripTest()
		{
			// Arrange
			bool[] boolArray = new bool[] { true, true, true, false, true, false, false, true, false };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(boolArray);
			bool[] boolArrayDeserialized1 = AUDALF_Deserialize.Deserialize<bool>(result);
			bool[] boolArrayDeserialized2 = AUDALF_Deserialize.Deserialize<bool>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(boolArrayDeserialized1);
			Assert.IsNotNull(boolArrayDeserialized2);

			CollectionAssert.AreEqual(boolArray, boolArrayDeserialized1);
			CollectionAssert.AreEqual(boolArray, boolArrayDeserialized2);
		}

		[Test, Description("Big integer array roundtrip test")]
		public void BigIntegerArrayRoundtripTest()
		{
			// Arrange
			BigInteger[] bigIntegerArray = new BigInteger[] { BigInteger.MinusOne, BigInteger.One, 
                               BigInteger.Zero, 120, 128, 255, 1024, 
                               Int64.MinValue, Int64.MaxValue, 
                               BigInteger.Parse("90123123981293054321") };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(bigIntegerArray);
			BigInteger[] bigIntegerArrayDeserialized1 = AUDALF_Deserialize.Deserialize<BigInteger>(result);
			BigInteger[] bigIntegerArrayDeserialized2 = AUDALF_Deserialize.Deserialize<BigInteger>(new MemoryStream(result));

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(bigIntegerArrayDeserialized1);
			Assert.IsNotNull(bigIntegerArrayDeserialized2);

			CollectionAssert.AreEqual(bigIntegerArray, bigIntegerArrayDeserialized1);
			CollectionAssert.AreEqual(bigIntegerArray, bigIntegerArrayDeserialized2);
		}

		[Test, Description("Datetime array roundtrip test")]
		public void DateTimeArrayRoundtripTest()
		{
			// Arrange
			DateTime[] dateTimeArray = new DateTime[] { new DateTime(1966, 1, 1), new DateTime(2000, 2, 28), new DateTime(2022, 6, 6) };
			SerializationSettings settings1 = new SerializationSettings() { dateTimeFormat = DateTimeFormat.ISO8601 };
			SerializationSettings settings2 = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInMilliseconds };
			SerializationSettings settings3 = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInSeconds };

			// Act
			byte[] result1 = AUDALF_Serialize.Serialize(dateTimeArray, settings1);
			byte[] result2 = AUDALF_Serialize.Serialize(dateTimeArray, settings2);
			byte[] result3 = AUDALF_Serialize.Serialize(dateTimeArray, settings3);

			DateTime[] dateTimeArrayDeserialized11 = AUDALF_Deserialize.Deserialize<DateTime>(result1);
			DateTime[] dateTimeArrayDeserialized21 = AUDALF_Deserialize.Deserialize<DateTime>(result2);
			DateTime[] dateTimeArrayDeserialized31 = AUDALF_Deserialize.Deserialize<DateTime>(result3);
			
			DateTime[] dateTimeArrayDeserialized12 = AUDALF_Deserialize.Deserialize<DateTime>(new MemoryStream(result1));
			DateTime[] dateTimeArrayDeserialized22 = AUDALF_Deserialize.Deserialize<DateTime>(new MemoryStream(result2));
			DateTime[] dateTimeArrayDeserialized32 = AUDALF_Deserialize.Deserialize<DateTime>(new MemoryStream(result3));

			// Assert
			Assert.IsNotNull(result1);
			Assert.IsNotNull(result2);
			Assert.IsNotNull(result3);

			CollectionAssert.AreNotEqual(result1, result2, "Results should not be equal because of different time formats");
			CollectionAssert.AreNotEqual(result1, result3, "Results should not be equal because of different time formats");
			CollectionAssert.AreNotEqual(result2, result3, "Results should not be equal because of different time formats");

			Assert.IsNotNull(dateTimeArrayDeserialized11);
			Assert.IsNotNull(dateTimeArrayDeserialized21);
			Assert.IsNotNull(dateTimeArrayDeserialized31);

			Assert.IsNotNull(dateTimeArrayDeserialized12);
			Assert.IsNotNull(dateTimeArrayDeserialized22);
			Assert.IsNotNull(dateTimeArrayDeserialized32);

			CollectionAssert.AreEqual(dateTimeArray, dateTimeArrayDeserialized11);
			CollectionAssert.AreEqual(dateTimeArray, dateTimeArrayDeserialized21);
			CollectionAssert.AreEqual(dateTimeArray, dateTimeArrayDeserialized31);

			CollectionAssert.AreEqual(dateTimeArray, dateTimeArrayDeserialized12);
			CollectionAssert.AreEqual(dateTimeArray, dateTimeArrayDeserialized22);
			CollectionAssert.AreEqual(dateTimeArray, dateTimeArrayDeserialized32);
		}

		[Test, Description("Datetimeoffset array roundtrip test")]
		public void DateTimeOffsetArrayRoundtripTest()
		{
			// Arrange
			DateTimeOffset[] dateTimeOffsetArray = new DateTimeOffset[] { new DateTimeOffset(new DateTime(1966, 1, 1)), new DateTimeOffset(new DateTime(2000, 2, 28)), new DateTimeOffset(new DateTime(2022, 6, 6)) };
			SerializationSettings settings1 = new SerializationSettings() { dateTimeFormat = DateTimeFormat.ISO8601 };
			SerializationSettings settings2 = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInMilliseconds };
			SerializationSettings settings3 = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInSeconds };

			// Act
			byte[] result1 = AUDALF_Serialize.Serialize(dateTimeOffsetArray, settings1);
			byte[] result2 = AUDALF_Serialize.Serialize(dateTimeOffsetArray, settings2);
			byte[] result3 = AUDALF_Serialize.Serialize(dateTimeOffsetArray, settings3);

			DateTimeOffset[] dateTimeArrayDeserialized1 = AUDALF_Deserialize.Deserialize<DateTimeOffset>(result1);
			DateTimeOffset[] dateTimeArrayDeserialized2 = AUDALF_Deserialize.Deserialize<DateTimeOffset>(result2);
			DateTimeOffset[] dateTimeArrayDeserialized3 = AUDALF_Deserialize.Deserialize<DateTimeOffset>(result3);

			// Assert
			Assert.IsNotNull(result1);
			Assert.IsNotNull(result2);
			Assert.IsNotNull(result3);

			CollectionAssert.AreNotEqual(result1, result2, "Results should not be equal because of different time formats");
			CollectionAssert.AreNotEqual(result1, result3, "Results should not be equal because of different time formats");
			CollectionAssert.AreNotEqual(result2, result3, "Results should not be equal because of different time formats");

			Assert.IsNotNull(dateTimeArrayDeserialized1);
			Assert.IsNotNull(dateTimeArrayDeserialized2);
			Assert.IsNotNull(dateTimeArrayDeserialized3);

			CollectionAssert.AreEqual(dateTimeOffsetArray, dateTimeArrayDeserialized1);
			CollectionAssert.AreEqual(dateTimeOffsetArray, dateTimeArrayDeserialized2);
			CollectionAssert.AreEqual(dateTimeOffsetArray, dateTimeArrayDeserialized3);
		}

		[Test, Description("Byte-byte dictionary roundtrip test")]
		public void ByteByteDictionaryRoundtripTest()
		{
			// Arrange
			Dictionary<byte, byte> byteByteDictionary = new Dictionary<byte, byte>() 
			{
				{0, 1},
				{10, 11},
				{100, 101},
				{254, 255},
			};

			// Act
			byte[] result = AUDALF_Serialize.Serialize(byteByteDictionary);
			Dictionary<byte, byte> byteByteDictionaryDeserialized1 = AUDALF_Deserialize.Deserialize<byte, byte>(result, doSafetyChecks: false);
			Dictionary<byte, byte> byteByteDictionaryDeserialized2 = AUDALF_Deserialize.Deserialize<byte, byte>(new MemoryStream(result), doSafetyChecks: false);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(byteByteDictionaryDeserialized1);
			Assert.IsNotNull(byteByteDictionaryDeserialized2);

			Assert.AreEqual(byteByteDictionary, byteByteDictionaryDeserialized1);
			Assert.AreEqual(byteByteDictionary, byteByteDictionaryDeserialized2);
		}

		[Test, Description("Int-int dictionary roundtrip test")]
		public void IntIntDictionaryRoundtripTest()
		{
			// Arrange
			Dictionary<int, int> intIntDictionary = new Dictionary<int, int>() 
			{
				{int.MinValue, int.MinValue + 1},
				{0, 1},
				{10, 11},
				{100, 101},
				{10000, 10001},
				{int.MaxValue - 1, int.MaxValue}
			};

			// Act
			byte[] result = AUDALF_Serialize.Serialize(intIntDictionary);
			Dictionary<int, int> intIntDictionaryDeserialized1 = AUDALF_Deserialize.Deserialize<int, int>(result, doSafetyChecks: false);
			Dictionary<int, int> intIntDictionaryDeserialized2 = AUDALF_Deserialize.Deserialize<int, int>(new MemoryStream(result), doSafetyChecks: false);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(intIntDictionaryDeserialized1);
			Assert.IsNotNull(intIntDictionaryDeserialized2);

			Assert.AreEqual(intIntDictionary, intIntDictionaryDeserialized1);
			Assert.AreEqual(intIntDictionary, intIntDictionaryDeserialized2);
		}

		[Test, Description("String-object dictionary roundtrip test")]
		public void StringObjectDictionaryRoundtripTest()
		{
			// Arrange
			Dictionary<string, object?> stringObjectDictionary = new Dictionary<string, object?>() 
			{
				{ "1", "is one" },
				{ "second", 137f },
				{ "emojis", "🐶🍦"},
				{ "nicebool", true },
				{ "ain", new DateTimeOffset(2011, 11, 17, 4, 45, 32, new TimeSpan(7, 0, 0))},
				
				{ "ushortarray", new ushort[] {0, 1, 1337, ushort.MaxValue } },
				{ "uintarray", new uint[] {1, uint.MinValue, 7, uint.MaxValue} },
				{ "intarray", new int[] {1, int.MinValue, 7, int.MaxValue} },
				{ "longarray", new long[] {1, long.MinValue, 4898797, 13, long.MaxValue} },

				{ "floatarray", new float[] { float.MinValue, -1, 3.14f, float.MaxValue } },
				{ "doublearray", new double[] { double.MinValue, -1, 0.0, 3.14, double.MaxValue } }
			};

			DeserializationSettings deserializationSettings = new DeserializationSettings()
			{
				wantedDateTimeType = typeof(DateTimeOffset)
			};

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringObjectDictionary);
			Dictionary<string, object?> stringObjectDictionaryDeserialized1 = AUDALF_Deserialize.Deserialize<string, object?>(result, doSafetyChecks: false, settings: deserializationSettings);
			Dictionary<string, object?> stringObjectDictionaryDeserialized2 = AUDALF_Deserialize.Deserialize<string, object?>(new MemoryStream(result), doSafetyChecks: false, settings: deserializationSettings);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(stringObjectDictionaryDeserialized1);
			Assert.IsNotNull(stringObjectDictionaryDeserialized2);

			CollectionAssert.AreEqual(stringObjectDictionary, stringObjectDictionaryDeserialized1);
			CollectionAssert.AreEqual(stringObjectDictionary, stringObjectDictionaryDeserialized2);
		}

		[Test, Description("String-object dictionary roundtrip single values test")]
		public void StringObjectDictionaryRoundtripSingleValuesTest()
		{
			// Arrange
			Dictionary<string, object?> stringObjectDictionary = new Dictionary<string, object?>() 
			{
				{ "1", "is one" },
				{ "second", 137f },
				{ "emojis", "🐶🍦"},
				{ "nicebool", true },
				{ "ain", new DateTimeOffset(2011, 11, 17, 4, 45, 32, new TimeSpan(7, 0, 0))},

				{ "bytearray", new byte[] {0, 1, byte.MaxValue } },
				{ "ushortarray", new ushort[] {0, 1, 1337, ushort.MaxValue } },
				{ "uintarray", new uint[] {1, uint.MinValue, 7, uint.MaxValue} },
				{ "ulongtarray", new ulong[] {ulong.MinValue, 1, 489484987, ulong.MaxValue} },

				{ "sbytearray", new sbyte[] {sbyte.MinValue, 0, 1, sbyte.MaxValue } },
				{ "shortarray", new short[] {0, 1, 1337, short.MaxValue, short.MinValue } },
				{ "intarray", new int[] {1, int.MinValue, 7, int.MaxValue} },
				{ "longarray", new long[] {1, long.MinValue, 4898797, 13, long.MaxValue} },

				{ "floatarray", new float[] { float.MinValue, -1, 3.14f, float.MaxValue } },
				{ "doublearray", new double[] { double.MinValue, -1, 0.0, 3.14, double.MaxValue } }
			};

			DeserializationSettings deserializationSettings = new DeserializationSettings()
			{
				wantedDateTimeType = typeof(DateTimeOffset)
			};

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringObjectDictionary);

			// Assert
			Assert.IsNotNull(result);

			Assert.AreEqual("is one", (string?)AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "1"));
			Assert.AreEqual(137f, (float?)AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "second"));
			Assert.AreEqual(true, (bool?)AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "nicebool"));
			Assert.AreEqual(stringObjectDictionary["ain"], (DateTimeOffset?)AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "ain", settings: deserializationSettings));

			Assert.AreEqual("is one", (string?)AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "1"));
			Assert.AreEqual(137f, (float?)AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "second"));
			Assert.AreEqual(true, (bool?)AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "nicebool"));
			Assert.AreEqual(stringObjectDictionary["ain"], (DateTimeOffset?)AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "ain", settings: deserializationSettings));

			CollectionAssert.AreEqual((byte[])stringObjectDictionary["bytearray"]!, (byte[]?)AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "bytearray")!);
			CollectionAssert.AreEqual((ushort[])stringObjectDictionary["ushortarray"]!, (ushort[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "ushortarray")!);
			CollectionAssert.AreEqual((uint[])stringObjectDictionary["uintarray"]!, (uint[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "uintarray")!);
			CollectionAssert.AreEqual((ulong[])stringObjectDictionary["ulongtarray"]!, (ulong[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "ulongtarray")!);

			CollectionAssert.AreEqual((byte[])stringObjectDictionary["bytearray"]!, (byte[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "bytearray")!);
			CollectionAssert.AreEqual((ushort[])stringObjectDictionary["ushortarray"]!, (ushort[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "ushortarray")!);
			CollectionAssert.AreEqual((uint[])stringObjectDictionary["uintarray"]!, (uint[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "uintarray")!);
			CollectionAssert.AreEqual((ulong[])stringObjectDictionary["ulongtarray"]!, (ulong[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "ulongtarray")!);

			CollectionAssert.AreEqual((sbyte[])stringObjectDictionary["sbytearray"]!, (sbyte[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "sbytearray")!);
			CollectionAssert.AreEqual((short[])stringObjectDictionary["shortarray"]!, (short[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "shortarray")!);
			CollectionAssert.AreEqual((int[])stringObjectDictionary["intarray"]!, (int[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "intarray")!);
			CollectionAssert.AreEqual((long[])stringObjectDictionary["longarray"]!, (long[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "longarray")!);

			CollectionAssert.AreEqual((sbyte[])stringObjectDictionary["sbytearray"]!, (sbyte[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "sbytearray")!);
			CollectionAssert.AreEqual((short[])stringObjectDictionary["shortarray"]!, (short[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "shortarray")!);
			CollectionAssert.AreEqual((int[])stringObjectDictionary["intarray"]!, (int[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "intarray")!);
			CollectionAssert.AreEqual((long[])stringObjectDictionary["longarray"]!, (long[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "longarray")!);

			CollectionAssert.AreEqual((float[])stringObjectDictionary["floatarray"]!, (float[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "floatarray")!);
			CollectionAssert.AreEqual((double[])stringObjectDictionary["doublearray"]!, (double[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(result, "doublearray")!);

			CollectionAssert.AreEqual((float[])stringObjectDictionary["floatarray"]!, (float[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "floatarray")!);
			CollectionAssert.AreEqual((double[])stringObjectDictionary["doublearray"]!, (double[])AUDALF_Deserialize.DeserializeSingleValue<string,object?>(new MemoryStream(result), "doublearray")!);
		}

		[Test, Description("String-byte array dictionary roundtrip test")]
		public void StringByteArrayDictionaryRoundtripTest()
		{
			// Arrange
			Dictionary<string, byte[]?> stringByteArrayDictionary = new Dictionary<string, byte[]?>() 
			{
				{ "1", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, byte.MaxValue } },
				{ "second", null },
				{ "threes", new byte[] { 127, 128, 111 } },
				{ "four", new byte[] {  } },
				{ "fiv", new byte[] { 42 } },
			};

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringByteArrayDictionary);
			Dictionary<string, byte[]?> stringByteArrayDictionaryDeserialized1 = AUDALF_Deserialize.Deserialize<string, byte[]?>(result, doSafetyChecks: false);
			Dictionary<string, byte[]?> stringByteArrayDictionaryDeserialized2 = AUDALF_Deserialize.Deserialize<string, byte[]?>(new MemoryStream(result), doSafetyChecks: false);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(stringByteArrayDictionaryDeserialized1);
			Assert.IsNotNull(stringByteArrayDictionaryDeserialized2);

			CollectionAssert.AreEqual(stringByteArrayDictionary, stringByteArrayDictionaryDeserialized1);
			CollectionAssert.AreEqual(stringByteArrayDictionary, stringByteArrayDictionaryDeserialized2);
		}
	}
}