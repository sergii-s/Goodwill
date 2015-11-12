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

        public static Deck<T> ToDeck<T>(this IEnumerable<T> items)
        {
            return new Deck<T>(items);
        }

        public static IEnumerable<T> GenerateRandom<T>(this List<T> items, int count = int.MaxValue)
        {
            for (var i = 0; i < count; i++)
            {
                yield return items[Rnd.Next(items.Count)];
            }
        }

        public static IEnumerable<T> Pick<T>(this List<T> items)
        {
            return items.Pick(arg => true);
        }

        public static IEnumerable<T> Pick<T>(this List<T> items, Func<T,bool> check)
        {
            while (true)
            {
                var item = items.FirstOrDefault(check);
                if (item == null)
                {
                    break;
                }
                items.Remove(item);
                yield return item;
            }
        }
        
        public static T Random<T>(this IEnumerable<T> items)
        {
            var itemsArray = items as T[] ?? items.ToArray();
            var itemIdx = Rnd.Next(itemsArray.Length);
            return itemsArray[itemIdx];
        }
    }
}