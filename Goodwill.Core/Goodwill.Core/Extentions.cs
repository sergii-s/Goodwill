using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public static class Extentions
    {
        public static Deck<T> Shuffle<T>(this IEnumerable<T> items)
        {
            var rnd = new Random((int)DateTime.Now.Ticks);
            return new Deck<T>(items.OrderBy(item => rnd.Next()));
        }
    }
}