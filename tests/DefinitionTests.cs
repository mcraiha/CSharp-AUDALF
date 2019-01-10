using NUnit.Framework;
using System;
using System.Text;
using CSharp_AUDALF;

namespace Tests
{
	public class DefinitionTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void FourCCTest()
		{
			// Arrange
			string fourCC = "AUDA";

			// Act
			byte[] fourCCAsBytes = Encoding.ASCII.GetBytes(fourCC);

			// Assert
			CollectionAssert.AreEqual(Definitions.fourCC, fourCC);
		}

		[Test]
		public void SpecialTypeTest()
		{
			// Arrange
			ulong expected = 0;

			// Act
			ulong valueGotFromBytes = BitConverter.ToUInt64(Definitions.specialType, 0);

			// Assert
			Assert.AreEqual(expected, valueGotFromBytes);
		}

		[Test]
		public void UnsignedIntergersTest()
		{
			// Arrange
			ulong expected8bit = 1;
			ulong expected16bit = 2;
			ulong expected32bit = 3;
			ulong expected64bit = 4;
			ulong expected128bit = 5;
			ulong expected256bit = 6;
			ulong expected512bit = 7;
			ulong expected1024bit = 8;
			ulong expected2048bit = 9;
			ulong expected4096bit = 10;

			// Act
			ulong result8bit = BitConverter.ToUInt64(Definitions.unsigned_8_bit_integerType, 0);
			ulong result16bit = BitConverter.ToUInt64(Definitions.unsigned_16_bit_integerType, 0);
			ulong result32bit = BitConverter.ToUInt64(Definitions.unsigned_32_bit_integerType, 0);
			ulong result64bit = BitConverter.ToUInt64(Definitions.unsigned_64_bit_integerType, 0);
			ulong result128bit = BitConverter.ToUInt64(Definitions.unsigned_128_bit_integerType, 0);
			ulong result256bit = BitConverter.ToUInt64(Definitions.unsigned_256_bit_integerType, 0);
			ulong result512bit = BitConverter.ToUInt64(Definitions.unsigned_512_bit_integerType, 0);
			ulong result1024bit = BitConverter.ToUInt64(Definitions.unsigned_1024_bit_integerType, 0);
			ulong result2048bit = BitConverter.ToUInt64(Definitions.unsigned_2048_bit_integerType, 0);
			ulong result4096bit = BitConverter.ToUInt64(Definitions.unsigned_4096_bit_integerType, 0);

			// Assert
			Assert.AreEqual(expected8bit, result8bit);
			Assert.AreEqual(expected16bit, result16bit);
			Assert.AreEqual(expected32bit, result32bit);
			Assert.AreEqual(expected64bit, result64bit);
			Assert.AreEqual(expected128bit, result128bit);
			Assert.AreEqual(expected256bit, result256bit);
			Assert.AreEqual(expected512bit, result512bit);
			Assert.AreEqual(expected1024bit, result1024bit);
			Assert.AreEqual(expected2048bit, result2048bit);
			Assert.AreEqual(expected4096bit, result4096bit);
		}

		[Test]
		public void SignedIntergersTest()
		{
			// Arrange
			ulong expected8bit = 16777217;
			ulong expected16bit = 16777218;
			ulong expected32bit = 16777219;
			ulong expected64bit = 16777220;
			ulong expected128bit = 16777221;
			ulong expected256bit = 16777222;
			ulong expected512bit = 16777223;
			ulong expected1024bit = 16777224;
			ulong expected2048bit = 16777225;
			ulong expected4096bit = 16777226;

			// Act
			ulong result8bit = BitConverter.ToUInt64(Definitions.signed_8_bit_integerType, 0);
			ulong result16bit = BitConverter.ToUInt64(Definitions.signed_16_bit_integerType, 0);
			ulong result32bit = BitConverter.ToUInt64(Definitions.signed_32_bit_integerType, 0);
			ulong result64bit = BitConverter.ToUInt64(Definitions.signed_64_bit_integerType, 0);
			ulong result128bit = BitConverter.ToUInt64(Definitions.signed_128_bit_integerType, 0);
			ulong result256bit = BitConverter.ToUInt64(Definitions.signed_256_bit_integerType, 0);
			ulong result512bit = BitConverter.ToUInt64(Definitions.signed_512_bit_integerType, 0);
			ulong result1024bit = BitConverter.ToUInt64(Definitions.signed_1024_bit_integerType, 0);
			ulong result2048bit = BitConverter.ToUInt64(Definitions.signed_2048_bit_integerType, 0);
			ulong result4096bit = BitConverter.ToUInt64(Definitions.signed_4096_bit_integerType, 0);

			// Assert
			Assert.AreEqual(expected8bit, result8bit);
			Assert.AreEqual(expected16bit, result16bit);
			Assert.AreEqual(expected32bit, result32bit);
			Assert.AreEqual(expected64bit, result64bit);
			Assert.AreEqual(expected128bit, result128bit);
			Assert.AreEqual(expected256bit, result256bit);
			Assert.AreEqual(expected512bit, result512bit);
			Assert.AreEqual(expected1024bit, result1024bit);
			Assert.AreEqual(expected2048bit, result2048bit);
			Assert.AreEqual(expected4096bit, result4096bit);
		}

