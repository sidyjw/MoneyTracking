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

    /// <summary>
    /// Cria uma nova instância de MonthYear.
    /// </summary>
    public static MonthYear Create(int year, int month)
    {
        if (year < 1900 || year > 2100)
            throw new ArgumentException("Ano deve estar entre 1900 e 2100.", nameof(year));

        if (month < 1 || month > 12)
            throw new ArgumentException("Mês deve estar entre 1 e 12.", nameof(month));

        return new MonthYear(year, month);
    }

    /// <summary>
    /// Cria um MonthYear a partir de um DateTime.
    /// </summary>
    public static MonthYear FromDateTime(DateTime date)
    {
        return new MonthYear(date.Year, date.Month);
    }

    /// <summary>
    /// Retorna o MonthYear atual (mês e ano atuais).
    /// </summary>
    public static MonthYear Current()
    {
        var now = DateTime.Now;
        return new MonthYear(now.Year, now.Month);
    }

    /// <summary>
    /// Retorna o primeiro dia do mês/ano.
    /// </summary>
    public DateTime ToFirstDayOfMonth()
    {
        return new DateTime(Year, Month, 1);
    }

    /// <summary>
    /// Retorna o último dia do mês/ano.
    /// </summary>
    public DateTime ToLastDayOfMonth()
    {
        return new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
    }

    /// <summary>
    /// Adiciona meses ao MonthYear atual.
    /// </summary>
    public MonthYear AddMonths(int months)
    {
        var date = new DateTime(Year, Month, 1).AddMonths(months);
        return new MonthYear(date.Year, date.Month);
    }

    /// <summary>
    /// Retorna o próximo mês.
    /// </summary>
    public MonthYear Next() => AddMonths(1);

    /// <summary>
    /// Retorna o mês anterior.
    /// </summary>
    public MonthYear Previous() => AddMonths(-1);

    /// <summary>
    /// Retorna uma representação string no formato "MM/yyyy".
    /// </summary>
    public override string ToString() => $"{Month:D2}/{Year}";

    /// <summary>
    /// Retorna uma representação string formatada.
    /// </summary>
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