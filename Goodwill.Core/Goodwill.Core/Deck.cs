using System.Collections.Generic;

namespace Goodwill.Core
{
    public class Deck<T> : Queue<T>
    {
        public Deck()
        {
        }

        public Deck(IEnumerable<T> enumerable) : base(enumerable)
        {
        }

        public List<T> Pick(int count)
        {
            var result = new List<T>();
            while (count > 0)
            {
                result.Add(Dequeue());
                count--;
            }
            return result;
        }

        public T Pick()
        {
            return Dequeue();
        }
    }
}