using System;
using System.Collections.Generic;

namespace CSharp_AUDALF
{
	public static class Definitions
	{
		public static readonly byte[] fourCC = new byte[4] { 0x41, 0x55, 0x44, 0x41 };

		public static readonly byte[] versionNumber = new byte[4] { 0x01, 0x00, 0x00, 0x00 };

		public static readonly byte[] payloadSizePlaceholder = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

		public static readonly byte[] specialType = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

		#region Unsigned integer

		public static readonly byte[] unsigned_8_bit_integerType = new byte[8] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
		public static readonly byte[] unsigned_16_bit_integerType = new byte[8] { 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] unsigned_32_bit_integerType = new byte[8] { 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] unsigned_64_bit_integerType = new byte[8] { 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] unsigned_128_bit_integerType = new byte[8] { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] unsigned_256_bit_integerType = new byte[8] { 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] unsigned_512_bit_integerType = new byte[8] { 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] unsigned_1024_bit_integerType = new byte[8] { 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] unsigned_2048_bit_integerType = new byte[8] { 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
		public static readonly byte[] unsigned_4096_bit_integerType = new byte[8] { 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; 

		#endregion // Unsigned integer

		#region Signed integer

		public static readonly byte[] signed_8_bit_integerType = new byte[8] { 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 };
		public static readonly byte[] signed_16_bit_integerType = new byte[8] { 0x02, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] signed_32_bit_integerType = new byte[8] { 0x03, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] signed_64_bit_integerType = new byte[8] { 0x04, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] signed_128_bit_integerType = new byte[8] { 0x05, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] signed_256_bit_integerType = new byte[8] { 0x06, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] signed_512_bit_integerType = new byte[8] { 0x07, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] signed_1024_bit_integerType = new byte[8] { 0x08, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 }; 
		public static readonly byte[] signed_2048_bit_integerType = new byte[8] { 0x09, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 };
		public static readonly byte[] signed_4096_bit_integerType = new byte[8] { 0x0A, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 }; 

		#endregion // Signed integer

		#region Floating points

		public static readonly byte[] floating_point_8_bit = new byte[8] { 0x01, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00 };

		public static readonly byte[] floating_point_16_bit = new byte[8] { 0x02, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00 };

		public static readonly byte[] floating_point_32_bit = new byte[8] { 0x03, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00 };

		public static readonly byte[] floating_point_64_bit = new byte[8] { 0x04, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00 };

		public static readonly byte[] floating_point_128_bit = new byte[8] { 0x05, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00 };

		public static readonly byte[] floating_point_256_bit = new byte[8] { 0x06, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00 };

		public static readonly byte[] floating_point_512_bit = new byte[8] { 0x07, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00 };

		#endregion // Floating points


		#region Strings

		public static readonly byte[] string_ascii = new byte[8] { 0x01, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00 };

		public static readonly byte[] string_utf8 = new byte[8] { 0x02, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00 };

		public static readonly byte[] string_utf16 = new byte[8] { 0x03, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00 };

		public static readonly byte[] string_utf32 = new byte[8] { 0x04, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00 };

		#endregion // Strings


		#region Known offsets

		public static readonly int fourCCOffset = 0;
		public static readonly int versionOffset = 4;

		public static readonly int payloadSizeOffset = 8;
		public static readonly int indexCountOffset = 16;
		public static readonly int keyTypeOffset = 24;
		public static readonly int entryDefinitionsOffset = 32;

		#endregion // Known offsets


		#region Types to types pairings

		private static Dictionary<Type, byte[]> dotnetTypeToAUDALF = new Dictionary<Type, byte[]>();

		static Definitions()
		{
			dotnetTypeToAUDALF.Add(typeof(int), signed_32_bit_integerType);
			dotnetTypeToAUDALF.Add(typeof(float), floating_point_32_bit);
			dotnetTypeToAUDALF.Add(typeof(string), string_utf8);
		}

		public static byte[] GetAUDALFtypeWithDotnetType(Type type)
		{
			return dotnetTypeToAUDALF[type];
		}

		#endregion // Types to types pairings


		#region Common math

		public static ulong NextDivisableBy8(ulong current)
		{
			ulong bits = current & 7;
    		if (bits == 0) return current;
    		return current + (8-bits);
		}

		#endregion
	}
}
