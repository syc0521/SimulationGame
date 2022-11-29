using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Unity.Mathematics;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public struct PinchStartEvent : IEvent
    {
        public float2 primaryPos, secondaryPos;
    }
    
    public struct PinchChangedEvent : IEvent
    {
        public float2 primaryPos, secondaryPos;
    }
    
    public struct PinchEndEvent : IEvent
    {
        public float2 primaryPos, secondaryPos;
    }

    public struct SwipeStartEvent : IEvent
    {
        public float2 pos;
        public float time;
    }
    
    public struct SwipeChangedEvent : IEvent
    {
        public float2 pos;
        public float time;
    }
    
    public struct SwipeEndEvent : IEvent
    {
        public float2 pos;
        public float time;
    }

    public struct TouchEvent : IEvent
    {
        public Vector2 pos;
    }
    
    public struct LongPressEvent : IEvent
    {
        public float2 pos;
    }
    
    public class InputManager : ManagerBase, IInputManager
    {
        private GameControl _gameControl;
        private Coroutine _swipeCoroutine, _pinchCoroutine;

        public override void OnAwake()
        {
            _gameControl = new();
            _gameControl.Enable();
        }
        
        public override void OnStart()
        {
            _gameControl.GamePlay.PrimaryContact.started += SwipeStart;
            _gameControl.GamePlay.PrimaryContact.canceled += SwipeEnd;
            _gameControl.GamePlay.PrimaryContact.performed += Touch;

            _gameControl.GamePlay.SecondaryTouchContact.started += PinchStart;
            _gameControl.GamePlay.SecondaryTouchContact.canceled += PinchEnd;
            
            _gameControl.GamePlay.PrimaryLong.performed += LongPress;
        }

        public override void OnDestroyed()
        {
            _gameControl.GamePlay.PrimaryContact.started -= SwipeStart;
            _gameControl.GamePlay.PrimaryContact.canceled -= SwipeEnd;
            _gameControl.GamePlay.PrimaryContact.performed -= Touch;

            _gameControl.GamePlay.SecondaryTouchContact.started -= PinchStart;
            _gameControl.GamePlay.SecondaryTouchContact.canceled -= PinchEnd;

            _gameControl.GamePlay.PrimaryLong.performed -= LongPress;
            _gameControl.Disable();
        }

        public bool IsPointerOverGameObject()
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            List<RaycastResult> raycastResult = new List<RaycastResult>();

            foreach (Touch touch in UnityEngine.Input.touches)
            {
                pointer.position = touch.position;
                EventSystem.current.RaycastAll(pointer, raycastResult);
                return raycastResult.Count > 0;   
            }

            return false;
        }

        private void Touch(InputAction.CallbackContext ctx)
        {
            EventCenter.DispatchEvent(new TouchEvent
            {
                pos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>()
            });
        }
        
        private void LongPress(InputAction.CallbackContext ctx)
        {
            EventCenter.DispatchEvent(new LongPressEvent
            {
                pos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>()
            });
        }

        private void SwipeStart(InputAction.CallbackContext ctx)
        {
            EventCenter.DispatchEvent(new SwipeStartEvent
            {
                pos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>(),
                time = (float)ctx.startTime
            });
            _swipeCoroutine = MonoApp.Instance.StartCoroutine(SwipeDetection(ctx));
        }
        
        private void SwipeEnd(InputAction.CallbackContext ctx)
        {
            EventCenter.DispatchEvent(new SwipeEndEvent
            {
                pos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>(),
                time = (float)ctx.startTime
            });
            
            if (_swipeCoroutine != null)
            {
                MonoApp.Instance.StopCoroutine(_swipeCoroutine);
            }
        }

        private IEnumerator SwipeDetection(InputAction.CallbackContext ctx)
        {
            while (true)
            {
                if (UnityEngine.Input.touchCount > 1) break;
                
                EventCenter.DispatchEvent(new SwipeChangedEvent
                {
                    pos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>(),
                    time = (float)ctx.startTime
                });
                yield return null;
            }
        }

        private void PinchStart(InputAction.CallbackContext ctx)
        {
            EventCenter.DispatchEvent(new PinchStartEvent
            {
                primaryPos = _gameControl.GamePlay.PrimaryFingerPosition.ReadValue<Vector2>(),
                secondaryPos = _gameControl.GamePlay.SecondaryFingerPosition.ReadValue<Vector2>(),
            });
            _pinchCoroutine = MonoApp.Instance.StartCoroutine(PinchDetection());
        }
        
        private void PinchEnd(InputAction.CallbackContext ctx)
        {
            EventCenter.DispatchEvent(new PinchEndEvent
            {
                primaryPos = _gameControl.GamePlay.PrimaryFingerPosition.ReadValue<Vector2>(),
                secondaryPos = _gameControl.GamePlay.SecondaryFingerPosition.ReadValue<Vector2>(),
            });
            
            if (_pinchCoroutine != null)
            {
                MonoApp.Instance.StopCoroutine(_pinchCoroutine);
            }
        }
        
        private IEnumerator PinchDetection()
        {
            while (true)
            {
                if (UnityEngine.Input.touchCount == 2)
                {
                    EventCenter.DispatchEvent(new PinchChangedEvent
                    {
                        primaryPos = _gameControl.GamePlay.PrimaryFingerPosition.ReadValue<Vector2>(),
                        secondaryPos = _gameControl.GamePlay.SecondaryFingerPosition.ReadValue<Vector2>(),
                    });
                }
                else
                {
                    break;
                }
                yield return null;
            }
        }
    }
}