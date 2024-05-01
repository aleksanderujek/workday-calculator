public interface IDayIncrementStrategy
{
    /// <summary>
    /// Calculates the new workday based on the given date and increment.
    /// </summary>
    /// <param name="date">The date to start from.</param>
    /// <param name="isPositiveIncrement">A flag indicating whether the increment is positive or negative.</param>
    /// <returns>The new workday.</returns>
    DateTime CalculcateNewWorkday(DateTime date, bool isPositiveIncrement);
}


/// <summary>
/// Represents a strategy for calculating the new workday based on the given date and increment direction.
/// If the new date falls on a Saturday, based on increment direction, it will be adjusted to the next Monday or the previous Friday.
/// </summary>
public class SaturdayStrategy : IDayIncrementStrategy
{
    public DateTime CalculcateNewWorkday(DateTime date, bool isPositiveIncrement)
    {
        DateTime newDate = date.AddDays(isPositiveIncrement ? 2 : -1);
        return newDate;
    }
}

/// <summary>
/// Represents a strategy for calculating the new workday based on the given date and increment direction.
/// If the new date falls on a Sunday, based on increment direction, it will be adjusted to the next Monday or the previous Friday.
/// </summary>
public class SundayStrategy : IDayIncrementStrategy
{
    public DateTime CalculcateNewWorkday(DateTime date, bool isPositiveIncrement)
    {
        DateTime newDate = date.AddDays(isPositiveIncrement ? 1 : -2);
        return newDate;
    }
}

/// <summary>
/// Represents a strategy for calculating the new workday based on the given date and increment direction.
/// If the new date falls on a holiday, based on increment direction, it will be adjusted to the next workday or the previous workday.
/// </summary>
public class HolidayStrategy : IDayIncrementStrategy
{
    public DateTime CalculcateNewWorkday(DateTime date, bool isPositiveIncrement)
    {
        DateTime newDate = date.AddDays(isPositiveIncrement ? 1 : -1);
        return newDate;
    }
}

/// <summary>
/// Represents a strategy for calculating the new workday based on the given date and increment direction.
/// If the new date falls on a regular workday, it will be returned as is.
/// </summary>
public class DefaultStrategy : IDayIncrementStrategy
{
    public DateTime CalculcateNewWorkday(DateTime date, bool isPositiveIncrement)
    {
        return date;
    }
}