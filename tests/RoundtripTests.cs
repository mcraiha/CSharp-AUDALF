using NUnit.Framework;
using CSharp_AUDALF;
using System;
using System.Collections.Generic;

namespace Tests
{
	public class RoundtripTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
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
		}

		[Test]
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
		}

		[Test]
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
		}

		[Test]
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
		}

		[Test]
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
		}

		[Test]
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
		}

		[Test]
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
		}

		[Test]
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
		}

		[Test]
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
		}

		[Test]
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
		}

		[Test]
		public void StringArrayRoundtripTest()
		{
			// Arrange
			string[] stringArray = new string[] { "something", null, "🐱" };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringArray);
			string[] stringArrayDeserialized = AUDALF_Deserialize.Deserialize<string>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(stringArrayDeserialized);

			CollectionAssert.AreEqual(stringArray, stringArrayDeserialized);
		}

		[Test]
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

		[Test]
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

		[Test]
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

		[Test]
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

		[Test]
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
				{ "intarray", new int[] {1, int.MinValue, 7, int.MaxValue} },
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

		[Test]
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