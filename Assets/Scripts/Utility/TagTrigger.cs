using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace ProjectBonsai
{
    public class TagTrigger : MonoBehaviour
    {
        [SerializeField] string triggerTag;

        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        // Event delegates triggered on click.
        [FormerlySerializedAs("OnTagEnter")]
        [SerializeField] private ButtonClickedEvent m_OnTagEnter = new ButtonClickedEvent();

        [FormerlySerializedAs("OnTagExit")]
        [SerializeField] private ButtonClickedEvent m_OnTagExit = new ButtonClickedEvent();

        public ButtonClickedEvent onTagTriggerEnter
        {
            get { return m_OnTagEnter; }
            set { m_OnTagEnter = value; }
        }

        public ButtonClickedEvent onTagTriggerExit
        {
            get { return m_OnTagExit; }
            set { m_OnTagExit = value; }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(triggerTag))
            {
                m_OnTagEnter.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(triggerTag))
            {
                m_OnTagExit.Invoke();
            }
        }
    }
}
