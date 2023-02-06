using ProjectBonsai.Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBonsai
{
    /// <summary>
    /// Maintains a list of all current colliders in trigger. Note that the trigger can be the gameobject itself, or the collision object
    /// </summary>
    public class CollidersInTrigger : MonoBehaviour
    {
        List<Collider> triggerList;

        private void Start()
        {
            triggerList = new List<Collider>();
        }

        void OnTriggerEnter(Collider trigger)
        {
            if (!triggerList.Contains(trigger))
            {
                triggerList.Add(trigger);
            }
        }

        void OnTriggerExit(Collider trigger)
        {
            if (triggerList.Contains(trigger))
            {
                triggerList.Remove(trigger);
            }
        }

        public List<Collider> GetList()
        {
            List<Collider> triggers = new List<Collider>();
            foreach (Collider trigger in triggerList)
            {
                triggers.Add(trigger);
            }
            return triggers;
        }

        public void RemoveItem(Collider trigger)
        {
            triggerList.Remove(trigger);
        }
    }
}

