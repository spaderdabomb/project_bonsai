using ProjectBonsai.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectBonsai.AI
{
    public class AiWalkState : AiState // Player Movement disables/re-enables any idle states when this state is set to inactive
    {
        protected override void OnStartState()
        {
            base.OnStartState();
            m_AiController.agent.speed = m_AiController.aiData.BaseSpeed;
        }

        protected override void OnStopState(bool bTransition)
        {
            base.OnStopState(bTransition);
        }

        protected override void OnTickStateConditionals()
        {
            base.OnTickStateConditionals();

        }

        protected override bool OnCanSetStateActive()
        {
            return base.OnCanSetStateActive();
        }

        protected override void OnStartAnimation()
        {
            base.OnStartAnimation();
            m_AiController.animator.SetBool("isWalking", true);
        }

        protected override void OnStopAnimation()
        {
            base.OnStopAnimation();
            m_AiController.animator.SetBool("isWalking", false);
        }
    }
}