using System;
using System.Collections.Generic;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Numerics;
using System.Linq;

namespace CSharp_AUDALF;

/// <summary>
/// Date time format
/// </summary>
public enum DateTimeFormat
{
	/// <summary>
	/// ISO8601, Default value
	/// </summary>
	ISO8601 = 0,

	/// <summary>
	/// Unix milliseconds
	/// </summary>
	UnixInMilliseconds,

	/// <summary>
	/// Unix seconds
	/// </summary>
	UnixInSeconds
}

/// <summary>
/// Serialization settings
/// </summary>
public class SerializationSettings
{
	/// <summary>
	/// Date time format for serialization
	/// </summary>
	public DateTimeFormat dateTimeFormat;
}

/// <summary>
/// Deserialization Settings
/// </summary>
public class DeserializationSettings
{
	/// <summary>
	/// Wanted time type for deserialization
	/// </summary>
	public required Type wantedDateTimeType;
}

/// <summary>
/// Definitons that are static
/// </summary>
public static class Definitions
{
	#region Errors

	private static readonly string CannotParseTypeError = "Cannot parse type!";

	#endregion // Errors

	/// <summary>
	/// How much space (in bytes) fourCC takes
	/// </summary>
	public const int fourCCSize = 4;

	/// <summary>
	/// FourCC identifier as byte array
	/// </summary>
	/// <value>AUDA aka 0x41, 0x55, 0x44, 0x41</value>
	public static readonly ImmutableArray<byte> fourCC = [0x41, 0x55, 0x44, 0x41];

	/// <summary>
	/// Version number of current AUDALF
	/// </summary>
	/// <value>1 aka 0x01, 0x00, 0x00, 0x00</value>
	public static readonly ImmutableArray<byte> versionNumber = [0x01, 0x00, 0x00, 0x00];

