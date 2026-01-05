namespace Domain.ValueObjects;

public record CategoryType
{
    public static readonly CategoryType Income = new CategoryType(CategoryTypeEnum.Income);
    public static readonly CategoryType Expense = new CategoryType(CategoryTypeEnum.Expense);

    public CategoryTypeEnum Value { get; }
    private CategoryType(CategoryTypeEnum value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString();
}

public enum CategoryTypeEnum
{
    Income,
    Expense
}
