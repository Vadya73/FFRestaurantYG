using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "Cooking Zone Config", fileName = "CookingZoneConfig", order = 0)]
    public class CookingZoneConfig : ScriptableObject
    {
        [SerializeField] private CookingObjectConfig _cookableObject;
        [SerializeField] private float _cookingTime;
        
        public float CookingTime => _cookingTime;
    }
}