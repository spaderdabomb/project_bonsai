using UnityEngine;

namespace ProjectBonsai.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ProjectBonsai/Items/Item")]
    public class ScriptableItem : ScriptableObject
    {
        public string Name = "";
        public string ShortName = "";
        public string Description = "";
        public int MaxStack = 1;
        public int Weight = 0;
        public Sprite Icon = null;
        public GameObject Prefab = null;
        public bool IsStackable => MaxStack > 1;
    }
}