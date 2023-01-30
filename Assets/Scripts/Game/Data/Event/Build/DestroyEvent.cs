using System;
using Game.Core;

namespace Game.Data.Event
{
    public struct DestroyEvent : IEvent
    {
        public Action handler;
    }
}