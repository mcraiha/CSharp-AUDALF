using NUnit.Framework;
using CSharp_AUDALF;
using System;
using System.Collections.Generic;
using System.Numerics;

using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CollectionAssert = NUnit.Framework.Legacy.CollectionAssert;

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
			byte[] byteArrayDeserialized = AUDALF_Deserialize.Deserialize<byte>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(byteArrayDeserialized);

			CollectionAssert.AreEqual(byteArray, byteArrayDeserialized);

			for (ulong i = 0; i < (ulong)byteArray.Length; i++)
			{
				byte elementAtIndex = AUDALF_Deserialize.DeserializeSingleElement<byte>(result, i);
				Assert.AreEqual(byteArray[i], elementAtIndex);
			}
		}

		[Test, Description("UShort array roundtrip test")]
		public void UShortArrayRoundtripTest()
		{
			// Arrange
			ushort[] ushortArray = new ushort[] { 0, 1, 10, 100, 1000, ushort.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(ushortArray);
			ushort[] uShortArrayDeserialized = AUDALF_Deserialize.Deserialize<ushort>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(uShortArrayDeserialized);

			CollectionAssert.AreEqual(ushortArray, uShortArrayDeserialized);

			for (ulong i = 0; i < (ulong)ushortArray.Length; i++)
			{
				ushort elementAtIndex = AUDALF_Deserialize.DeserializeSingleElement<ushort>(result, i);
				Assert.AreEqual(ushortArray[i], elementAtIndex);
			}
		}

		[Test, Description("UInt array roundtrip test")]
		public void UIntArrayRoundtripTest()
		{
			// Arrange
			uint[] uintArray = new uint[] { 0, 1, 10, 100, 1000, 1000000, uint.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(uintArray);
			uint[] uIntArrayDeserialized = AUDALF_Deserialize.Deserialize<uint>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(uIntArrayDeserialized);

			CollectionAssert.AreEqual(uintArray, uIntArrayDeserialized);

			for (ulong i = 0; i < (ulong)uintArray.Length; i++)
			{
				uint elementAtIndex = AUDALF_Deserialize.DeserializeSingleElement<uint>(result, i);
				Assert.AreEqual(uintArray[i], elementAtIndex);
			}
		}

		[Test, Description("ULong array roundtrip test")]
		public void ULongArrayRoundtripTest()
		{
			// Arrange
			ulong[] ulongArray = new ulong[] { 0, 1, 10, 100, 1000, 1000000, 1000000000, ulong.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(ulongArray);
			ulong[] uLongArrayDeserialized = AUDALF_Deserialize.Deserialize<ulong>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(uLongArrayDeserialized);

			CollectionAssert.AreEqual(ulongArray, uLongArrayDeserialized);

			for (ulong i = 0; i < (ulong)ulongArray.Length; i++)
			{
				ulong elementAtIndex = AUDALF_Deserialize.DeserializeSingleElement<ulong>(result, i);
				Assert.AreEqual(ulongArray[i], elementAtIndex);
			}
		}

		[Test, Description("SByte array roundtrip test")]
		public void SByteArrayRoundtripTest()
		{
			// Arrange
			sbyte[] sbyteArray = new sbyte[] { sbyte.MinValue, 0, 1, 10, 100, sbyte.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(sbyteArray);
			sbyte[] sbyteArrayDeserialized = AUDALF_Deserialize.Deserialize<sbyte>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(sbyteArrayDeserialized);

			CollectionAssert.AreEqual(sbyteArray, sbyteArrayDeserialized);

			for (ulong i = 0; i < (ulong)sbyteArray.Length; i++)
			{
				sbyte elementAtIndex = AUDALF_Deserialize.DeserializeSingleElement<sbyte>(result, i);
				Assert.AreEqual(sbyteArray[i], elementAtIndex);
			}
		}

		[Test, Description("Short array roundtrip test")]
		public void ShortArrayRoundtripTest()
		{
			// Arrange
			short[] shortArray = new short[] { short.MinValue, 0, 1, 10, 100, 1000, short.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(shortArray);
			short[] shortArrayDeserialized = AUDALF_Deserialize.Deserialize<short>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(shortArrayDeserialized);

			CollectionAssert.AreEqual(shortArray, shortArrayDeserialized);

			for (ulong i = 0; i < (ulong)shortArray.Length; i++)
			{
				short elementAtIndex = AUDALF_Deserialize.DeserializeSingleElement<short>(result, i);
				Assert.AreEqual(shortArray[i], elementAtIndex);
			}
		}

		[Test, Description("Int array roundtrip test")]
		public void IntArrayRoundtripTest()
		{
			// Arrange
			int[] intArray = new int[] { int.MinValue, 0, 1, 10, 100, 1000, 1000000, int.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(intArray);
			int[] intArrayDeserialized = AUDALF_Deserialize.Deserialize<int>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(intArrayDeserialized);

			CollectionAssert.AreEqual(intArray, intArrayDeserialized);

			for (ulong i = 0; i < (ulong)intArray.Length; i++)
			{
				int elementAtIndex = AUDALF_Deserialize.DeserializeSingleElement<int>(result, i);
				Assert.AreEqual(intArray[i], elementAtIndex);
			}
		}

		[Test, Description("Long array roundtrip test")]
		public void LongArrayRoundtripTest()
		{
			// Arrange
			long[] longArray = new long[] { long.MinValue, 0, 1, 10, 100, 1000, 1000000, 1000000000, long.MaxValue };
			
			// Act
			byte[] result = AUDALF_Serialize.Serialize(longArray);
			long[] LongArrayDeserialized = AUDALF_Deserialize.Deserialize<long>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(LongArrayDeserialized);

			CollectionAssert.AreEqual(longArray, LongArrayDeserialized);

			for (ulong i = 0; i < (ulong)longArray.Length; i++)
			{
				long elementAtIndex = AUDALF_Deserialize.DeserializeSingleElement<long>(result, i);
				Assert.AreEqual(longArray[i], elementAtIndex);
			}
		}

		[Test, Description("Float array roundtrip test")]
		public void FloatArrayRoundtripTest()
		{
			// Arrange
			float[] floatArray = new float[] { float.MinValue, -1, 3.14f, float.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(floatArray);
			float[] floatArrayDeserialized = AUDALF_Deserialize.Deserialize<float>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(floatArrayDeserialized);

			CollectionAssert.AreEqual(floatArray, floatArrayDeserialized);

			for (ulong i = 0; i < (ulong)floatArray.Length; i++)
			{
				float elementAtIndex = AUDALF_Deserialize.DeserializeSingleElement<float>(result, i);
				Assert.AreEqual(floatArray[i], elementAtIndex);
			}
		}

		[Test, Description("Double array roundtrip test")]
		public void DoubleArrayRoundtripTest()
		{
			// Arrange
			double[] doubleArray = new double[] { double.MinValue, -1, 0.0, 3.14, double.MaxValue };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(doubleArray);
			double[] doubleArrayDeserialized = AUDALF_Deserialize.Deserialize<double>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(doubleArrayDeserialized);

			CollectionAssert.AreEqual(doubleArray, doubleArrayDeserialized);

			for (ulong i = 0; i < (ulong)doubleArray.Length; i++)
			{
				double elementAtIndex = AUDALF_Deserialize.DeserializeSingleElement<double>(result, i);
				Assert.AreEqual(doubleArray[i], elementAtIndex);
			}
		}

		[Test, Description("String array roundtrip test")]
		public void StringArrayRoundtripTest()
		{
			// Arrange
			string[] stringArray = new string[] { "something", null, "🐱", "null" };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringArray);
			string[] stringArrayDeserialized = AUDALF_Deserialize.Deserialize<string>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(stringArrayDeserialized);

			CollectionAssert.AreEqual(stringArray, stringArrayDeserialized);

			for (ulong i = 0; i < (ulong)stringArray.Length; i++)
			{
				string elementAtIndex = AUDALF_Deserialize.DeserializeSingleElement<string>(result, i);
				Assert.AreEqual(stringArray[i], elementAtIndex);
			}
		}

		[Test, Description("String-String dictionary roundtrip test")]
		public void StringStringDictionaryRoundtripTest()
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
			Type keyType = AUDALF_Deserialize.ParseKeyType(result);
			Dictionary<string, string> stringStringDictionaryDeserialized = AUDALF_Deserialize.Deserialize<string, string>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(stringStringDictionaryDeserialized);
			Assert.AreEqual(typeof(string), keyType);

			CollectionAssert.AreEqual(stringStringDictionary, stringStringDictionaryDeserialized);
		}

		[Test, Description("String-String dictionary roundtrip single values test")]
		public void StringStringDictionaryRoundtripSingleValuesTest()
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
			Type keyType = AUDALF_Deserialize.ParseKeyType(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(typeof(string), keyType);

			Assert.AreEqual("is one", AUDALF_Deserialize.DeserializeSingleValue<string,string>(result, "1"));
			Assert.IsNull(AUDALF_Deserialize.DeserializeSingleValue<string,string>(result, "second"));
			Assert.AreEqual("🐶🍦", AUDALF_Deserialize.DeserializeSingleValue<string,string>(result, "emojis"));
		}

		[Test, Description("Booleans array roundtrip test")]
		public void BooleansArrayRoundtripTest()
		{
			// Arrange
			bool[] boolArray = new bool[] { true, true, true, false, true, false, false, true, false };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(boolArray);
			bool[] boolArrayDeserialized = AUDALF_Deserialize.Deserialize<bool>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(boolArrayDeserialized);

			CollectionAssert.AreEqual(boolArray, boolArrayDeserialized);
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
			BigInteger[] bigIntegerArrayDeserialized = AUDALF_Deserialize.Deserialize<BigInteger>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(bigIntegerArrayDeserialized);

			CollectionAssert.AreEqual(bigIntegerArray, bigIntegerArrayDeserialized);
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

			DateTime[] dateTimeArrayDeserialized1 = AUDALF_Deserialize.Deserialize<DateTime>(result1);
			DateTime[] dateTimeArrayDeserialized2 = AUDALF_Deserialize.Deserialize<DateTime>(result2);
			DateTime[] dateTimeArrayDeserialized3 = AUDALF_Deserialize.Deserialize<DateTime>(result3);

			// Assert
			Assert.IsNotNull(result1);
			Assert.IsNotNull(result2);
			Assert.IsNotNull(result3);

			Assert.IsNotNull(dateTimeArrayDeserialized1);
			Assert.IsNotNull(dateTimeArrayDeserialized2);
			Assert.IsNotNull(dateTimeArrayDeserialized3);

			CollectionAssert.AreEqual(dateTimeArray, dateTimeArrayDeserialized1);
			CollectionAssert.AreEqual(dateTimeArray, dateTimeArrayDeserialized2);
			CollectionAssert.AreEqual(dateTimeArray, dateTimeArrayDeserialized3);
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
			Dictionary<byte, byte> byteByteDictionaryDeserialized = AUDALF_Deserialize.Deserialize<byte, byte>(result, doSafetyChecks: false);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(byteByteDictionaryDeserialized);

			Assert.AreEqual(byteByteDictionary, byteByteDictionaryDeserialized);
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
			Dictionary<int, int> intIntDictionaryDeserialized = AUDALF_Deserialize.Deserialize<int, int>(result, doSafetyChecks: false);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(intIntDictionaryDeserialized);

			Assert.AreEqual(intIntDictionary, intIntDictionaryDeserialized);
		}

		[Test, Description("String-object dictionary roundtrip test")]
		public void StringObjectDictionaryRoundtripTest()
		{
			// Arrange
			Dictionary<string, object> stringObjectDictionary = new Dictionary<string, object>() 
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

				{ "floatarray", new float[] { float.MinValue, -1, 3.14f, float.MaxValue } }
			};

			DeserializationSettings deserializationSettings = new DeserializationSettings()
			{
				wantedDateTimeType = typeof(DateTimeOffset)
			};

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringObjectDictionary);
			Dictionary<string, object> stringObjectDictionaryDeserialized = AUDALF_Deserialize.Deserialize<string, object>(result, doSafetyChecks: false, settings: deserializationSettings);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(stringObjectDictionaryDeserialized);

			CollectionAssert.AreEqual(stringObjectDictionary, stringObjectDictionaryDeserialized);
		}

		[Test, Description("String-object dictionary roundtrip single values test")]
		public void StringObjectDictionaryRoundtripSingleValuesTest()
		{
			// Arrange
			Dictionary<string, object> stringObjectDictionary = new Dictionary<string, object>() 
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

				{ "floatarray", new float[] { float.MinValue, -1, 3.14f, float.MaxValue } }
			};

			DeserializationSettings deserializationSettings = new DeserializationSettings()
			{
				wantedDateTimeType = typeof(DateTimeOffset)
			};

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringObjectDictionary);

			// Assert
			Assert.IsNotNull(result);

			Assert.AreEqual("is one", (string)AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "1"));
			Assert.AreEqual(137f, (float)AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "second"));
			Assert.AreEqual(true, (bool)AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "nicebool"));
			Assert.AreEqual(stringObjectDictionary["ain"], (DateTimeOffset)AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "ain", settings: deserializationSettings));

			CollectionAssert.AreEqual((byte[])stringObjectDictionary["bytearray"], (byte[])AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "bytearray"));
			CollectionAssert.AreEqual((ushort[])stringObjectDictionary["ushortarray"], (ushort[])AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "ushortarray"));
			CollectionAssert.AreEqual((uint[])stringObjectDictionary["uintarray"], (uint[])AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "uintarray"));
			CollectionAssert.AreEqual((ulong[])stringObjectDictionary["ulongtarray"], (ulong[])AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "ulongtarray"));

			CollectionAssert.AreEqual((sbyte[])stringObjectDictionary["sbytearray"], (sbyte[])AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "sbytearray"));
			CollectionAssert.AreEqual((short[])stringObjectDictionary["shortarray"], (short[])AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "shortarray"));
			CollectionAssert.AreEqual((int[])stringObjectDictionary["intarray"], (int[])AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "intarray"));
			CollectionAssert.AreEqual((long[])stringObjectDictionary["longarray"], (long[])AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "longarray"));

			CollectionAssert.AreEqual((float[])stringObjectDictionary["floatarray"], (float[])AUDALF_Deserialize.DeserializeSingleValue<string,object>(result, "floatarray"));
		}

		[Test, Description("String-byte array dictionary roundtrip test")]
		public void StringByteArrayDictionaryRoundtripTest()
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
			Dictionary<string, byte[]> stringByteArrayDictionaryDeserialized = AUDALF_Deserialize.Deserialize<string, byte[]>(result, doSafetyChecks: false);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(stringByteArrayDictionaryDeserialized);

			CollectionAssert.AreEqual(stringByteArrayDictionary, stringByteArrayDictionaryDeserialized);
		}
	}
}