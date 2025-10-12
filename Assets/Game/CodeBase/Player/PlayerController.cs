using UnityEngine;

namespace Player
{
    public class PlayerController
    {
        private Rigidbody _playerRigidbody;
        private Vector3 _movingDelta;
        private bool _isMoving;
        private float _speed;
    
        public PlayerController(Rigidbody playerRigidbody, float speed)
        {
            _playerRigidbody = playerRigidbody;
            _speed = speed;
        }

        public void Update(float deltaTime)
        {
            if (_isMoving)
            {
                MoveTo(_movingDelta, deltaTime);
            }
        }

        private void MoveTo(Vector3 deltaPosition, float deltaTime)
        {
            _playerRigidbody.MovePosition(_playerRigidbody.position + deltaPosition * (_speed * deltaTime));
        }

        public void StartMoving() => _isMoving = true;

        public void EndMoving() => _isMoving = false;

        public void SetMoveDelta(Vector3 moveDelta) => _movingDelta = moveDelta;
    }
}