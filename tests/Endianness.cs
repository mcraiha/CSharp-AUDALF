using NUnit.Framework;
using System;

namespace Tests
{
	public class EndiannessTests
	{
		[SetUp]
		public void Setup()
		{

		}

		[Test]
		public void CheckEndianness()
		{
			if (!BitConverter.IsLittleEndian)
			{
				Assert.Fail("Tests only work in little endian system!");
			}

			Assert.Pass();
		}
	}
}