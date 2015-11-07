using Goodwill.Core.Events;

namespace Goodwill.Core
{
    public class Build
    {
        public static ManagerBuilder Manager => new ManagerBuilder();
        public static GameEventBuilder Event => new GameEventBuilder();
    }
}