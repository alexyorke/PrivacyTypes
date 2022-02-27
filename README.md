# PrivacyTypes
A C# WIP library to show how you can prevent accessing PII by restricting which methods can access and read the information during runtime and deserialization.

## Examples

Normally, when an object is deserialized you have access to its properties immediately, even if they are sensitive.
```csharp
// retrieve a person record from the server (which has confidential info)
var unsafePerson = JsonConvert.DeserializeObject<UnsafePerson>(GetPersonJson());
Console.WriteLine(unsafePerson.fullName); // oh no! prints "John Smith", which is bad because this is secret information
```

This presents a few issues:
- Fields can be accidently logged.
- It can be complicated to audit the data flow because the data is mutable and can be easily appended to, encrypted, or modified.

With PrivacyTypes, you can deserialize to a `PrivateType` which allows you to specify manual access levels and puts custom restrictions on sensitive information. In this example, we have deserialized a person to a `SafePerson` object which has `PrivateType`s for its fields.

```csharp
// retrieve a person record from the server (which has confidential info) but this time, let's do it safely
var safePerson = JsonConvert.DeserializeObject<SafePerson>(GetPersonJson());
try
{
  Console.WriteLine(safePerson.fullName); // fails because it is private information based on the type of the model
}
catch (InvalidOperationException)
{
  // ignored
}
```

We're not able to view the full name field because it is considered private.

Also, types can be contaiminated if they are mixed with higher privacy levels. For example,

```csharp
var firstPlusFullName = highlyPrivateString.Concat(safePerson.lastName, context);
```

This will cause `firstPlusFullName` to be a highly private string because we have concatenated a low private string with a high private string.

You can also specify a context where private data is accessed. Note: it is not guaranteed that the GC will clear the memory immediately after the using block, but it prevents using the data accidently.

```csharp
using (var context = new PrivateTypeAuthorizationContext(PrivateTypeAuthorizationContextPrivacyLevel.VERYHIGH)) // prevent accessing secret data by isolating the access level
{
  HighPrivateString contaminatedConcat = highlyPrivateString.Concat(mediumPrivateString, context); // mixing access levels contaminates data on a type level

  logger.Log(contaminatedConcat, context); // contents can only be logged explicitly if it is not private (logger won't log this value because the logger's level is set to LOW)
}
```

### If I can't see the data, how can I do anything with it?

There are a few ways to access the data:
- Use the `__unsafe_get` method on the type to get the contents.
- Implement the `PrivateType<T>` in your class and create a new method to retrieve the data.
- Prefer methods that validate the data internally without passing the raw string to the user.

## How is this different than SecureString?
SecureString aims to encrypt the data in memory and is also [deprecated](https://github.com/dotnet/platform-compat/blob/master/docs/DE0001.md).

PrivacyTypes does not aim to encrypt data in memory because it is intended to be part of a suite of tools for security. It is intended for increasing the auditability of where PII is used and promotes patterns that could make data access safer.

It is still possible to shoot yourself in the foot so-to-speak by calling `__unsafe_get` and logging the fields. However, since most modern IDEs allow you to view references to a function/method, it allows you to easily see where this data is being logged.
