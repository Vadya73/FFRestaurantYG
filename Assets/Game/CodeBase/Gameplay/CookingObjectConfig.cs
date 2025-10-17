using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "Cooking Object Config", fileName = "CookingObjectConfig", order = 0)]
    public class CookingObjectConfig : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
    }
}