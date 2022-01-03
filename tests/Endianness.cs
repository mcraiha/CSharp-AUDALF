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

		[Test, Description("Make sure tests are run in little endian system")]
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