using Gameplay;
using Input;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private JoyStick _joystick;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private GameObject _visualModel;
        [Header("Tray")]
        [SerializeField] private GameObject _tray;
        [SerializeField] private Transform _cookedObjectPosition;
    
        private MovementController _movementController;
        private TraySystem _traySystem;
        private Rigidbody _rigidbody;
        
        public MovementController MovementController => _movementController;
        public TraySystem TraySystem => _traySystem;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        
            _movementController = new MovementController(_rigidbody, _speed, _visualModel);
            _traySystem = new TraySystem(_tray, _cookedObjectPosition);
            
            _joystick.OnStartMoving += StartMoving;
            _joystick.OnMoving += SetMoveDelta;
            _joystick.OnEndMoving += EndMoving;
        }

        private void Start()
        {
            if (_traySystem.IsTaken)
                _traySystem.ShowTray();
            else
                _traySystem.HideTray();
        }

        private void Update()
        {
            _movementController.Update(Time.deltaTime);
        }

        private void StartMoving()
        {
            _movementController.StartMoving();
            _movementController.StartRotate();
        }

        private void EndMoving()
        {
            _movementController.EndMoving();
            _movementController.EndRotate();
        }

        private void SetMoveDelta(Vector2 moveDelta)
        {
            _movementController.SetMoveDelta(new Vector3(moveDelta.x, 0f, moveDelta.y));
        }
        
        public void SetTrayActive(bool active) => _tray.SetActive(active);

        public void SpawnObjectOnTray(CookingObject cookableObjectPrefab)
        {
            _traySystem.SpawnObject(cookableObjectPrefab);
        }

        public void SetCookedObject(CookingObject cookedObject)
        {
            _traySystem.SetCookedObject(cookedObject);
        }
    }
}