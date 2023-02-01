using System;
using System.Runtime.CompilerServices;
using Game.Core;
using Game.GamePlaySystem.BurstUtil;
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

        private float2 _preDir;
        
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
            // todo 摄像机相对运动待完善
            var deltaPos = -math.normalize(evt.pos - _startPosition) * (Time.deltaTime * cameraSpeed);
            if (math.length(deltaPos) > float.Epsilon)
            {
                var camTransform = mainCam.transform;
                var forward = camTransform.forward;
                camTransform.position += new Vector3(deltaPos[0] * -forward.y, 0, deltaPos[1] * 0.75f * forward.z);
            }

            _startPosition = evt.pos;
        }

        private void OnPinchStarted(PinchStartEvent evt)
        {
            _preDistance = math.distance(evt.primaryPos, evt.secondaryPos);
            _preDir = evt.secondaryPos - evt.primaryPos;
        }

        /// <summary>
        /// 双指缩放改变大小
        /// </summary>
        private void ChangeCameraSize(PinchChangedEvent evt)
        {
            var distance = math.distance(evt.primaryPos, evt.secondaryPos);
            var currentDir = evt.secondaryPos - evt.primaryPos;
            var deltaAngle = BuildingUtils.SignedAngle(new float3(_preDir, 0), new float3(currentDir, 0), Vector3.forward);
            
            var camTransform = mainCam.transform;
            var angle = camTransform.eulerAngles;
            camTransform.rotation = Quaternion.Euler(angle.x, angle.y + deltaAngle / 2, 0);
            _preDir = currentDir;

            if (math.abs(distance - _preDistance) >= float.Epsilon)
            {
                camTransform.position += new Vector3(0, (distance - _preDistance) * Time.deltaTime * -0.5f, 0);
                camTransform.rotation *= Quaternion.Euler((distance - _preDistance) * 0.02f, 0, 0);
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