		[Test]
		public void FloatingPointsTest()
		{
			// Arrange
			ulong expected8bit = 33554433;
			ulong expected16bit = 33554434;
			ulong expected32bit = 33554435;
			ulong expected64bit = 33554436;
			ulong expected128bit = 33554437;
			ulong expected256bit = 33554438;
			ulong expected512bit = 33554439;

			// Act
			ulong result8bit = BitConverter.ToUInt64(Definitions.floating_point_8_bit, 0);
			ulong result16bit = BitConverter.ToUInt64(Definitions.floating_point_16_bit, 0);
			ulong result32bit = BitConverter.ToUInt64(Definitions.floating_point_32_bit, 0);
			ulong result64bit = BitConverter.ToUInt64(Definitions.floating_point_64_bit, 0);
			ulong result128bit = BitConverter.ToUInt64(Definitions.floating_point_128_bit, 0);
			ulong result256bit = BitConverter.ToUInt64(Definitions.floating_point_256_bit, 0);
			ulong result512bit = BitConverter.ToUInt64(Definitions.floating_point_512_bit, 0);


			// Assert
			Assert.AreEqual(expected8bit, result8bit);
			Assert.AreEqual(expected16bit, result16bit);
			Assert.AreEqual(expected32bit, result32bit);
			Assert.AreEqual(expected64bit, result64bit);
			Assert.AreEqual(expected128bit, result128bit);
			Assert.AreEqual(expected256bit, result256bit);
			Assert.AreEqual(expected512bit, result512bit);
		}

		[Test]
		public void StringsTest()
		{
			// Arrange
			ulong expectedAscii = 83886081;
			ulong expectedUtf8 = 83886082;
			ulong expectedUtf16 = 83886083;
			ulong expectedUtf32 = 83886084;

			// Act
			ulong resultAscii = BitConverter.ToUInt64(Definitions.string_ascii, 0);
			ulong resultUtf8 = BitConverter.ToUInt64(Definitions.string_utf8, 0);
			ulong resultUtf16 = BitConverter.ToUInt64(Definitions.string_utf16, 0);
			ulong resultUtf32 = BitConverter.ToUInt64(Definitions.string_utf32, 0);

			// Assert
			Assert.AreEqual(expectedAscii, resultAscii);
			Assert.AreEqual(expectedUtf8, resultUtf8);
			Assert.AreEqual(expectedUtf16, resultUtf16);
			Assert.AreEqual(expectedUtf32, resultUtf32);
		}

		[Test]
		public void BooleanTest()
		{
			// Arrange
			ulong expectedBoolean = 100663297;

			// Act
			ulong resultBoolean = BitConverter.ToUInt64(Definitions.booleans, 0);

			// Assert
			Assert.AreEqual(expectedBoolean, resultBoolean);
		}

		[Test]
		public void DateTimesTest()
		{
			// Arrange
			ulong expectedUnixSeconds = 117440513;
			ulong expectedUnixMilliseconds = 117440514;
			ulong expectedISO8601 = 117440515;

			// Act
			ulong resultUnixSeconds = BitConverter.ToUInt64(Definitions.datetime_unix_seconds, 0);
			ulong resultUnixMilliseconds = BitConverter.ToUInt64(Definitions.datetime_unix_milliseconds, 0);
			ulong resultISO8601 = BitConverter.ToUInt64(Definitions.datetime_iso_8601, 0);

			// Assert
			Assert.AreEqual(expectedUnixSeconds, resultUnixSeconds);
			Assert.AreEqual(expectedUnixMilliseconds, resultUnixMilliseconds);
			Assert.AreEqual(expectedISO8601, resultISO8601);
		}
	}
}