﻿using System;

namespace SFA.Infrastructure.Interfaces.Caching

{
    public enum CacheDuration
    {
        CacheDefault = 0,
        OneMinute = 1,
        FiveMinutes = 5,
        FifteenMinutes = 15,
        ThirtyMinutes = 30,
        OneHour = 60,
        OneDay = 1440,
    }
}
