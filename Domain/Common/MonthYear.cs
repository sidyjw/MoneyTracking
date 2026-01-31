namespace Domain.Common;

public record MonthYear : IComparable<MonthYear>
{
    public int Year { get; }
    public int Month { get; }

    private MonthYear(int year, int month)
    {
        Year = year;
        Month = month;
    }

    public static ResultT<MonthYear> Create(int year, int month)
    {
        if (year < 1900 || year > 2100)
            return Error.Validation(MonthYearErrors.InvalidYear, "Ano deve estar entre 1900 e 2100.");

        if (month < 1 || month > 12)
            return Error.Validation(MonthYearErrors.InvalidMonth, "Mês deve estar entre 1 e 12.");

        return new MonthYear(year, month);
    }

    public static MonthYear FromDateTime(DateTime date)
    {
        return new MonthYear(date.Year, date.Month);
    }

    public static MonthYear Current()
    {
        var now = DateTime.Now;
        return new MonthYear(now.Year, now.Month);
    }

    public DateTime ToFirstDayOfMonth()
    {
        return new DateTime(Year, Month, 1);
    }

    public DateTime ToLastDayOfMonth()
    {
        return new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
    }

    public MonthYear AddMonths(int months)
    {
        var date = new DateTime(Year, Month, 1).AddMonths(months);
        return new MonthYear(date.Year, date.Month);
    }

    public MonthYear Next() => AddMonths(1);

    public MonthYear Previous() => AddMonths(-1);

    public override string ToString() => $"{Month:D2}/{Year}";

    public string ToString(string format)
    {
        var date = new DateTime(Year, Month, 1);
        return date.ToString(format);
    }

    #region Comparison

    public int CompareTo(MonthYear? other)
    {
        if (other is null) return 1;

        var yearComparison = Year.CompareTo(other.Year);
        if (yearComparison != 0) return yearComparison;

        return Month.CompareTo(other.Month);
    }

    public static bool operator <(MonthYear left, MonthYear right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(MonthYear left, MonthYear right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(MonthYear left, MonthYear right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(MonthYear left, MonthYear right)
    {
        return left.CompareTo(right) >= 0;
    }

    #endregion
}
