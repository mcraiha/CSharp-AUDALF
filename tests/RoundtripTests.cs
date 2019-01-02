using NUnit.Framework;
using CSharp_AUDALF;

namespace Tests
{
	public class RoundtripTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void IntArrayRoundtripTest()
		{
			// Arrange
			int[] intArray = new int[] { 1, 10, 100, 1000 };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(intArray);
			int[] intArrayDeserialized = AUDALF_Deserialize.Deserialize<int>(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(intArrayDeserialized);

			CollectionAssert.AreEqual(intArray, intArrayDeserialized);
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
	}
}