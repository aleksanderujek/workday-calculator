public interface IDayIncrementStrategy
{
    DateTime CalculcateNewWorkday(DateTime date, bool isPositiveIncrement);
}

public class SaturdayStrategy : IDayIncrementStrategy
{
    public DateTime CalculcateNewWorkday(DateTime date, bool isPositiveIncrement)
    {
        DateTime newDate = date.AddDays(isPositiveIncrement ? 2 : -1);
        return newDate;
    }
}

public class SundayStrategy : IDayIncrementStrategy
{
    public DateTime CalculcateNewWorkday(DateTime date, bool isPositiveIncrement)
    {
        DateTime newDate = date.AddDays(isPositiveIncrement ? 1 : -2);
        return newDate;
    }
}

public class HolidayStrategy : IDayIncrementStrategy
{
    public DateTime CalculcateNewWorkday(DateTime date, bool isPositiveIncrement)
    {
        DateTime newDate = date.AddDays(isPositiveIncrement ? 1 : -1);
        return newDate;
    }
}

public class DefaultStrategy : IDayIncrementStrategy
{
    public DateTime CalculcateNewWorkday(DateTime date, bool isPositiveIncrement)
    {
        return date;
    }
}