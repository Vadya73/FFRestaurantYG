using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private Transform[] _places;
        [SerializeField, ReadOnly] private Client[] _clients;
        
        private Dictionary<Transform, Client> _seats = new();

        private bool _isBooked;
        
        public Dictionary<Transform, Client> Seats => _seats;

        public void SetBooked(bool b)
        {
            _isBooked = b;
        }
    }
}