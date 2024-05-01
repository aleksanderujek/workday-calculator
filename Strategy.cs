public interface IDayIncrementStrategy
{
    DateTime CalculcateNewWorkday(DateTime date, bool isAdding);
}

public class SaturdayStrategy : IDayIncrementStrategy
{
    public DateTime CalculcateNewWorkday(DateTime date, bool isAdding)
    {
        DateTime newDate = date.AddDays(isAdding ? 2 : -1);
        //Console.WriteLine("It's Saturday. Adding 2 days: " + newDate.ToString("d"));
        return newDate;
    }
}

public class SundayStrategy : IDayIncrementStrategy
{
    public DateTime CalculcateNewWorkday(DateTime date, bool isAdding)
    {
        DateTime newDate = date.AddDays(isAdding ? 1 : -2);
        //Console.WriteLine("It's Sunday. Adding 1 day: " + newDate.ToString("d"));
        return newDate;
    }
}

public class HolidayStrategy : IDayIncrementStrategy
{
    public DateTime CalculcateNewWorkday(DateTime date, bool isAdding)
    {
        DateTime newDate = date.AddDays(isAdding ? 1 : -1);
        //Console.WriteLine("It's a holiday. Adding 1 day: " + newDate.ToString("d"));
        return newDate;
    }
}

public class DefaultStrategy : IDayIncrementStrategy
{
    public DateTime CalculcateNewWorkday(DateTime date, bool isAdding)
    {
        return date;
    }
}