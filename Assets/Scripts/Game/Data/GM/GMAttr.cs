using System;

namespace Game.Data.GM
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GMAttr : Attribute
    {
        public string type;
        public string name;
        public int priority;
    }
}