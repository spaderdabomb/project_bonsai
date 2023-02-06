using ProjectBonsai.StateMachine;
using UnityEngine;

namespace ProjectBonsai.AI
{
    public class AiIdleState : AiState // Player Movement disables/re-enables any idle states when this state is set to inactive
    {
        [SerializeField] AnimationClip idleAnimation;
        protected override void OnStartState()
        {
            base.OnStartState();
        }

        protected override void OnStopState(bool bTransition) // bTransition is true if transitioning, false if stopping
        {
            base.OnStopState(bTransition);

            if (bTransition)
            {
                return;
            }
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
            m_AiController.animator.SetBool("isIdling", true);
        }

        protected override void OnStopAnimation()
        {
            base.OnStopAnimation();
            m_AiController.animator.SetBool("isIdling", false);
        }
    }
}