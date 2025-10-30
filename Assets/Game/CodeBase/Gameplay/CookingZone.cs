using System;
using DG.Tweening;
using Timer;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Gameplay
{
    public class CookingZone : MonoBehaviour
    {
        [SerializeField] private CookingZoneConfig _zoneConfig;
        [SerializeField] private Transform _lookAtObject;
        [SerializeField] private UIDocument _zoneUI;
        
        private VisualElement _root;
        private Player.Player _player;
        private CountDownTimer _countDownTimer;
        private RadialFillElement _radialFillElement;
        private Sequence _cookingSequence;

        [Inject]
        private void Construct(Player.Player player)
        {
            _player = player;
        }

        private void Awake()
        {
            _countDownTimer = new CountDownTimer(this, _zoneConfig.CookingTime);
            _zoneUI.gameObject.SetActive(false);
            
            if (_zoneUI == null)
                _zoneUI = GetComponentInChildren<UIDocument>();
            
            _countDownTimer.OnStarted += ShowTimerUI;
            _countDownTimer.OnEnded += OnEndCooking;
            _countDownTimer.OnStopped += HideTimerUI;
            _countDownTimer.OnTimeChanged += UpdateVisualTimer;
        }
        
        private void OnDisable()
        {
            _countDownTimer.OnStarted -= ShowTimerUI;
            _countDownTimer.OnEnded -= OnEndCooking;
            _countDownTimer.OnStopped -= HideTimerUI;
            _countDownTimer.OnTimeChanged -= UpdateVisualTimer;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                player.MovementController.OnEndMove += StartCooking;
                player.MovementController.OnStartMove += StopCooking;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                player.MovementController.OnEndMove -= StartCooking;
                player.MovementController.OnStartMove -= StopCooking;
            }
        }

        private void ShowTimerUI()
        {
            _zoneUI.gameObject.SetActive(true);
            
            _root = _zoneUI.rootVisualElement;
            _radialFillElement = _root.Q<RadialFillElement>();
        }

        private void HideTimerUI()
        {
            _zoneUI.gameObject.SetActive(false);
        }

        private void OnEndCooking()
        {
            _zoneUI.gameObject.SetActive(false);
            
            _player.SetTrayActive(true);
            _player.SpawnObjectOnTray(_zoneConfig.CookableObject.Prefab);
        }

        private void UpdateVisualTimer()
        {
            _radialFillElement.value = Mathf.Clamp(_countDownTimer.Progress, 0 , _zoneConfig.CookingTime);
        }
        
        private void StartCooking()
        {
            if (_player.TraySystem.IsTaken)
                return;
            
            _cookingSequence?.Kill();
            _countDownTimer.ResetTime();
            
            _cookingSequence = DOTween.Sequence()
                .AppendCallback(() => _player.MovementController.StartMoveAtPoint(transform.position))
                .AppendInterval(.3f)
                .AppendCallback(() => _player.MovementController.LookAt(_lookAtObject.position, .5f))
                .AppendInterval(.3f)
                .OnComplete(() => _countDownTimer.Play());
        }

        private void StopCooking()
        {
            _cookingSequence?.Kill();
            _countDownTimer.Stop();
            _countDownTimer.ResetTime();
        }
    }
}
