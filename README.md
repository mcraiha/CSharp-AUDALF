# CSharp-AUDALF
.Net Standard compatible [AUDALF](https://github.com/mcraiha/AUDALF) implementation in C#

## Build status
[![Build Status](https://travis-ci.org/mcraiha/CSharp-AUDALF.svg?branch=master)](https://travis-ci.org/mcraiha/CSharp-AUDALF)

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

```

## What types are currently supported

byte (single value and array), int (single value and array), float, string, datetime

## Test cases

You can run test cases by moving to **tests** folder and running following command
```bash
dotnet test
```

## License

All the code is licensed under [Unlicense](LICENSE)