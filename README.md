# CSharp-AUDALF
.Net Standard compatible [AUDALF](https://github.com/mcraiha/AUDALF) implementation in C#

## Build status
![.NET Core](https://github.com/mcraiha/CSharp-AUDALF/workflows/.NET%20Core/badge.svg)
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/0c9c327249964ce2a2fd9bc66a69b3eb)](https://www.codacy.com/gh/mcraiha/CSharp-AUDALF/dashboard?utm_source=github.com&utm_medium=referral&utm_content=mcraiha/CSharp-AUDALF&utm_campaign=Badge_Coverage)

## Why?

Because I needed this for my personal project

## How do I use this?

Either copy the [AUDALF-deserialize.cs](src/AUDALF-deserialize.cs), [AUDALF-serialize.cs](src/AUDALF-serialize.cs) and [Common.cs](src/Common.cs) files to your project or use [nuget package](https://www.nuget.org/packages/LibAUDALF/) 

Then do code like
```csharp
using CSharp_AUDALF;

Dictionary<string, string> someDictionary = new Dictionary<string, string>()
{
    { "one", "value is here"},
    { "thirteen", "important data here" }
};

// Serialize
byte[] serializedBytes = AUDALF_Serialize.Serialize(someDictionary);

// Deserialize
Dictionary<string, string> fromBytes = AUDALF_Deserialize.Deserialize<string, string>(serializedBytes);

// Or deserialize single value
string deserializedString = AUDALF_Deserialize.DeserializeSingleValue<string,string>(serializedBytes, "one");
```

## What types are currently supported

- byte (single value and array)
- ushort (single value and array)
- uint (single value and array)
- ulong (single value and array)
- sbyte
- short (single value and array)
- int (single value and array) 
- long (single value and array) 
- float
- double
- string
- boolean
- BigInteger
- datetime
- datetimeoffset

## Test cases

You can run test cases by moving to **tests** folder and running following command
```bash
dotnet test
```

## License

All the code is licensed under [Unlicense](LICENSE)