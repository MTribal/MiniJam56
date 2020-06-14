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

        protected virtual void Start()
        {
            InitializeMachine();
        }

        /// <summary>
        /// Initialze is called in Start. Should set the first state.
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
