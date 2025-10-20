using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "Cooking Object Config", fileName = "CookingObjectConfig", order = 0)]
    public class CookingObjectConfig : ScriptableObject
    {
        [SerializeField] private CookingObjectType _type;
        [SerializeField] private CookingObject _prefab;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _sellCost;
        
        public CookingObjectType Type => _type;
        public CookingObject Prefab => _prefab;
        public Sprite Icon => _icon;
    }

    public enum CookingObjectType
    {
        None = 0,
        PotatoFree = 1 << 0,
        CheeseBurger = 1 << 1,
        CocaCola = 1 << 2,
        IceCream = 1 << 3,
    }
}