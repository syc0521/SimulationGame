using System;
using System.IO;
using System.Runtime.CompilerServices;
using Game.Audio;
using Game.Core;
using Game.Data;
using Game.Data.Event.Common;
using Game.GamePlaySystem.BurstUtil;
using Game.Input;
using Unity.Mathematics;
using UnityEngine;

namespace Game.GamePlaySystem
{
    public class CameraManager : GamePlaySystemBase<CameraManager>
    {
        public Camera mainCam => Camera.main;

        private float2 _startPosition;
        private float _preDistance;

        private float2 _preDir;
        private bool _isEntered;
        
        private readonly float minHeight = ConfigTable.Instance.GetGestureConfig().minHeight;
        private readonly float maxHeight = ConfigTable.Instance.GetGestureConfig().maxHeight;
        private readonly float2 Center = new(100, 100);

        public override void OnStart()
        {
            EventCenter.AddListener<LoadSceneFinishedEvent>(ChangeUICam);

            EventCenter.AddListener<SwipeStartEvent>(OnSwipeStarted);
            EventCenter.AddListener<SwipeChangedEvent>(ChangeCameraPosition);

            EventCenter.AddListener<PinchStartEvent>(OnPinchStarted);
            EventCenter.AddListener<PinchChangedEvent>(ChangeCameraSize);
            
            EventCenter.AddListener<WheelScrollEvent>(WheelChangeCameraSize);
            EventCenter.AddListener<MouseRotateEvent>(MouseRotateCamera);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<LoadSceneFinishedEvent>(ChangeUICam);

            EventCenter.RemoveListener<SwipeStartEvent>(OnSwipeStarted);
            EventCenter.RemoveListener<SwipeChangedEvent>(ChangeCameraPosition);

            EventCenter.RemoveListener<PinchStartEvent>(OnPinchStarted);
            EventCenter.RemoveListener<PinchChangedEvent>(ChangeCameraSize);
            
            EventCenter.RemoveListener<WheelScrollEvent>(WheelChangeCameraSize);
            EventCenter.RemoveListener<MouseRotateEvent>(MouseRotateCamera);
            
            base.OnDestroyed();
        }

        public override void OnUpdate()
        {
            if (_isEntered)
            {
                var pos = mainCam.transform.position;
                var xzPos = new float2(pos.x, pos.z);
                var dis = math.distance(xzPos, Center);
                Managers.Get<IAudioManager>().AdjustAmbientVolume(dis * 0.00565f + 0.15f); // 0.8/141.4
            }
        }

        private void ChangeUICam(LoadSceneFinishedEvent evt)
        {
            ConfigTable.Instance.MainUICam.gameObject.SetActive(false);
            var cam = mainCam.transform.GetChild(0).GetComponent<Camera>();
            ConfigTable.Instance.MainCanvas.worldCamera = cam;
            _isEntered = true;
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
            var camPosY = mainCam.transform.position.y;
            var config = ConfigTable.Instance.GetGestureConfig();
            var speed = config.cameraSpeed * (camPosY / 5.5f);
            var deltaPos = -(evt.pos - _startPosition) * (Time.deltaTime * speed * 0.025f);//速度
            if (math.length(deltaPos) > float.Epsilon)
            {
                var camTransform = mainCam.transform;
                var forward = (float3)camTransform.forward;
                var angle = BuildingUtils.SignedAngle(new float2(0, 1), forward.xz);
                var delta3d = new float3(deltaPos.x, deltaPos.y, 0);
                var angleDelta = Quaternion.AngleAxis(angle, delta3d) * delta3d;
                
                var camPos = camTransform.position;
                var delta = new Vector3(angleDelta.x, 0, angleDelta.y);
                var newCamPos = camPos + delta;
                var posY = camPos.y;
                
                if (newCamPos.x > config.xLimit[0] && newCamPos.x < config.xLimit[1] &&
                    newCamPos.z > config.zLimit[0] && newCamPos.z < config.zLimit[1])
                {
                    camTransform.Translate(new Vector3(angleDelta.x, 0, angleDelta.y));
                    camPos = camTransform.position;
                    camTransform.position = new Vector3(camPos.x, posY, camPos.z);
                }
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
            var deltaDistance = distance - _preDistance;
            
            var camTransform = mainCam.transform;
            var angle = camTransform.eulerAngles;
            camTransform.rotation = Quaternion.Euler(angle.x, angle.y + deltaAngle / 1.5f, 0);
            _preDir = currentDir;

            var newPosY = camTransform.position.y + deltaDistance * Time.deltaTime * -0.3f;
            if (newPosY < minHeight || newPosY > maxHeight)
            {
                return;
            }
            if (math.abs(deltaDistance) >= float.Epsilon)
            {
                camTransform.position += new Vector3(0, deltaDistance * Time.deltaTime * -0.3f, 0);
                camTransform.rotation *= Quaternion.Euler(-deltaDistance * 0.035f, 0, 0);
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
                
                if (camTransform.position.y + delta < minHeight || camTransform.position.y + delta > maxHeight)
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
            camTransform.rotation = Quaternion.Euler(angle.x, angle.y + evt.pos.x / 3, 0);
        }

        public void TakePhoto()
        {
            int width = Screen.width;
            int height = Screen.height;
            var rt = new RenderTexture(width, height, 16);
            mainCam.targetTexture = rt;
            RenderTexture.active = rt;
            mainCam.Render();
            var photo = new Texture2D(width, height);
            photo.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            photo.Apply();
            mainCam.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.Destroy(rt);
            SavePhoto(photo);
        }
        
        private void SavePhoto(Texture2D tex)
        {
            var path = Application.persistentDataPath;
            if (tex == null) return;
            var photo = tex.EncodeToPNG();
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllBytes($"{path}/city.png", photo);
                Debug.Log($"{path}/city.png");
#if UNITY_ANDROID && !UNITY_EDITOR
                using AndroidJavaObject activity = new("com.defaultcompany.ecs.MainActivity");
                activity.Call("SavePhoto");
#endif
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

    }
}