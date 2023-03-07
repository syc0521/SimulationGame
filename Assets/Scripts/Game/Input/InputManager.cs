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

    public struct WheelScrollEvent : IEvent
    {
        public float delta;
    }

    public struct MouseRotateEvent : IEvent
    {
        public float2 pos;
        public float time;
    }
    
    public class InputManager : ManagerBase, IInputManager
    {
        private GameControl _gameControl;
        private Coroutine _swipeCoroutine, _pinchCoroutine, _mouseRotateCoroutine;

        private bool isSwiping = false;
        private float2 _startPos;
        private bool _gestureEnable = true;

        public override void OnAwake()
        {
            _gameControl = new();
            _gameControl.Enable();
        }
        
        public override void OnStart()
        {
            _gameControl.GamePlay.PrimaryContact.canceled += Touch;

            _gameControl.GamePlay.PrimaryContact.started += SwipeStart;
            _gameControl.GamePlay.PrimaryContact.canceled += SwipeEnd;

            _gameControl.GamePlay.SecondaryTouchContact.started += PinchStart;
            _gameControl.GamePlay.SecondaryTouchContact.canceled += PinchEnd;
            
            _gameControl.GamePlay.PrimaryLong.performed += LongPress;
            _gameControl.GamePlay.WheelScroll.performed += WheelScroll;

            _gameControl.GamePlay.RightClick.started += MouseRotateStart;
            _gameControl.GamePlay.RightClick.canceled += MouseRotateEnd;
        }

        public override void OnDestroyed()
        {
            _gameControl.GamePlay.PrimaryContact.canceled -= Touch;

            _gameControl.GamePlay.PrimaryContact.started -= SwipeStart;
            _gameControl.GamePlay.PrimaryContact.canceled -= SwipeEnd;

            _gameControl.GamePlay.SecondaryTouchContact.started -= PinchStart;
            _gameControl.GamePlay.SecondaryTouchContact.canceled -= PinchEnd;

            _gameControl.GamePlay.PrimaryLong.performed -= LongPress;
            _gameControl.GamePlay.WheelScroll.performed -= WheelScroll;
            
            _gameControl.GamePlay.RightClick.started -= MouseRotateStart;
            _gameControl.GamePlay.RightClick.canceled -= MouseRotateEnd;

            _gameControl.Disable();
        }

        private bool IsPointerOverGameObject()
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            List<RaycastResult> raycastResult = new List<RaycastResult>();

            foreach (Touch touch in UnityEngine.Input.touches)
            {
                pointer.position = touch.position;
                EventSystem.current.RaycastAll(pointer, raycastResult);
                return raycastResult.Count > 0;   
            }
            
            pointer.position = UnityEngine.Input.mousePosition;
            EventSystem.current.RaycastAll(pointer, raycastResult);
            return raycastResult.Count > 0;
        }

        private void Touch(InputAction.CallbackContext ctx)
        {
            if (!isSwiping && CanSendInteractEvent())
            {
                MonoApp.Instance.StartCoroutine(TouchDetection());
            }
        }

        private IEnumerator TouchDetection()
        {
            yield return new WaitForSeconds(2 / 90.0f);
            if (!isSwiping && CanSendInteractEvent())
            {
                EventCenter.DispatchEvent(new TouchEvent
                {
                    pos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>()
                });
            }
        }
        
        private void LongPress(InputAction.CallbackContext ctx)
        {
            if (!isSwiping && CanSendInteractEvent())
            {
                EventCenter.DispatchEvent(new LongPressEvent
                {
                    pos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>()
                });
            }
        }

        private void SwipeStart(InputAction.CallbackContext ctx)
        {
            _startPos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>();
            if (CanSendInteractEvent())
            {
                EventCenter.DispatchEvent(new SwipeStartEvent
                {
                    pos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>(),
                    time = (float)ctx.startTime
                });
            }
            _swipeCoroutine = MonoApp.Instance.StartCoroutine(SwipeDetection(ctx));
        }
        
        private void SwipeEnd(InputAction.CallbackContext ctx)
        {
            isSwiping = false;
            if (CanSendInteractEvent())
            {
                EventCenter.DispatchEvent(new SwipeEndEvent
                {
                    pos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>(),
                    time = (float)ctx.startTime
                });
            }

            if (_swipeCoroutine != null)
            {
                MonoApp.Instance.StopCoroutine(_swipeCoroutine);
            }
        }

        private IEnumerator SwipeDetection(InputAction.CallbackContext ctx)
        {
            yield return new WaitForSeconds(1 / 90.0f);
            
            while (true)
            {
                if (UnityEngine.Input.touchCount > 1) break;
                float2 endPos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>();
                if (math.distance(_startPos, endPos) > 0.3f)
                {
                    isSwiping = true;
                }
                
                if (isSwiping && CanSendInteractEvent())
                {
                    EventCenter.DispatchEvent(new SwipeChangedEvent
                    {
                        pos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>(),
                        time = (float)ctx.startTime
                    });
                }
                yield return null;
            }
        }
        
        private void MouseRotateStart(InputAction.CallbackContext ctx)
        {
            _startPos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>();
            _mouseRotateCoroutine = MonoApp.Instance.StartCoroutine(MouseRotateDetection(ctx));
        }
        
        private void MouseRotateEnd(InputAction.CallbackContext ctx)
        {
            isSwiping = false;

            if (_mouseRotateCoroutine != null)
            {
                MonoApp.Instance.StopCoroutine(_mouseRotateCoroutine);
            }
        }
        
        private IEnumerator MouseRotateDetection(InputAction.CallbackContext ctx)
        {
            yield return new WaitForSeconds(2 / 90.0f);

            while (true)
            {
                if (UnityEngine.Input.touchCount > 1) break;
                float2 endPos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>();
                if (math.distance(_startPos, endPos) > 0.3f)
                {
                    isSwiping = true;
                }
                
                endPos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>();
                var distance = endPos - _startPos;
                
                if (isSwiping && CanSendInteractEvent())
                {
                    EventCenter.DispatchEvent(new MouseRotateEvent
                    {
                        pos = distance,
                        time = (float)ctx.startTime
                    });
                }

                _startPos = _gameControl.GamePlay.PrimaryPosition.ReadValue<Vector2>();
                yield return null;
            }
        }

        private void PinchStart(InputAction.CallbackContext ctx)
        {
            if (CanSendInteractEvent())
            {
                EventCenter.DispatchEvent(new PinchStartEvent
                {
                    primaryPos = _gameControl.GamePlay.PrimaryFingerPosition.ReadValue<Vector2>(),
                    secondaryPos = _gameControl.GamePlay.SecondaryFingerPosition.ReadValue<Vector2>(),
                });
            }
            _pinchCoroutine = MonoApp.Instance.StartCoroutine(PinchDetection());
        }
        
        private void PinchEnd(InputAction.CallbackContext ctx)
        {
            if (CanSendInteractEvent())
            {
                isSwiping = false;
                EventCenter.DispatchEvent(new PinchEndEvent
                {
                    primaryPos = _gameControl.GamePlay.PrimaryFingerPosition.ReadValue<Vector2>(),
                    secondaryPos = _gameControl.GamePlay.SecondaryFingerPosition.ReadValue<Vector2>(),
                });
            }
            
            if (_pinchCoroutine != null)
            {
                MonoApp.Instance.StopCoroutine(_pinchCoroutine);
            }
        }
        
        private IEnumerator PinchDetection()
        {
            while (true)
            {
                if (UnityEngine.Input.touchCount == 2 && CanSendInteractEvent())
                {
                    isSwiping = true;
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
        
        private void WheelScroll(InputAction.CallbackContext ctx)
        {
            if (_gestureEnable)
            {
                EventCenter.DispatchEvent(new WheelScrollEvent
                {
                    delta = _gameControl.GamePlay.WheelScroll.ReadValue<Vector2>()[1]
                });
            }
        }

        public void SetGestureState(bool enabled)
        {
            _gestureEnable = enabled;
        }

        private bool CanSendInteractEvent() => !IsPointerOverGameObject() && _gestureEnable;

    }
}