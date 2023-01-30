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
            EventCenter.AddListener<MouseRotateEvent>(MouseRotateCamera);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<SwipeStartEvent>(OnSwipeStarted);
            EventCenter.RemoveListener<SwipeChangedEvent>(ChangeCameraPosition);

            EventCenter.RemoveListener<PinchStartEvent>(OnPinchStarted);
            EventCenter.RemoveListener<PinchChangedEvent>(ChangeCameraSize);
            
            EventCenter.RemoveListener<WheelScrollEvent>(WheelChangeCameraSize);
            EventCenter.RemoveListener<MouseRotateEvent>(MouseRotateCamera);
            
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
                mainCam.transform.Rotate(Vector3.forward, (distance - _preDistance) / 10, Space.World);

                _preDistance = distance;
            }
        }
        
        private void WheelChangeCameraSize(WheelScrollEvent evt)
        {
            var camTransform = mainCam.transform;
            
            var distance = math.abs(evt.delta);
            if (distance >= float.Epsilon)
            {
                var delta = evt.delta * Time.deltaTime * -0.1f;
                if (camTransform.position.y + delta is < 2 or > 10)
                {
                    return;
                }

                camTransform.position += new Vector3(0, evt.delta * Time.deltaTime * -0.1f, 0);
                camTransform.rotation *= Quaternion.Euler(-evt.delta * 0.005f, 0, 0);
            }
        }

        private void MouseRotateCamera(MouseRotateEvent evt)
        {
            var camTransform = mainCam.transform;
            var angle = camTransform.eulerAngles;
            camTransform.rotation = Quaternion.Euler(angle.x, angle.y + evt.pos.x / 2, 0);
        }
    }
}