using System.Text.Json.Serialization;

namespace Domain.Common;

public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class representing a successful result.
    /// </summary>
    protected Result()
    {
        IsSuccess = true;
        Errors = default;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class representing a failed result with an error.
    /// </summary>
    /// <param name="error">The error associated with the failed result.</param>
    protected Result(Error error)
    {
        IsSuccess = false;
        Errors = [error];
    }

    protected Result(List<Error> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }

    /// <summary>
    /// Gets a value indicating whether the result is successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result is failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error associated with the result, if any.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<Error>? Errors { get; } = [];

    /// <summary>
    /// Implicitly converts an <see cref="Errors"/> to a <see cref="Result"/> representing a failed result.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified error.</returns>
    public static implicit operator Result(Error error) =>
        new(error);

    public static implicit operator Result(List<Error> errors) =>
        new(errors);

    /// <summary>
    /// Creates a new instance of <see cref="Result"/> representing a successful result.
    /// </summary>
    /// <returns>A new instance of <see cref="Result"/> representing a successful result.</returns>
    public static Result Success() =>
        new();

    /// <summary>
    /// Creates a new instance of <see cref="Result"/> representing a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failed result.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified error.</returns>
    public static Result Failure(Error error) =>
        new(error);
}