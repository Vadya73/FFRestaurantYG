using System;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerController
    {
        private readonly Rigidbody _playerRigidbody;
        private readonly GameObject _visualModel;
        private readonly float _speed;
        private Vector3 _movingDelta;
        private Vector3 _pointToMove;
        
        private bool _isMoving;
        private bool _isMovingAtPoint;
        private bool _isRotating;

        public event Action OnStartMove;
        public event Action OnEndMove;
        public event Action OnStartRotate;
        public event Action OnEndRotate;
    
        public PlayerController(Rigidbody playerRigidbody, float speed, GameObject visualModel)
        {
            _playerRigidbody = playerRigidbody;
            _speed = speed;
            _visualModel = visualModel;
        }

        public void Update(float deltaTime)
        {
            if (_isMoving)
                MoveTo(_movingDelta, deltaTime);

            if (_isMovingAtPoint)
                MoveAtPoint(deltaTime);
            
            if (_isRotating)
                RotateToMoveDirection(deltaTime);
        }
        
        public void StartMoving()
        {
            _isMoving = true;
            OnStartMove?.Invoke();
        }

        public void EndMoving()
        {
            _isMoving = false;
            OnEndMove?.Invoke();
        }

        public void StartRotate()
        {
            _isRotating = true;
            OnStartRotate?.Invoke();
        }

        public void EndRotate()
        {
            _isRotating = false;
            OnEndRotate?.Invoke();
        }

        public void SetMoveDelta(Vector3 moveDelta) => _movingDelta = moveDelta;

        public void StartMoveAtPoint(Vector3 position)
        {
            _pointToMove = position;
            _isMovingAtPoint = true;
        }

        public void LookAt(Vector3 targetPosition, float rotateDuration)
        {
            Vector3 direction = targetPosition - _visualModel.transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Vector3 targetEuler = targetRotation.eulerAngles;

            _visualModel.transform.DORotate(targetEuler, rotateDuration);
        }
        
        private void MoveAtPoint(float deltaTime)
        {
            Vector3 direction = (_pointToMove - _playerRigidbody.position).normalized;

            MoveTo(direction / 2, deltaTime);

            RotateToDirection(direction, deltaTime);

            if (Vector3.Distance(_playerRigidbody.position, _pointToMove) < 0.1f)
            {
                _playerRigidbody.position = _pointToMove;
                _isMovingAtPoint = false;
            }
        }

        private void MoveTo(Vector3 normalizedDelta, float deltaTime)
        {
            _playerRigidbody.MovePosition(_playerRigidbody.position + normalizedDelta * (_speed * deltaTime));
        }

        private void RotateToMoveDirection(float deltaTime)
        {
            Vector3 direction = new Vector3(_movingDelta.x, 0f, _movingDelta.z);

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            _visualModel.transform.rotation = Quaternion.Lerp(
                _visualModel.transform.rotation,
                targetRotation,
                deltaTime * 10f
            );
        }
        
        private void RotateToDirection(Vector3 direction, float deltaTime)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            _visualModel.transform.rotation = Quaternion.Lerp(
                _visualModel.transform.rotation,
                targetRotation,
                deltaTime * 10f
            );
        }
    }
}