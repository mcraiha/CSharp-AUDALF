# CSharp-AUDALF

.Net 8 compatible [AUDALF](https://github.com/mcraiha/AUDALF) implementation in C#

## How do I use this?

```csharp
using CSharp_AUDALF;

Dictionary<string, string?> someDictionary = new Dictionary<string, string?>()
{
    { "one", "value is here"},
    { "thirteen", "important data here" }
};

// Serialize
byte[] serializedBytes = AUDALF_Serialize.Serialize(someDictionary);

// Deserialize
Dictionary<string, string?> fromBytes = AUDALF_Deserialize.Deserialize<string, string?>(serializedBytes);

// Or deserialize single value
string? deserializedString = AUDALF_Deserialize.DeserializeSingleValue<string, string?>(serializedBytes, "one");
```
