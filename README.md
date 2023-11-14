## Overview
**ThrowAway** is a C# library designed to elegantly handle the dichotomy between successful outcomes **(Values)** and error states **(Failures)** in method returns. 

The library leverages the functional concept of **Option**, enabling the return of an `Option` from a method that can either be matched, mapped, filtered, transposed, etc. or implicitly type cast to the underlying **Value**.

## Key Features
- **Dual Outcomes**: Facilitates methods to return either a Value or a Failure, encapsulated in `Option` types.
- **Seamless Type Casting**: Implicit type casting of `Option` with a value to its underlying type; throws an exception if it's a Failure, which can be caught and handled elsewhere.
- **No Nulls Allowed**: Options are struct-based, ensuring an instance can never be null. Disallows nulls for both Value and Failure, enhancing type safety and predictability.

## Installation

Use the following NuGet command:

```shell
dotnet add package ThrowAway
```

## Basic examples
**Utilize Option Types**:
```csharp
var successfulOutcome = Option.Some(5);
var errorOutcome = Option<int>.Fail("Error occurred");
```
**Implicit Returns in Methods**:

In this example, imagine a scenario where a method calculates the area of a rectangle. If either of the dimensions is non-positive, it returns a failure message; otherwise, it calculates the area.
```csharp
public Option<double> CalculateArea(double length, double width)
{
    if (length <= 0 || width <= 0) 
        return "Invalid dimensions provided";

    return length * width;
}

var areaResult = CalculateArea(10, 5);
if (areaResult.HasFailed)
{
    var failureMessage = areaResult.Failure; // "Invalid dimensions provided"
}
else
{
    var area = areaResult.Value; // 50 (Area of the rectangle)
}

```

**Exception Handling**:

Demonstrates the ProcessAreaCalculation method where input strings are safely parsed and area calculation is performed. 
```csharp
public Option<int, (AreaError, string)> ProcessAreaCalculation(string lengthInput, string widthInput)
{
    if (!double.TryParse(lengthInput, out double length))
        return (AreaError.InvalidInput, $"Invalid length: {lengthInput}");

    if (!double.TryParse(widthInput, out double width))
        return (AreaError.InvalidInput, $"Invalid width: {widthInput}");

    return CalculateArea(length, width).Match(
        area => area > 50 ? 1 : 0, // Check if area is greater than 50
        failure => (AreaError.CalculationError, failure) // "Invalid dimensions provided"
    );
}

```
Check for failures and either process the successful result or handle the error, extracting the error type and message
```csharp
var processResult = ProcessAreaCalculation("10", "6");
if (processResult.HasFailed)
{
    var (errorType, errorMessage) = processResult.Failure;
    // Handle error based on errorType and errorMessage
}
else
{
    var isAreaAboveThreshold = processResult.Value; // 1 if area > 50, else 0
    // continue with the calculations
}
```

Or implicitly type convert the Value to int and use a try-catch block to handle potential failures from ProcessAreaCalculation. In case of a failure the exception details contain the Failure value."


```csharp
try
{
    int isAreaAboveThreshold = ProcessAreaCalculation("10", "6");
    // continue with the calculations
}
catch (HasFailedException ex)
{
    var (errorType, errorMessage) = ex.Failure;
    // Handle error based on errorType and errorMessage
}
```
The try catch (or Option.CatchFailure()) can be as many layers down in the code as you want, the exception will fall thru all the way and still contain the Failure value.

## Detailed Explanation
"ThrowAway" introduces `Option<V>` and `Option<V, F>` structures, with `V` representing the Value type and `F` the Failure type (defaulted to string in `Option<V>`). This design allows methods to return either a value or a reason for failure in a type-safe manner. 

Explicit type casting of an `Option` to its value type simplifies usage, and in cases of failure, it throws a catchable exception. This functionality provides a blend of functional and imperative programming, allowing failures to be handled at the appropriate level in the call stack.


## License
"ThrowAway" is licensed under the MIT License. This permissive license allows free usage, modification, and distribution, making it suitable for both open-source and commercial applications.