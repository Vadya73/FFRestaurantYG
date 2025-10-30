using Player;
using UnityEngine;
using VContainer;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _followTarget;
        [SerializeField] private Vector3 _followOffset;
        [SerializeField] private float _followSmoothTime = 0.2f;
        
        private MovementController _movementController;
        private Transform _objectTransform;
        private Vector3 _velocity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            transform.position = _followTarget.position + _followOffset;
        }
#endif

        [Inject]
        private void Construct(Player.Player player)
        {
            _movementController = player.MovementController;   
        }

        private void Awake()
        {
            _objectTransform = GetComponent<Transform>();
        }

        private void LateUpdate()
        {
            var pointToFollow = _followTarget.position + _followOffset;

            if (Vector3.Distance(_objectTransform.position, pointToFollow) > 0.1f)
                _objectTransform.position = Vector3.SmoothDamp(_objectTransform.position, pointToFollow, ref _velocity, _followSmoothTime);
        }
    }
}
