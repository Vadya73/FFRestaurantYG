using Infrastructure.ObjectPool;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Client : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _speed;
        private AIMovementController _aiMovementController;
        private NavMeshAgent _navMeshAgent;
        
        public AIMovementController AIMovementController => _aiMovementController;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _aiMovementController = new AIMovementController(_navMeshAgent, _speed);
        }

        public void OnSpawn()
        {
            
        }

        public void OnDespawn()
        {
            
        }
    }

    public class AIMovementController
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly float _speed;

        public AIMovementController(NavMeshAgent navMeshAgent, float speed)
        {
            _navMeshAgent = navMeshAgent;
            _speed = speed;
        }

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
        }
    }
}
