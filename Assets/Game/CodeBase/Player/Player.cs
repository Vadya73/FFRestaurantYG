using Input;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private JoyStick _joystick;
        [SerializeField] private float _speed = 10f;
    
        private PlayerController _playerController;
        private Rigidbody _rigidbody;
        
        public PlayerController PlayerController => _playerController;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        
            _playerController = new PlayerController(_rigidbody, _speed);
            
            _joystick.OnStartMoving += StartMoving;
            _joystick.OnMoving += SetMoveDelta;
            _joystick.OnEndMoving += EndMoving;
        }

        private void Update()
        {
            _playerController.Update(Time.deltaTime);
        }

        private void StartMoving()
        {
            _playerController.StartMoving();
        }

        private void EndMoving()
        {
            _playerController.EndMoving();
        }

        private void SetMoveDelta(Vector2 moveDelta)
        {
            _playerController.SetMoveDelta(new Vector3(moveDelta.x, 0f, moveDelta.y));
        }
    }
}