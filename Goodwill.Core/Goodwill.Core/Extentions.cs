using System;
using System.Collections.Generic;
using System.Linq;

namespace Goodwill.Core
{
    public static class Extentions
    {
        private static readonly Random Rnd = new Random((int)DateTime.Now.Ticks);

        public static Deck<T> Shuffle<T>(this IEnumerable<T> items)
        {
            return new Deck<T>(items.OrderBy(item => Rnd.Next()));
        }

        public static IEnumerable<T> GenerateRandom<T>(this List<T> items, int count = int.MaxValue)
        {
            for (var i = 0; i < count; i++)
            {
                yield return items[Rnd.Next(items.Count)];
            }
        }
    }
}