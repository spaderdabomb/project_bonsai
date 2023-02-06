using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace ProjectBonsai.StateMachine
{
    [SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
    [DefaultExecutionOrder(1)]
    public class StateController : MonoBehaviour
    {
        /*
         *  Properties
         */

        [field: SerializeField]
        public List<State> DefaultStates { get; private set; } = new();

        [field: SerializeField]
        public List<State> ActiveStates { get; private set; } = new();

        [field: SerializeField]
        public List<State> InactiveStates { get; private set; } = new();

        /*
         *  Functions
         */

        #region Unity Functions

        private void Awake()
        {
            foreach (State state in DefaultStates)
            {
                ActiveStates.Add(state);

                state.InitializeState(this);
                state.enabled = false;
            }

            foreach (State state in InactiveStates)
            {
                state.InitializeState(this);
                state.enabled = false;
            }
        }

        private void Start()
        {
            foreach (State state in ActiveStates)
            {
                state.StartState();
            }
        }

        #endregion

        #region State Management

        public void SetStateActive<T>() where T : State
        {
            SetStateActive(typeof(T));
        }

        public void SetStateActive(Type type)
        {
            if (!CanSetStateActive(type))
            {
                return;
            }

            if (ActiveStates.Exists(state => state.GetType() == type))
            {
                return;
            }

            State state = InactiveStates.Find(state => state.GetType() == type);

            if (state == null)
            {
                return;
            }

            ActiveStates.Add(state);
            InactiveStates.Remove(state);

            state.StartState();
        }

        public void SetStateActive(State state)
        {
            if (!CanSetStateActive(state))
            {
                return;
            }

            if (!InactiveStates.Contains(state))
            {
                return;
            }

            ActiveStates.Add(state);
            InactiveStates.Remove(state);

            state.StartState();
        }

        public void SetStateInactive<T>() where T : State
        {
            SetStateInactive(typeof(T));
        }

        public void SetStateInactive(Type type, bool bTransition = false)
        {
            if (InactiveStates.Exists(state => state.GetType() == type))
            {
                return;
            }

            State state = ActiveStates.Find(state => state.GetType() == type);

            if (state == null)
            {
                return;
            }

            InactiveStates.Add(state);
            ActiveStates.Remove(state);

            state.StopState(bTransition);
        }

        public void SetStateInactive(State state, bool bTransition = false)
        {
            if (InactiveStates.Contains(state))
            {
                return;
            }

            if (!ActiveStates.Contains(state))
            {
                return;
            }

            InactiveStates.Add(state);
            ActiveStates.Remove(state);

            state.StopState(bTransition);
        }

        public void TransitionToState<T>(State oldState) where T : State
        {
            TransitionToState(oldState, typeof(T));
        }

        public void TransitionToState(State oldState, Type newState)
        {
            SetStateInactive(oldState, true);
            SetStateActive(newState);
        }

        public void TransitionToState(State oldState, State newState)
        {
            SetStateInactive(oldState, true);
            SetStateActive(newState);
        }

        #endregion

        #region Helper Functions

        public bool CanSetStateActive<T>() where T : State
        {
            return CanSetStateActive(typeof(T));
        }

        public bool CanSetStateActive(Type stateType)
        {
            if (ActiveStates.Any(x => x.GetType() == stateType))
            {
                return false;
            }

            foreach (State state in InactiveStates)
            {
                if (state.GetType() != stateType)
                {
                    continue;
                }

                return state.CanSetStateActive();
            }

            return false;
        }

        public bool CanSetStateActive(State state)
        {
            return !ActiveStates.Contains(state) && state.CanSetStateActive();
        }


        public bool IsStateOrAnySubclassActive<T>() where T : State
        {
            return IsStateOrAnySubclassActive(typeof(T));
        }

        public bool IsStateOrAnySubclassActive(Type stateType)
        {
            foreach (State state in ActiveStates)
            {
                Type type = state.GetType();

                if (type == stateType || type.IsSubclassOf(stateType))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsStateActive<T>() where T : State
        {
            return IsStateActive(typeof(T));
        }

        public bool IsStateActive(Type type)
        {
            return ActiveStates.Exists(state => state.GetType() == type);
        }

        public bool IsStateActive(State state)
        {
            return ActiveStates.Contains(state);
        }

        public bool IsStateInactive<T>() where T : State
        {
            return IsStateInactive(typeof(T));
        }

        public bool IsStateInactive(Type type)
        {
            return InactiveStates.Exists(state => state.GetType() == type);
        }

        public bool IsStateInactive(State state)
        {
            return InactiveStates.Contains(state);
        }

        public T GetState<T>() where T : State
        {
            return (T)GetState(typeof(T));
        }

        public State GetState(Type type)
        {
            State foundState = ActiveStates.Find(state => state.GetType() == type);

            if (foundState == null)
            {
                foundState = InactiveStates.Find(state => state.GetType() == type);
            }

            return foundState;
        }

        public T GetActiveState<T>() where T : State
        {
            return (T)GetActiveState(typeof(T));
        }

        public State GetActiveState(Type type)
        {
            return ActiveStates.Find(state => state.GetType() == type);
        }

        public T GetInactiveState<T>() where T : State
        {
            return (T)GetInactiveState(typeof(T));
        }

        public State GetInactiveState(Type type)
        {
            return InactiveStates.Find(state => state.GetType() == type);
        }

        #endregion
    }
}