using UnityEngine;

namespace ProjectBonsai.StateMachine
{
    /// <summary>
    /// <para>States are <see cref="MonoBehaviour"/>'s that are used by the <see cref="StateMachine"/> to control the flow of an entity/object.</para>
    /// <para>However, the states are automatically disabled and enabled by the <see cref="StateMachine"/>. And are only active when set to be.</para>
    /// <para>
    /// You have access to a couple <see cref="MonoBehaviour"/> functions, however, you should be using the built in <see cref="State"/> functions.
    /// <br />
    /// For example: <see cref="State.OnStartState"/> and <see cref="State.OnStopState"/>, view the <see cref="State"/> class for more information.
    /// </para>
    /// </summary>
    [DefaultExecutionOrder(100)]
    public abstract class State : MonoBehaviour
    {
        /*
         *  Properties
         */

        /// <summary>
        /// The owning state controller that's handling this state.
        /// </summary>
        protected StateController m_StateController = null!;

        /*
         *  Functions
         */

        #region State Controller Called Functions - Cannot be overriden

        public void InitializeState(StateController stateController)
        {
            m_StateController = stateController;
            OnInitializeState();
        }

        public void StartState()
        {
            enabled = true;
            OnStartState();
            StartAnimation();
        }

        public void StopState(bool bTransition = false)
        {
            enabled = false;
            OnStopState(bTransition);
            StopAnimation();
        }

        public bool CanSetStateActive()
        {
            return enabled == false && OnCanSetStateActive();
        }

        public void Update()
        {
            OnTickStateConditionals();
            TickAnimation();
            OnUpdateState();
        }

        public void FixedUpdate()
        {
            OnFixedUpdateState();
        }

        public void LateUpdate()
        {
            OnLateUpdateState();
        }

        private void StartAnimation()
        {
            OnStartAnimation();
        }

        private void StopAnimation()
        {
            OnStopAnimation();
        }

        private void TickAnimation()
        {
            OnTickAnimation();
        }

        #endregion

        #region State Functions - Can be overriden

        protected virtual void OnInitializeState() { }

        protected virtual void OnStartState() { }

        protected virtual void OnStopState(bool bTransition) { }

        protected virtual bool OnCanSetStateActive()
        {
            return true;
        }

        protected virtual void OnTickStateConditionals() { }

        protected virtual void OnUpdateState() { }

        protected virtual void OnFixedUpdateState() { }

        protected virtual void OnLateUpdateState() { }

        protected virtual void OnStartAnimation() { }

        protected virtual void OnStopAnimation() { }

        protected virtual void OnTickAnimation() { }

        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (enabled)
            {
                // states should never be enabled by default
                // they are enabled by the state controller
                // this is to prevent the state from running when it's not supposed to
                enabled = false;
            }
        }
#endif
    }
}