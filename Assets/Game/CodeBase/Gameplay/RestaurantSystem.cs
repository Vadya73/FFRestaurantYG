using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class RestaurantSystem : MonoBehaviour
    {
        [SerializeField] private List<Table> _allTables;
        [SerializeField] private List<Table> _freeTables;
        [SerializeField] private List<Table> _bookedTables;
        private ClientSystem _clientSystem;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _allTables = GetComponentsInChildren<Table>().ToList();
        }
#endif
        
        [Inject]
        private void Construct(ClientSystem clientSystem)
        {
            _clientSystem = clientSystem;
        }
        
        private void Awake()
        {
            _freeTables = new List<Table>(_allTables);
            _bookedTables = new List<Table>();
            _clientSystem.OnClientSpawn += SetClientTable;
        }

        private void OnDestroy()
        {
            _clientSystem.OnClientSpawn -= SetClientTable;
        }

        private void SetClientTable(Client client)
        {
            if (_freeTables == null || _freeTables.Count == 0) 
                return;
            
            var randomSeat = _freeTables[0].Seats.Keys.ElementAt(Random.Range(0, _freeTables[0].Seats.Count));
            client.AIMovementController.MoveTo(randomSeat.position);
            
            _freeTables[0].SetBooked(true);
            var table = _freeTables[0];
            _freeTables.RemoveAt(0);
            _bookedTables.Add(table);
        }
    }
}