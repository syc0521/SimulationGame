using System.Collections.Generic;
using Game.Core;

namespace Game.Data.Event.Task
{
    public struct RefreshTaskEvent : IEvent
    {
        public Dictionary<int, PlayerTaskData> playerTask;
    }
}