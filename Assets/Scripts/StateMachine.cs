using UnityEngine;

namespace My_Utils
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State atualState;

        public void SetState(State state)
        {
            atualState = state;
            state.Enter();
        }

        /// <summary>
        /// Initialze should be called in Awake or Start, and should set the first state.
        /// </summary>
        public abstract void InitializeMachine();

        protected virtual void Update()
        {
            atualState.HandleInput();
            atualState.LogicUpdate();
        }

        protected virtual void FixedUpdate()
        {
            atualState.PhysicsUpdate();
        }
    }
}
