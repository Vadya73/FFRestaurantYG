using Gameplay;
using UnityEngine;

namespace Player
{
    public class TraySystem
    {
        private readonly GameObject _tray;
        private readonly Transform _cookedObjectPosition;
        private CookingObject _currentCookingObject;

        public bool IsTaken { get; private set; } = false;

        public TraySystem(GameObject tray, Transform cookedObjectPosition)
        {
            _tray = tray;
            _cookedObjectPosition = cookedObjectPosition;
        }

        public void SpawnObject(CookingObject cookableObjectPrefab)
        {
            _currentCookingObject = Object.Instantiate(cookableObjectPrefab, _tray.transform);
            _currentCookingObject.transform.position = _cookedObjectPosition.position;
            IsTaken = true;
        }

        public CookingObject GetCookingObject()
        {
            IsTaken = false;
            CookingObject cookingObject = _currentCookingObject;
            _currentCookingObject = null;
            
            HideTray();
            
            return cookingObject;
        }

        public void HideTray()
        {
            _tray.SetActive(false);
        }

        public void ShowTray()
        {
            _tray.SetActive(true);
        }

        public void SetCookedObject(CookingObject cookedObject)
        {
            _currentCookingObject = cookedObject;
            _currentCookingObject.transform.position = _cookedObjectPosition.position;
            _currentCookingObject.transform.SetParent(_tray.transform);
        }
    }
}