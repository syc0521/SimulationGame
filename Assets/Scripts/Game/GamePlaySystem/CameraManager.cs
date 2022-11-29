using System;
using Game.Core;
using Game.Input;
using Unity.Mathematics;
using UnityEngine;

namespace Game.GamePlaySystem
{
    public class CameraManager : GamePlaySystemBase<CameraManager>
    {
        public Camera mainCam;
        public float cameraSpeed = 7.5f;

        private float2 _startPosition;
        private float _preDistance;
        
        public override void OnStart()
        {
            EventCenter.AddListener<SwipeStartEvent>(OnSwipeStarted);
            EventCenter.AddListener<SwipeChangedEvent>(ChangeCameraPosition);

            EventCenter.AddListener<PinchStartEvent>(OnPinchStarted);
            EventCenter.AddListener<PinchChangedEvent>(ChangeCameraSize);
            
            EventCenter.AddListener<WheelScrollEvent>(WheelChangeCameraSize);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<SwipeStartEvent>(OnSwipeStarted);
            EventCenter.RemoveListener<SwipeChangedEvent>(ChangeCameraPosition);

            EventCenter.RemoveListener<PinchStartEvent>(OnPinchStarted);
            EventCenter.RemoveListener<PinchChangedEvent>(ChangeCameraSize);
            
            EventCenter.RemoveListener<WheelScrollEvent>(WheelChangeCameraSize);
            base.OnDestroyed();
        }

        private void OnSwipeStarted(SwipeStartEvent evt)
        {
            _startPosition = evt.pos;
        }

        /// <summary>
        /// 滑动改变摄像机位置
        /// </summary>
        private void ChangeCameraPosition(SwipeChangedEvent evt)
        {
            var deltaPos = -math.normalize(evt.pos - _startPosition) * (Time.deltaTime * cameraSpeed);
            if (math.length(deltaPos) > float.Epsilon)
            {
                mainCam.transform.position += new Vector3(deltaPos[0], 0, deltaPos[1] * 0.75f);
            }

            _startPosition = evt.pos;
        }

        private void OnPinchStarted(PinchStartEvent evt)
        {
            _preDistance = math.distance(evt.primaryPos, evt.secondaryPos);
        }

        /// <summary>
        /// 双指缩放改变大小
        /// </summary>
        private void ChangeCameraSize(PinchChangedEvent evt)
        {
            var distance = math.distance(evt.primaryPos, evt.secondaryPos);
            if (math.abs(distance - _preDistance) >= float.Epsilon)
            {
                mainCam.transform.position += new Vector3(0, (distance - _preDistance) * Time.deltaTime * -0.5f, 0);
                _preDistance = distance;
            }
        }
        
        private void WheelChangeCameraSize(WheelScrollEvent evt)
        {
            var distance = math.abs(evt.delta);
            if (distance >= float.Epsilon)
            {
                mainCam.transform.position += new Vector3(0, evt.delta * Time.deltaTime * -0.1f, 0);
            }
        }
    }
}