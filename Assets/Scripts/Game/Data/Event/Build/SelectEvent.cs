using Game.Core;
using UnityEngine;

namespace Game.Data.Event
{
    public struct SelectEvent : IEvent
    {
        public Vector3 position;
    }
}