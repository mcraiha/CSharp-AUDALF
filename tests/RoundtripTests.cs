using NUnit.Framework;
using CSharp_AUDALF;
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
			Dictionary<string, string> stringStringDictionaryDeserialized = AUDALF_Deserialize.Deserialize<string, string>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(stringStringDictionaryDeserialized);

			CollectionAssert.AreEqual(stringStringDictionary, stringStringDictionaryDeserialized);
		}
	}
}