	/// <summary>
	/// Payload size placeholder
	/// </summary>
	/// <value>0</value>
	public static readonly ImmutableArray<byte> payloadSizePlaceholder = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Special type byte array. Reserved for special cases
	/// </summary>
	/// <value>0</value>
	public static readonly ImmutableArray<byte> specialType = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];


	#region Unsigned integer

	/// <summary>
	/// Unsigned 8 bit integer, equals byte, range [0 .. 255]
	/// </summary>
	/// <value>1</value>
	public static readonly ImmutableArray<byte> unsigned_8_bit_integerType = [0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Unsigned 16 bit integer, range [0 .. 65535]
	/// </summary>
	/// <value>2</value>
	public static readonly ImmutableArray<byte> unsigned_16_bit_integerType = [0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Unsigned 32 bit integer, range [0 .. 4294967295]
	/// </summary>
	/// <value>3</value>
	public static readonly ImmutableArray<byte> unsigned_32_bit_integerType = [0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Unsigned 64 bit integer, range [0 .. 18446744073709551615]
	/// </summary>
	/// <value>4</value>
	public static readonly ImmutableArray<byte> unsigned_64_bit_integerType = [0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Unsigned 128 bit integer, range [0 .. 2^128 − 1]
	/// </summary>
	/// <value>5</value>
	public static readonly ImmutableArray<byte> unsigned_128_bit_integerType = [0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Unsigned 256 bit integer, range [0 .. 2^256 − 1]
	/// </summary>
	/// <value>6</value>
	public static readonly ImmutableArray<byte> unsigned_256_bit_integerType = [0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Unsigned 512 bit integer, range [0 .. 2^512 − 1]
	/// </summary>
	/// <value>7</value>
	public static readonly ImmutableArray<byte> unsigned_512_bit_integerType = [0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Unsigned 1024 bit integer, range [0 .. 2^1024 − 1]
	/// </summary>
	/// <value>8</value>
	public static readonly ImmutableArray<byte> unsigned_1024_bit_integerType = [0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Unsigned 2048 bit integer, range [0 .. 2^2048 − 1]
	/// </summary>
	/// <value>9</value>
	public static readonly ImmutableArray<byte> unsigned_2048_bit_integerType = [0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Unsigned 4096 bit integer, range [0 .. 2^4096 − 1]
	/// </summary>
	/// <value>10</value>
	public static readonly ImmutableArray<byte> unsigned_4096_bit_integerType = [0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

	#endregion // Unsigned integer

	#region Unsigned integer array

	/// <summary>
	/// Array of unsigned 8 bit integers
	/// </summary>
	/// <value>65537</value>
	public static readonly ImmutableArray<byte> unsigned_8_bit_integerArrayType = [0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of unsigned 16 bit integers
	/// </summary>
	/// <value>65538</value>
	public static readonly ImmutableArray<byte> unsigned_16_bit_integerArrayType = [0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of unsigned 32 bit integers
	/// </summary>
	/// <value>65539</value>
	public static readonly ImmutableArray<byte> unsigned_32_bit_integerArrayType = [0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of unsigned 64 bit integers
	/// </summary>
	/// <value>65540</value>
	public static readonly ImmutableArray<byte> unsigned_64_bit_integerArrayType = [0x04, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of unsigned 128 bit integers
	/// </summary>
	/// <value>65541</value>
	public static readonly ImmutableArray<byte> unsigned_128_bit_integerArrayType = [0x05, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of unsigned 256 bit integers
	/// </summary>
	/// <value>65542</value>
	public static readonly ImmutableArray<byte> unsigned_256_bit_integerArrayType = [0x06, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of unsigned 512 bit integers
	/// </summary>
	/// <value>65543</value>
	public static readonly ImmutableArray<byte> unsigned_512_bit_integerArrayType = [0x07, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of unsigned 1024 bit integers
	/// </summary>
	/// <value>65544</value>
	public static readonly ImmutableArray<byte> unsigned_1024_bit_integerArrayType = [0x08, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of unsigned 2048 bit integers
	/// </summary>
	/// <value>65545</value>
	public static readonly ImmutableArray<byte> unsigned_2048_bit_integerArrayType = [0x09, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of unsigned 4096 bit integers
	/// </summary>
	/// <value>65546</value>
	public static readonly ImmutableArray<byte> unsigned_4096_bit_integerArrayType = [0x0A, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00];

	#endregion // Unsigned integer array

	#region Signed integer

	/// <summary>
	/// Signed 8 bit integer, range [-128 .. 127]
	/// </summary>
	/// <value>16777217</value>
	public static readonly ImmutableArray<byte> signed_8_bit_integerType = [0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Signed 16 bit integer, range [-32768 .. 32767]
	/// </summary>
	/// <value>16777218</value>
	public static readonly ImmutableArray<byte> signed_16_bit_integerType = [0x02, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Signed 32 bit integer, range [-2147483648 .. 2 147 483 647]
	/// </summary>
	/// <value>16777219</value>
	public static readonly ImmutableArray<byte> signed_32_bit_integerType = [0x03, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Signed 64 bit integer, range [-9 223 372 036 854 775 808 .. 9 223 372 036 854 775 807]
	/// </summary>
	/// <value>16777220</value>
	public static readonly ImmutableArray<byte> signed_64_bit_integerType = [0x04, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Signed 128 bit integer, range [-2^127 .. 2^127 − 1]
	/// </summary>
	/// <value>16777221</value>
	public static readonly ImmutableArray<byte> signed_128_bit_integerType = [0x05, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Signed 256 bit integer, range [-2^255 .. 2^255 − 1]
	/// </summary>
	/// <value>16777222</value>
	public static readonly ImmutableArray<byte> signed_256_bit_integerType = [0x06, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Signed 512 bit integer, range [-2^511 .. 2^511 − 1]
	/// </summary>
	/// <value>16777223</value>
	public static readonly ImmutableArray<byte> signed_512_bit_integerType = [0x07, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Signed 1024 bit integer, range [-2^1023 .. 2^1023 − 1]
	/// </summary>
	/// <value>16777224</value>
	public static readonly ImmutableArray<byte> signed_1024_bit_integerType = [0x08, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Signed 2048 bit integer, range [-2^2047 .. 2^2047 − 1]
	/// </summary>
	/// <value>16777225</value>
	public static readonly ImmutableArray<byte> signed_2048_bit_integerType = [0x09, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Signed 4096 bit integer, range [-2^4095 .. 2^4095 − 1]
	/// </summary>
	/// <value>16777226</value>
	public static readonly ImmutableArray<byte> signed_4096_bit_integerType = [0x0A, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00];

	#endregion // Signed integer


	#region Signed integer array

	/// <summary>
	/// Array of signed 8 bit integers
	/// </summary>
	/// <value>16842753</value>
	public static readonly ImmutableArray<byte> signed_8_bit_integerArrayType = [0x01, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of signed 16 bit integers
	/// </summary>
	/// <value>16842754</value>
	public static readonly ImmutableArray<byte> signed_16_bit_integerArrayType = [0x02, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of signed 32 bit integers
	/// </summary>
	/// <value>16842755</value>
	public static readonly ImmutableArray<byte> signed_32_bit_integerArrayType = [0x03, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of signed 64 bit integers
	/// </summary>
	/// <value>16842756</value>
	public static readonly ImmutableArray<byte> signed_64_bit_integerArrayType = [0x04, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of signed 128 bit integers
	/// </summary>
	/// <value>16842757</value>
	public static readonly ImmutableArray<byte> signed_128_bit_integerArrayType = [0x05, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of signed 256 bit integers
	/// </summary>
	/// <value>16842758</value>
	public static readonly ImmutableArray<byte> signed_256_bit_integerArrayType = [0x06, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of signed 512 bit integers
	/// </summary>
	/// <value>16842759</value>
	public static readonly ImmutableArray<byte> signed_512_bit_integerArrayType = [0x07, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of signed 1024 bit integers
	/// </summary>
	/// <value>16842760</value>
	public static readonly ImmutableArray<byte> signed_1024_bit_integerArrayType = [0x08, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of signed 2048 bit integers
	/// </summary>
	/// <value>16842761</value>
	public static readonly ImmutableArray<byte> signed_2048_bit_integerArrayType = [0x09, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Array of signed 4096 bit integers
	/// </summary>
	/// <value>16842762</value>
	public static readonly ImmutableArray<byte> signed_4096_bit_integerArrayType = [0x0A, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00];

	#endregion // Signed integer array


	#region Floating points

	/// <summary>
	/// 8 bit floating point format
	/// </summary>
	/// <value>33554433</value>
	public static readonly ImmutableArray<byte> floating_point_8_bit = [0x01, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// 16 bit floating point format, binary16 from IEEE 754
	/// </summary>
	/// <value>33554434</value>
	public static readonly ImmutableArray<byte> floating_point_16_bit = [0x02, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// 32 bit floating point format, binary32 from IEEE 754
	/// </summary>
	/// <value>33554435</value>
	public static readonly ImmutableArray<byte> floating_point_32_bit = [0x03, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// 64 bit floating point format, binary64 from IEEE 754
	/// </summary>
	/// <value>33554436</value>
	public static readonly ImmutableArray<byte> floating_point_64_bit = [0x04, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// 128 bit floating point format, binary128 from IEEE 754
	/// </summary>
	/// <value>33554437</value>
	public static readonly ImmutableArray<byte> floating_point_128_bit = [0x05, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// 256 bit floating point format, binary256 from IEEE 754
	/// </summary>
	/// <value>33554438</value>
	public static readonly ImmutableArray<byte> floating_point_256_bit = [0x06, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// 512 bit floating point format
	/// </summary>
	/// <value>33554439</value>
	public static readonly ImmutableArray<byte> floating_point_512_bit = [0x07, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00];

	#endregion // Floating points


	#region Floating point array

	/// <summary>
	/// 8 bit floating point format
	/// </summary>
	/// <value>33619969</value>
	public static readonly ImmutableArray<byte> floating_point_8_bitArrayType = [0x01, 0x00, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// 16 bit floating point format, binary16 from IEEE 754
	/// </summary>
	/// <value>33619970</value>
	public static readonly ImmutableArray<byte> floating_point_16_bitArrayType = [0x02, 0x00, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// 32 bit floating point format, binary32 from IEEE 754
	/// </summary>
	/// <value>33619971</value>
	public static readonly ImmutableArray<byte> floating_point_32_bitArrayType = [0x03, 0x00, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// 64 bit floating point format, binary64 from IEEE 754
	/// </summary>
	/// <value>33619972</value>
	public static readonly ImmutableArray<byte> floating_point_64_bitArrayType = [0x04, 0x00, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// 128 bit floating point format, binary128 from IEEE 754
	/// </summary>
	/// <value>33619973</value>
	public static readonly ImmutableArray<byte> floating_point_128_bitArrayType = [0x05, 0x00, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// 256 bit floating point format, binary256 from IEEE 754
	/// </summary>
	/// <value>33619974</value>
	public static readonly ImmutableArray<byte> floating_point_256_bitArrayType = [0x06, 0x00, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// 512 bit floating point format
	/// </summary>
	/// <value>33619975</value>
	public static readonly ImmutableArray<byte> floating_point_512_bitArrayType = [0x07, 0x00, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00];

	#endregion // Floating points


	#region Strings

	/// <summary>
	/// ASCII string
	/// </summary>
	/// <value>83886081</value>
	public static readonly ImmutableArray<byte> string_ascii = [0x01, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// UTF-8 Unicode string
	/// </summary>
	/// <value>83886082</value>
	public static readonly ImmutableArray<byte> string_utf8 = [0x02, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// UTF-16 Unicode string
	/// </summary>
	/// <value>83886083</value>
	public static readonly ImmutableArray<byte> string_utf16 = [0x03, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// UTF-32 Unicode string
	/// </summary>
	/// <value>83886084</value>
	public static readonly ImmutableArray<byte> string_utf32 = [0x04, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00];

	#endregion // Strings

	#region Booleans

	/// <summary>
	/// Common boolean. It is either True (1) or False (0)
	/// </summary>
	/// <value>100663297</value>
	public static readonly ImmutableArray<byte> booleans = [0x01, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00];

	#endregion // Booleans

	#region Date / time 

	/// <summary>
	/// Aka POSIX time and UNIX Epoch time in seconds, as 64 bit unsigned integer
	/// </summary>
	/// <value>117440513</value>
	public static readonly ImmutableArray<byte> datetime_unix_seconds = [0x01, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// Aka POSIX time and UNIX Epoch time in milliseconds, as 64 bit unsigned integer
	/// </summary>
	/// <value>117440514</value>
	public static readonly ImmutableArray<byte> datetime_unix_milliseconds = [0x02, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00];

	/// <summary>
	/// ISO 8601, as UTF-8 string
	/// </summary>
	/// <value>117440515</value>
	public static readonly ImmutableArray<byte> datetime_iso_8601 = [0x03, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00];

	#endregion // Date / time 

	#region Arbitrarily large signed integer

	/// <summary>
	/// Signed integer without range limits	
	/// </summary>
	/// <value>134217729</value>
	public static readonly ImmutableArray<byte> bigIntegerType = [0x01, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00];

	#endregion // Arbitrarily large signed integer

	#region Known offsets

	/// <summary>
	/// Offset for FourCC
	/// </summary>
	public static readonly int fourCCOffset = 0;

	/// <summary>
	/// Offset for version
	/// </summary>
	public static readonly int versionOffset = 4;

	/// <summary>
	/// Offset for payload size
	/// </summary>
	public static readonly int payloadSizeOffset = 8;

	/// <summary>
	/// Offset for index count
	/// </summary>
	public static readonly int indexCountOffset = 16;

	/// <summary>
	/// Offset for key type
	/// </summary>
	public static readonly int keyTypeOffset = 24;

	/// <summary>
	/// Offset for entry definitions
	/// </summary>
	public static readonly int entryDefinitionsOffset = 32;

	#endregion // Known offsets


	#region Types to types pairings

	private static readonly FrozenDictionary<Type, byte[]> dotnetTypeToAUDALF = new Dictionary<Type, byte[]>()
	{
		// Single values
		{ typeof(byte), unsigned_8_bit_integerType.ToArray() },
		{ typeof(ushort), unsigned_16_bit_integerType.ToArray() },
		{ typeof(uint), unsigned_32_bit_integerType.ToArray() },
		{ typeof(ulong), unsigned_64_bit_integerType.ToArray() },

		{ typeof(sbyte), signed_8_bit_integerType.ToArray() },
		{ typeof(short), signed_16_bit_integerType.ToArray() },
		{ typeof(int), signed_32_bit_integerType.ToArray() },
		{ typeof(long), signed_64_bit_integerType.ToArray() },

		{ typeof(Half), floating_point_16_bit.ToArray() },
		{ typeof(float), floating_point_32_bit.ToArray() },
		{ typeof(double), floating_point_64_bit.ToArray() },

		{ typeof(string), string_utf8.ToArray() },

		{ typeof(bool), booleans.ToArray() },

		{ typeof(BigInteger), bigIntegerType.ToArray() },

		// Arrays
		{ typeof(byte[]), unsigned_8_bit_integerArrayType.ToArray() },
		{ typeof(ushort[]), unsigned_16_bit_integerArrayType.ToArray() },
		{ typeof(uint[]), unsigned_32_bit_integerArrayType.ToArray() },
		{ typeof(ulong[]), unsigned_64_bit_integerArrayType.ToArray() },

		{ typeof(sbyte[]), signed_8_bit_integerArrayType.ToArray() },
		{ typeof(short[]), signed_16_bit_integerArrayType.ToArray() },
		{ typeof(int[]), signed_32_bit_integerArrayType.ToArray() },
		{ typeof(long[]), signed_64_bit_integerArrayType.ToArray() },

		{ typeof(Half[]), floating_point_16_bitArrayType.ToArray() },
		{ typeof(float[]), floating_point_32_bitArrayType.ToArray() },
		{ typeof(double[]), floating_point_64_bitArrayType.ToArray() },

	}.ToFrozenDictionary();

	/// <summary>
	/// Get AUDALF type with Dotnet type
	/// </summary>
	/// <param name="type">Dotnet type</param>
	/// <returns>Byte array that contains eight bytes that tell AUDALF type</returns>
	public static byte[] GetAUDALFtypeWithDotnetType(Type type)
	{
		return dotnetTypeToAUDALF[type];
	}

	/// <summary>
	/// Get Dotnet type with AUDALF
	/// </summary>
	/// <param name="audalfTypeBytes">Byte array with 8 AUDALF type bytes</param>
	/// <returns>Type</returns>
	public static Type GetDotnetTypeWithAUDALFtype(ReadOnlySpan<byte> audalfTypeBytes)
	{
		foreach (KeyValuePair<Type, byte[]> entry in dotnetTypeToAUDALF)
		{
			if (ByteArrayCompare(entry.Value, audalfTypeBytes))
			{
				return entry.Key;
			}
		}

		throw new ArgumentException(CannotParseTypeError);
	}

	#endregion // Types to types pairings

	#region Common comparision

	/// <summary>
	/// Compare if two byte arrays are equal
	/// </summary>
	/// <param name="a1">First byte array</param>
	/// <param name="a2">Second byte array</param>
	/// <returns>True if they are equal; False otherwise</returns>
	public static bool ByteArrayCompare(ReadOnlySpan<byte> a1, ReadOnlySpan<byte> a2) 
	{
		return MemoryExtensions.SequenceEqual(a1, a2);
	}

	#endregion

	#region Common math

	/// <summary>
	/// Find next offset that is divisable by 8. E.g. 23 would return 24
	/// </summary>
	/// <param name="current">Current address/offset</param>
	/// <returns>Ulong value</returns>
	public static ulong NextDivisableBy8(ulong current)
	{
		ulong bits = current & 7;
		if (bits == 0) return current;
		return current + (8-bits);
	}

	#endregion
}
