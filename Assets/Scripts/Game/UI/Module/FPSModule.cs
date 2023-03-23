using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data.Event.Common;
using UnityEngine;

namespace Game.UI.Module
{
    public class FPSModule : BaseModule
    {
        private float m_LastUpdateShowTime = 0f;  //上一次更新帧率的时间;  
        private float m_UpdateShowDeltaTime = 0.6f;//更新帧率的时间间隔;  
        private int m_FrameUpdate = 0;//帧数;  
        private float m_FPS = 0;//帧率
        
        public override void OnCreated()
        {
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
        }

        public override void OnDestroyed()
        {
            m_LastUpdateShowTime = 0f;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            CalcFPS();
        }

        private void CalcFPS()
        {
            m_FrameUpdate++;
            if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
            {
                //FPS = 某段时间内的总帧数 / 某段时间
                m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);
                m_FrameUpdate = 0;
                m_LastUpdateShowTime = Time.realtimeSinceStartup;
            }
            EventCenter.DispatchEvent(new FPSEvent
            {
                fps = Mathf.CeilToInt(m_FPS),
            });
        }
    }
}