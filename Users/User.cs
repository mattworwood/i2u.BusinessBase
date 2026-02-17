using System;

using i2u.BusinessBase.Schedules.Models;

namespace i2u.BusinessBase.Users
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string NameFirst { get; set; } = string.Empty;

        public string NameLast { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string TimeZoneId { get; set; } = string.Empty;

        public TimeSpan DeliveryDailyAt { get; set; } = TimeSpan.Parse("00:00:00");

        public string BibleVersion { get; set; } = string.Empty;
    }
}
