﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Evento.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToTimespan(this DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var time = dateTime.Subtract(epoch);

            return time.Ticks / 10000;
        }
    }
}