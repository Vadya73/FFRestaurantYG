using Input;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Gameplay
{
    public class HolderCookedObject : MonoBehaviour
    { 
        [SerializeField] private Transform _holder;
        private VisualElement _root;
        private JoyStick _joyStick;
        private Player.Player _player;
        
        private CookingObject _currentCookingObject;
        private bool _hasCookedObject;

        [Inject]
        private void Construct(JoyStick joyStick, Player.Player player)
        {
            _joyStick = joyStick;
            _player = player;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                ShowHint();
                _joyStick.OnInteractButtonClicked += GetCookedObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                HideHint();
                _joyStick.OnInteractButtonClicked -= GetCookedObject;
            }
        }

        private void GetCookedObject()
        {
            if (_hasCookedObject)
            {
                _player.TraySystem.ShowTray();
                _player.SetCookedObject(_currentCookingObject);
                _hasCookedObject = false;
                _currentCookingObject = null;
                return;
            }
            
            _currentCookingObject = _player.TraySystem.GetCookingObject();
            _currentCookingObject.transform.SetParent(_holder);
            _currentCookingObject.transform.position = _holder.position; // make animation
            _hasCookedObject = true;
        }

        private void ShowHint()
        {
            _joyStick.InteractButtonSetActive(true);
        }

        private void HideHint()
        {
            _joyStick.InteractButtonSetActive(false);
        }
    }
}