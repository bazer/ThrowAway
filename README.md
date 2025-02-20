# ThrowAway

**ThrowAway** is a pragmatic and efficient C# library that marries functional programming concepts with traditional procedural error handling. It is designed to elegantly manage the dichotomy between successful outcomes (`Values`) and error states (`Failures`) by leveraging the power of the **Option** type. This hybrid approach makes it easy to write expressive, functional code while still integrating seamlessly with conventional try-catch exception handling.

## Overview

In many C# applications, error handling is either bogged down by traditional exception logic or forced into a rigid functional style. ThrowAway bridges these two worlds by offering:

- **Explicit Functional Handling:** Model success and failure as first-class citizens with Option types.
- **Seamless Procedural Integration:** Implicit conversions let you work with Options as if they were plain values - while automatically throwing a `HasFailedException` when an Option is in a failed state. This allows you to mix functional transformations with traditional try-catch error management.

## Key Features

- **Dual Outcomes:** Every operation returns an Option that encapsulates either a valid result (`Value`) or an error (`Failure`).
- **Implicit Conversions:** Options automatically convert to their underlying type. When an Option represents a failure, an exception is thrown, enabling smooth integration with procedural exception handling.
- **Rich Functional API:** A suite of extension methods (like `Map`, `FlatMap`, `Match`, `Filter`, `Transpose`, and `Use`) enables chaining and composing operations in a functional style.
- **No Nulls:** Being struct-based, Options guarantee non-null instances, improving type safety.
- **Hybrid Error Handling:** Combines the explicitness of functional error handling with the convenience of procedural exception handling.
- **Robust Exception Wrapping:** Methods like `CatchFailure` and `CatchAll` convert exceptions into failed Options, maintaining consistency in error handling.
- **Seamless Unwrapping:** `TryUnwrap` lets you extract both the value and the failure message without additional boilerplate.

## Two Flavors: Option (Maybe) vs. Option (Either)

ThrowAway provides two variants of the Option type:

1. **Option (Maybe)**  
   This variant fixes the failure type to a string. It's ideal when a simple error message suffices.  
   **Usage Example:**
   ```csharp
   // Creating a successful Option (Maybe)
   Option<int> maybeSuccess = Option.Some(100);

   // Creating a failed Option (Maybe)
   Option<int> maybeFailure = Option.Fail("Invalid input");

   // Implicit conversion and exception handling:
   try {
       int value = maybeSuccess; // Works fine
   }
   catch (HasFailedException<string> ex) {
       Console.WriteLine($"Failure: {ex.Failure}");
   }
   ```

2. **Option (Either)**  
   This variant is generic in its failure type, allowing you to provide richer or more structured error information.  
   **Usage Example:**
   ```csharp
   // Creating a successful Option (Either) with a custom failure type:
   Option<double, MyError> eitherSuccess = Option.Some<double, MyError>(3.14);

   // Creating a failed Option (Either) with a custom error:
   Option<double, MyError> eitherFailure = Option.Fail<double, MyError>(new MyError("Calculation error"));

   // Implicit conversion; note that an exception of type HasFailedException<MyError> is thrown on failure:
   try {
       double result = eitherSuccess;
   }
   catch (HasFailedException<MyError> ex) {
       Console.WriteLine($"Error: {ex.Failure}");
   }
   ```

In the above examples, `MyError` can be any type you define to represent error details, giving you the flexibility to carry structured error information.

## Exception Handling with CatchFailure and CatchAll

ThrowAway offers two powerful methods to encapsulate exception handling within your functional flow:

- **CatchFailure:**  
  Executes a function that returns an Option. If a `HasFailedException` occurs (i.e., a known failure within the Option chain), it converts the exception into a failed Option.

- **CatchAll:**  
  This method is more comprehensive. It catches both `HasFailedException` instances and any unexpected exceptions, converting them into a failed Option complete with the exception message and stack trace. This helps maintain a consistent error handling model without the need for manual try-catch blocks around every operation.

**Example:**
```csharp
// Using CatchFailure to wrap operations that may throw a HasFailedException
var result = Option.CatchFailure(() => ProcessData());

// Using CatchAll to capture any exception and convert it into a failure Option
var resultWithAll = Option.CatchAll(() => ProcessData(), errorMsg => $"Custom Error: {errorMsg}");
```

## Using TryUnwrap for Seamless Extraction

The `TryUnwrap` extension method lets you extract both the success value and the failure message in one go - without needing extra temporary variables. This method returns a boolean indicating whether the Option holds a value and outputs the value and failure as separate out parameters.

```csharp
if (areaResult.TryUnwrap(out double area, out string failure))
{
    // Use the value directly.
    Console.WriteLine($"Area is: {area}");
}
else
{
    // Handle the failure.
    Console.WriteLine($"Error calculating area: {failure}");
}
```

This feature is especially useful in procedural code, as it eliminates boilerplate and provides immediate access to both outcomes.

## Installation

Install ThrowAway via NuGet:

```shell
dotnet add package ThrowAway
```

## Usage Examples

### Basic Option Usage

Handling operations that might succeed or fail is straightforward:

```csharp
// Option (Maybe) usage:
var maybeOutcome = Option.Some(5);
var maybeError = Option.Fail("An error occurred");

// Option (Either) usage:
var eitherOutcome = Option.Some<int, string>(42);
var eitherError = Option.Fail<int, string>("Calculation failed");
```

### Implicit Conversion and Exception Handling

Implicit conversion makes the integration with try-catch blocks seamless:

```csharp
try
{
    int result = Option.Some(10); // Implicitly converts to int.
    Console.WriteLine($"Result: {result}");
}
catch (HasFailedException<string> ex)
{
    Console.WriteLine($"Error: {ex.Failure}");
}
```

### Combining Functional and Procedural Styles

A typical example where functional chaining meets procedural checks:

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
    Console.WriteLine($"Error: {areaResult.Failure}");
}
else
{
    // Implicit conversion to double
    double area = areaResult;
    Console.WriteLine($"Area: {area}");
}
```

### Advanced Functional Operations

Chain transformations and error handling with ThrowAway's rich API:

```csharp
var finalResult = Option.Some(5)
    .Map(x => x * 2)
    .FlatMap(x => x > 10 
        ? Option.Some(x.ToString()) 
        : Option.Fail<string>("Result is too small"))
    .Match(
        some: val => $"Success: {val}",
        fail: err => $"Failure: {err}"
    );
Console.WriteLine(finalResult);
```

## Why Choose ThrowAway?

- **Pragmatic Integration:**  
  Benefit from explicit functional error handling while continuing to use familiar procedural constructs like try-catch.
- **Clear Code Paths:**  
  Clearly distinguish between success and failure, making your code easier to understand and maintain.
- **Eliminate Nulls:**  
  Struct-based Options guarantee non-null instances, reducing the risk of null-reference errors.
- **Flexible API:**  
  Whether you need simple string errors (Option Maybe) or more detailed error types (Option Either), ThrowAway has you covered.
- **Robust Exception Wrapping:**  
  CatchFailure and CatchAll methods convert exceptions into failed Options, keeping error handling consistent.
- **Effortless Unwrapping:**  
  TryUnwrap simplifies the extraction of values and failures, bridging the gap between functional and procedural paradigms.

## License

ThrowAway is licensed under the MIT License, making it an open and flexible solution for error handling in both open-source and commercial projects.