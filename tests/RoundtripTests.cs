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
			int[] intArrayDeserialized = AUDALF_Deserialize.Deserialize(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(intArrayDeserialized);

			CollectionAssert.AreEqual(intArray, intArrayDeserialized);
		}

		[Test]
		public void StringArrayRoundtripTest()
		{
			// Arrange
			string[] stringArray = new string[] { "something", null, "🐱" };

			// Act
			byte[] result = AUDALF_Serialize.Serialize(stringArray);
			string[] stringArrayDeserialized = AUDALF_Deserialize.DeserializeStringArray(result);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(stringArrayDeserialized);

			CollectionAssert.AreEqual(stringArray, stringArrayDeserialized);
		}
	}
}