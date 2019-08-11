﻿using System;

namespace Xtreem.CryptoPrediction.Data.Types
{
    public struct Resolution
    {
        private const string MinuteDescription = "1";
        private const string HourDescription = "60";
        private const string DayDescription = "D";

        public TimeSpan Interval { get; }

        private Resolution(TimeSpan interval) => Interval = interval;

        public static Resolution Minute => new Resolution(TimeSpan.FromMinutes(1));

        public static Resolution Hour => new Resolution(TimeSpan.FromHours(1));

        public static Resolution Day => new Resolution(TimeSpan.FromDays(1));

        public static Resolution Parse(string s)
        {
            switch (s)
            {
                case MinuteDescription:
                    return Minute;
                case HourDescription:
                    return Hour;
                case DayDescription:
                    return Day;
                default:
                    throw new FormatException($"String was not recognised as a valid {nameof(Resolution)}.");
            }
        }

        public int IntervalsInPeriod(TimeSpan period) => (int)(period / Interval);

        public string ToTradingViewFormat() => this == Minute ? MinuteDescription : this == Hour ? HourDescription : this == Day ? DayDescription : Interval.ToString();

        public override string ToString() => this == Minute ? nameof(Minute) : this == Hour ? nameof(Hour) : this == Day ? nameof(Day) : Interval.ToString();

        public override bool Equals(object obj) => obj is Resolution other && Equals(other);

        public bool Equals(Resolution other) => Interval.Equals(other.Interval);

        public override int GetHashCode() => Interval.GetHashCode();

        public static bool operator ==(Resolution left, Resolution right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Resolution left, Resolution right)
        {
            return !left.Equals(right);
        }
    }
}
