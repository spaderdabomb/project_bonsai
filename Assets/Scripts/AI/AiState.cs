using ProjectBonsai.StateMachine;
using UnityEngine;

namespace ProjectBonsai.AI
{
    [RequireComponent(typeof(AiController))]
    public abstract class AiState : State
    {
        protected AiController m_AiController = null!;

        protected override void OnInitializeState()
        {
            base.OnInitializeState();
            m_AiController = m_StateController.GetComponent<AiController>();
        }
    }
}