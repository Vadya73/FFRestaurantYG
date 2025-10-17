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

            if (_zoneUI == null)
                _zoneUI = GetComponentInChildren<UIDocument>();
            
            _radialFillElement = _zoneUI.rootVisualElement.Q<RadialFillElement>();

            _countDownTimer.OnStarted += ShowTimerUI;
            _countDownTimer.OnEnded += OnEndCooking;
            _countDownTimer.OnStopped += HideTimerUI;
            _countDownTimer.OnTimeChanged += UpdateVisualTimer;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                player.PlayerController.OnEndMove += StartCooking;
                player.PlayerController.OnStartMove += StopCooking;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                player.PlayerController.OnEndMove -= StartCooking;
                player.PlayerController.OnStartMove -= StopCooking;
            }
        }

        private void ShowTimerUI()
        {
            _zoneUI.gameObject.SetActive(true);
        }

        private void HideTimerUI()
        {
            _zoneUI.gameObject.SetActive(false);
        }

        private void OnEndCooking()
        {
            _zoneUI.gameObject.SetActive(false);
        }

        private void UpdateVisualTimer()
        {
            _radialFillElement.value = Mathf.Clamp(_countDownTimer.Progress, 0 , _zoneConfig.CookingTime);
        }
        
        private void StartCooking()
        {
            _cookingSequence?.Kill();
            
            _cookingSequence = DOTween.Sequence()
                .AppendCallback(() => _player.PlayerController.StartMoveAtPoint(transform.position))
                .AppendInterval(.2f)
                .AppendCallback(() => _player.PlayerController.LookAt(_lookAtObject.position, .5f))
                .AppendInterval(.2f)
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
