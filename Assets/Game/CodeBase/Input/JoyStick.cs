using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Input
{
    public class JoyStick : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _root;
        private VisualElement _joystickStick;
        private VisualElement _joystickVisualArea;
        private VisualElement _joystickTouchArea;
        
        private Button _interactButton;

        private float _halfWidth;
        private Vector2 _inputDelta;
        private Vector2 _defaultPos;
        private Vector2 _startPos;
        private float _maxRadius;
        private bool _isDragging;
        private bool _isInitialized = false;

        public event Action OnStartMoving;
        public event Action<Vector2> OnMoving;
        public event Action OnEndMoving;
        
        public event Action OnInteractButtonClicked;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;

            _joystickStick = _root.Q<VisualElement>("joystickStick");
            _joystickVisualArea = _root.Q<VisualElement>("joystickVisualArea");
            _joystickTouchArea = _root.Q<VisualElement>("joystickTouchArea");
            
            _interactButton = _root.Q<Button>("InteractButton");

            _joystickTouchArea.RegisterCallback<GeometryChangedEvent>(InitUI);
            _joystickTouchArea.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _joystickTouchArea.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            _joystickTouchArea.RegisterCallback<PointerUpEvent>(OnPointerUp);
            
            _interactButton.RegisterCallback<ClickEvent>(OnInteract);
        }
        
        private void InitUI(GeometryChangedEvent geometryChangedEvent)
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            _maxRadius = Mathf.Min(_joystickVisualArea.resolvedStyle.width, _joystickVisualArea.resolvedStyle.height) / 2f;
            
            _defaultPos = new Vector2(
                _joystickStick.resolvedStyle.left,
                _joystickStick.resolvedStyle.top
            );

            _halfWidth = _joystickVisualArea.resolvedStyle.width / 2f;
            
            _joystickVisualArea.style.display = DisplayStyle.None;
            _interactButton.style.display = DisplayStyle.None;
        }

        public void InteractButtonSetActive(bool active)
        {
            _interactButton.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            _isDragging = true;
            _startPos = evt.localPosition;

            _joystickVisualArea.style.left = _startPos.x - _halfWidth;
            _joystickVisualArea.style.top  = _startPos.y - _halfWidth;
            
            _joystickVisualArea.style.display = DisplayStyle.Flex;

            OnStartMoving?.Invoke();
        }


        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_isDragging)
                return;

            Vector2 currentPos = evt.localPosition;
            Vector2 delta = currentPos - _startPos;
            delta = Vector2.ClampMagnitude(delta, _maxRadius);

            _joystickStick.style.left = _defaultPos.x + delta.x;
            _joystickStick.style.top = _defaultPos.y + delta.y;
            
            _inputDelta = new Vector2(delta.x, -delta.y) / _maxRadius;
            
            OnMoving?.Invoke(_inputDelta);
        }

        private void OnPointerUp(EventBase evt)
        {
            _isDragging = false;
            _joystickStick.style.left = _defaultPos.x;
            _joystickStick.style.top = _defaultPos.y;
            _inputDelta = Vector2.zero;
            
            _joystickVisualArea.style.display = DisplayStyle.None;
            
            OnEndMoving?.Invoke();
        }
        
        private void OnInteract(ClickEvent evt) => OnInteractButtonClicked?.Invoke();
    }
}
