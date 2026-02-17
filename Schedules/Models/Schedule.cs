using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace i2u.BusinessBase.Schedules.Models
{
    public class Schedule
    {
        private readonly TimeZoneInfo _timeZoneCentral;
        private readonly TimeZoneInfo _timeZoneUtc;

        public static ReadOnlyCollection<TimeZoneInfo> TimeZonesInfo = TimeZoneInfo.GetSystemTimeZones();

        public Guid Id { get; set; } = Guid.NewGuid();

        public TimeSpan DailyAt { get; set; } = TimeSpan.Parse("00:00:00");

        public TimeZoneInfo TimeZone { get; set; }

        public Schedule()
        {
            _timeZoneCentral = TimeZonesInfo.First(z => z.StandardName == "Central Standard Time");
            _timeZoneUtc = TimeZonesInfo.First(z => z.StandardName == "Coordinated Universal Time");
            TimeZone = _timeZoneCentral;
        }

        public Schedule(TimeSpan dailyAt) : this()
        {
            DailyAt = dailyAt;
        }

        public Schedule(TimeSpan dailyAt, string timeZone): this(dailyAt)
        {
            TimeZone = TimeZonesInfo.FirstOrDefault(z => z.StandardName == timeZone);
            if (TimeZone == null)
            {
                throw new TimeZoneNotFoundException();
            }
        }

        public DateTime CalculateNextScheduledTime(DateTime now)
        {
            if (now.Kind != DateTimeKind.Utc)
            {
                throw new InvalidTimeZoneException("DateTime object passed in is not DateTimeKind.Utc.");
            }

            var nextTime = new DateTime(now.Year, now.Month, now.Day, DailyAt.Hours, DailyAt.Minutes, DailyAt.Seconds, DateTimeKind.Unspecified);
            nextTime = TimeZoneInfo.ConvertTime(nextTime, TimeZone);

            while (nextTime < now)
            {
                nextTime = nextTime.AddDays(1);
            }

            return TimeZoneInfo.ConvertTime(nextTime, _timeZoneUtc);
        }

    }
}