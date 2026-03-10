using System;
using System.Globalization;

using i2u.BusinessBase.Schedules.Models;

namespace i2u.BusinessBaseTests.Schedules;

public class ScheduleTests
{
    private static Tuple<TimeSpan, DateTime, DateTime>[] _dailyAtTests =
    {
        new Tuple<TimeSpan, DateTime, DateTime>(
            TimeSpan.Parse("04:00:00"),
            DateTime.Parse("2026-02-01T00:00:00.000Z",  null, DateTimeStyles.RoundtripKind),
            DateTime.Parse("2026-02-01T10:00:00.000Z", null, DateTimeStyles.RoundtripKind)
            ),
        new Tuple<TimeSpan, DateTime, DateTime>(
            TimeSpan.Parse("04:00:00"),
            DateTime.Parse("2026-02-01T04:00:00.000Z",  null, DateTimeStyles.RoundtripKind),
            DateTime.Parse("2026-02-01T10:00:00.000Z", null, DateTimeStyles.RoundtripKind)
            ),
        new Tuple<TimeSpan, DateTime, DateTime>(
            TimeSpan.Parse("04:00:00"),
            DateTime.Parse("2026-02-01T10:00:00.000Z",  null, DateTimeStyles.RoundtripKind),
            DateTime.Parse("2026-02-02T10:00:00.000Z", null, DateTimeStyles.RoundtripKind)
            )
    };

    private static Tuple<TimeSpan, DateTime, DateTime>[] _dailyAtExceptionTests =
    {
        new Tuple<TimeSpan, DateTime, DateTime>(
            TimeSpan.Parse("04:00:00"),
            DateTime.Parse("2026-02-01T00:00:00.000 -0600",  null, DateTimeStyles.RoundtripKind),
            DateTime.Parse("2026-02-01T10:00:00.000Z", null, DateTimeStyles.RoundtripKind)
            ),
    };

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    [TestCaseSource(nameof(_dailyAtTests))]
    public void CalculateNextScheduledTime_DailyAt(Tuple<TimeSpan, DateTime, DateTime> testParams)
    {
        TimeSpan dailyAt = testParams.Item1;
        DateTime now = testParams.Item2;
        DateTime expectedResult = testParams.Item3;

        var schedule = new Schedule(dailyAt);
        DateTime calculatedNextScheduleTime = schedule.CalculateNextScheduledTime(now);
        Assert.That(calculatedNextScheduleTime.Year.Equals(expectedResult.Year),
            "Year is {0}, expected {1}.",
            calculatedNextScheduleTime.Year, expectedResult.Year);

        Assert.That(calculatedNextScheduleTime.Month.Equals(expectedResult.Month),
            "Month is {0}, expected {1}.",
            calculatedNextScheduleTime.Month, expectedResult.Month);

        Assert.That(calculatedNextScheduleTime.Day.Equals(expectedResult.Day),
            "Day is {0}, expected {1}.",
            calculatedNextScheduleTime.Day, expectedResult.Day);

        Assert.That(calculatedNextScheduleTime.Hour.Equals(expectedResult.Hour),
            "Hour is {0}, expected {1}.",
            calculatedNextScheduleTime.Hour, expectedResult.Hour);

        Assert.That(calculatedNextScheduleTime.Minute.Equals(expectedResult.Minute),
            "Minute is {0}, expected {1}.",
            calculatedNextScheduleTime.Minute, expectedResult.Minute);

        Assert.That(calculatedNextScheduleTime.Second.Equals(expectedResult.Second),
            "Second is {0}, expected {1}.",
            calculatedNextScheduleTime.Second, expectedResult.Second);

        Assert.That(calculatedNextScheduleTime.Kind.Equals(expectedResult.Kind),
            "Kind is {0}, expected {1}.",
            calculatedNextScheduleTime.Kind.ToString(), expectedResult.Kind.ToString());
    }

    [Test]
    [TestCaseSource(nameof(_dailyAtExceptionTests))]
    public void CalculateNextScheduledTime_DailyAt_Throws(Tuple<TimeSpan, DateTime, DateTime> testParams)
    {
        TimeSpan dailyAt = testParams.Item1;
        DateTime now = testParams.Item2;
        DateTime expectedResult = testParams.Item3;

        var schedule = new Schedule(dailyAt);

        Assert.Throws<InvalidTimeZoneException>(() =>
        {
            DateTime calculatedNextScheduleTime = schedule.CalculateNextScheduledTime(now);

        });
    }
}