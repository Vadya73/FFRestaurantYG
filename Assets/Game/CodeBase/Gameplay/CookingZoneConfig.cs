using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "Cooking Zone Config", fileName = "CookingZoneConfig", order = 0)]
    public class CookingZoneConfig : ScriptableObject
    {
        [SerializeField] private CookingObjectConfig _cookableObject;
        [SerializeField] private float _cookingTime;
        
        public CookingObjectConfig CookableObject => _cookableObject;
        public float CookingTime => _cookingTime;
    }
}