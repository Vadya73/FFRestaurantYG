using System;
using Infrastructure.ObjectPool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class ClientSystem : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Client _clientPrefab;
        [SerializeField] private int _clientsSize;
        [SerializeField] private Transform _clientsParent;
        
        private ObjectPool<Client> _clients;
        
        public event Action<Client> OnClientSpawn;

        private void Awake()
        {
            _clients = new ObjectPool<Client>(_clientPrefab, _clientsSize, _clientsParent);
            _clients.Initialize();
            
            SpawnClient();
        }

        private void SpawnClient()
        {
            Client client = _clients.Spawn();
            client.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
            OnClientSpawn?.Invoke(client);
        }

        private void DespawnClient(Client client)
        {
            _clients.Despawn(client);
        }
    }
}
