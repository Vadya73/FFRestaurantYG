using UnityEngine;
using UnityEngine.UIElements;

namespace Game.CodeBase.Input
{
    public class JoyStick : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _root;
        private VisualElement _joystickStick;
        private VisualElement _joystickVisualArea;
        private VisualElement _joystickTouchArea;

        private Vector2 _inputDelta;
        private Vector2 _defaultPos;
        private Vector2 _startPos;
        private float _maxRadius;
        private bool _isDragging;
        private bool _isInitialized = false;


        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;

            _joystickStick = _root.Q<VisualElement>("joystickStick");
            _joystickVisualArea = _root.Q<VisualElement>("joystickVisualArea");
            _joystickTouchArea = _root.Q<VisualElement>("joystickTouchArea");

            _joystickTouchArea.RegisterCallback<GeometryChangedEvent>(OnAttach);
            _joystickTouchArea.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _joystickTouchArea.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            _joystickTouchArea.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        private void OnAttach(GeometryChangedEvent geometryChangedEvent)
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            _maxRadius = Mathf.Min(_joystickVisualArea.resolvedStyle.width, _joystickVisualArea.resolvedStyle.height) / 2f;
            
            _defaultPos = new Vector2(
                _joystickStick.resolvedStyle.left,
                _joystickStick.resolvedStyle.top
            );
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            _isDragging = true;
            _startPos = evt.localPosition;
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
            
            _inputDelta = delta / _maxRadius;
        }

        private void OnPointerUp(EventBase evt)
        {
            _isDragging = false;
            _joystickStick.style.left = _defaultPos.x;
            _joystickStick.style.top = _defaultPos.y;
        }
    }
}
