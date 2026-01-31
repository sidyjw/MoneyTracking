namespace Domain.Common;

public sealed class ResultT<TValue> : Result
{
    private readonly TValue? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultT{TValue}"/> class with a successful result and a value.
    /// </summary>
    /// <param name="value">The value.</param>
    private ResultT(
        TValue value
    ) : base()
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultT{TValue}"/> class with a failed result and an error.
    /// </summary>
    /// <param name="error">The error.</param>
    private ResultT(
        Error error
    ) : base(error)
    {
        _value = default;
    }

    private ResultT(
        List<Error> errors
    ) : base(errors)
    {
        _value = default;
    }

    /// <summary>
    /// Gets the value of the result.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the result is not successful.</exception>
    public TValue Value =>
        IsSuccess ? _value! : throw new InvalidOperationException("Value can not be accessed when IsSuccess is false");

    /// <summary>
    /// Implicitly converts an <see cref="Error"/> to a <see cref="ResultT{TValue}"/> with a failed result.
    /// </summary>
    /// <param name="error">The error.</param>
    public static implicit operator ResultT<TValue>(Error error) =>
        new(error);

    public static implicit operator ResultT<TValue>(List<Error> errors) =>
        new(errors);

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="TValue"/> to a <see cref="ResultT{TValue}"/> with a successful result.
    /// </summary>
    /// <param name="value">The value.</param>
    public static implicit operator ResultT<TValue>(TValue value) =>
        new(value);

    /// <summary>
    /// Creates a new <see cref="ResultT{TValue}"/> with a successful result and a value.
    /// </summary>
    /// <param name="value">The value.</param>
    public static ResultT<TValue> Success(TValue value) =>
        new(value);

    /// <summary>
    /// Creates a new <see cref="ResultT{TValue}"/> with a failed result and an error.
    /// </summary>
    /// <param name="error">The error.</param>
    public static new ResultT<TValue> Failure(Error error) =>
        new(error);

    public static ResultT<TValue> Failure(List<Error> errors) =>
        new(errors);
}