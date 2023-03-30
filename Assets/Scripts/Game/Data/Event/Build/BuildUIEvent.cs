using Game.Core;
using UnityEngine;

namespace Game.Data.Event
{
    public struct BuildUIEvent : IEvent
    {
        public bool canConstruct;
    }

    public struct RoadConstructEvent : IEvent
    {
        
    }
}