﻿using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Xtreem.Crusader.Shared.Converters;
using NewtonsoftJsonConverterAttribute = Newtonsoft.Json.JsonConverterAttribute;

namespace Xtreem.Crusader.Shared.Types
{
    [TypeConverter(typeof(ResolutionTypeConverter)),
     JsonConverter(typeof(ResolutionJsonConverter)),
     NewtonsoftJsonConverter(typeof(ResolutionNewtonsoftJsonConverter))]
    public struct Resolution
    {
        private const string MinuteDescription = "1";
        private const string HourDescription = "60";
        private const string DayDescription = "D";

        public TimeSpan Interval { get; }

        public Resolution(TimeSpan interval) => Interval = interval;

        public static Resolution Minute => new Resolution(TimeSpan.FromMinutes(1));

        public static Resolution Hour => new Resolution(TimeSpan.FromHours(1));

        public static Resolution Day => new Resolution(TimeSpan.FromDays(1));

        public static Resolution Parse(string s)
        {
            switch (s)
            {
                case nameof(Minute):
                case MinuteDescription:
                    return Minute;
                case nameof(Hour):
                case HourDescription:
                    return Hour;
                case nameof(Day):
                case DayDescription:
                    return Day;
                default:
                    return TimeSpan.TryParse(s, out var result)
                        ? new Resolution(result)
                        : throw new FormatException($"String was not recognised as a valid {nameof(Resolution)}.");
            }
        }

        public int IntervalsInPeriod(TimeSpan period) => (int)(period.Ticks / Interval.Ticks); //TODO: Remove .Ticks when upgraded to .NET Standard 2.1.

        public override string ToString() => this == Minute ? nameof(Minute) : this == Hour ? nameof(Hour) : this == Day ? nameof(Day) : Interval.ToString();

        public override bool Equals(object obj) => obj is Resolution other && Equals(other);

        public bool Equals(Resolution other) => Interval.Equals(other.Interval);

        public override int GetHashCode() => Interval.GetHashCode();

        public static bool operator ==(Resolution left, Resolution right) => left.Equals(right);

        public static bool operator !=(Resolution left, Resolution right) => !left.Equals(right);
    }
}
