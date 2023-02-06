using ProjectBonsai.Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBonsai
{
    public class TriggersInTrigger : MonoBehaviour
    {
        List<Collider> triggerList;

        private void Start()
        {
            triggerList = new List<Collider>();
        }

        void OnTriggerEnter(Collider trigger)
        {
            print("trigger entered");
            print(trigger.gameObject);

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
