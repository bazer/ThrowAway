## Overview
"ThrowAway" is a C# library designed to elegantly handle the dichotomy between successful outcomes (Values) and failures in method returns. It empowers developers to return either a Value or a Failure from methods, streamlining error checks and main execution paths. The library leverages the concept of Options, enabling the return of an entire `Option` from a method, reflecting a functional programming approach integrated into the C# ecosystem.

## Key Features
- **Dual Outcomes**: Facilitates methods to return either a Value or a Failure, encapsulated in `Option` types.
- **Seamless Type Casting**: Implicit type casting of `Option` with a value to its underlying type; throws an exception if it's a Failure, which can be caught and handled elsewhere.
- **Robust Error Handling**: `Option.Catch` methods for nuanced exception management.
- **No Nulls Allowed**: Strictly typed to disallow nulls for both Value and Failure, enhancing type safety and predictability.
- **Struct-Based**: Options are struct-based, ensuring an instance can never be null.

## Quick Start Guide
1. **Install ThrowAway**: 

Use the following NuGet command:

```shell
dotnet add package ThrowAway
```
2. **Utilize Option Types**:
```csharp
var successfulOutcome = Option.Some(5);
var errorOutcome = Option<int>.Fail("Error occurred");
```
3. **Handle Returns in Methods**:
```csharp
public Option<int> PerformCalculation()
{
    if (errorCondition) 
        return "Error while calculating";

    // Main logic

    return calculatedValue;
}

var result = PerformCalculation();

if (result.HasFailed)
{
    var failure = result.Failure;
}
else
{
    var value = result.Value;
}

```
4. **Exception Handling**:
```csharp
public Option<string> GetMessage()
{
    int result = PerformCalculation(); // Throws HasFailedException if Option is a failure.

    if (result >= 0)
        return "Positive result";
    else
        return "Negative result";
}

var message = Option.Catch(() => GetMessage()); // Returns the Option with the value, or catches the HasFailedException and returns an Option with the failure

if (message.HasFailed)
{
    var failure = message.Failure; // "Error while calculating"
}
else
{
    var message = message.Value; // "Positive result" or "Negative result"
}
```

 or

```csharp
try
{
    int result = PerformCalculation();
    
    // "Positive result" or "Negative result"
}
catch (HasFailedException ex)
{
    var failure = ex.Failure; // "Error while calculating"
}
```

## Detailed Explanation
"ThrowAway" introduces `Option<V>` and `Option<V, F>` structures, with `V` representing the Value type and `F` the Failure type (defaulted to string in `Option<V>`). This design allows methods to return either a value or a reason for failure in a type-safe manner. Explicit type casting of an `Option` to its value type simplifies usage, and in cases of failure, it throws a catchable exception. This functionality provides a blend of functional and imperative programming, allowing failures to be handled at the appropriate level in the call stack.

## License
"ThrowAway" is licensed under the MIT License. This permissive license allows free usage, modification, and distribution, making it suitable for both open-source and commercial applications.