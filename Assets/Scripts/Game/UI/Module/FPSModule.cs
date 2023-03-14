using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data.Event.Common;
using UnityEngine;

namespace Game.UI.Module
{
    public class FPSModule : BaseModule
    {
        private const int Granularity = 10;
        private List<double> times;
        private int counter = 5;
        
        public override void OnCreated()
        {
            times = new List<double>();
        }

        public override void OnDestroyed()
        {
            times.Clear();
            times = null;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (times == null)
            {
                return;
            }
            
            if (counter <= 0)
            {
                CalcFPS();
                counter = Granularity;
            } 

            times.Add (Time.deltaTime);
            counter--; 
        }

        private void CalcFPS()
        {
            double sum = times.Sum();
            double average = sum / times.Count;
            double fps = 1 / average;
            EventCenter.DispatchEvent(new FPSEvent
            {
                fps = (int)fps,
            });
        }
    }
}